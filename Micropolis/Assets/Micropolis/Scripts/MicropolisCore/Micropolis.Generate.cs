namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Create a new map for a city.
        /// BUG We use a random number generator to draw a seed for initializing the
        ///     random number generator?
        /// </summary>
        public void generateMap()
        {
            generateSomeCity(getRandom16());
        }

        /// <summary>
        /// Generate a map for a city.
        /// </summary>
        /// <param name="seed">Random number generator initializing seed</param>
        private void generateSomeCity(int seed)
        {
            cityFileName = "";

            generateMap(seed);
            scenario = ScenarioType.SC_NONE;
            cityTime = 0;
            initSimLoad = 2;
            doInitialEval = true;

            initWillStuff();
            resetMapState();
            resetEditorState();
            invalidateMaps();
            updateFunds();
            doSimInit();

            simUpdate();

            callback("didGenerateMap", "");
        }

        /// <summary>
        /// Generate a map.
        /// </summary>
        /// <param name="seed">Initialization seed for the random generator.</param>
        private void generateMap(int seed)
        {
            generatedCitySeed = seed;

            seedRandom(seed);

            // Construct land.
            if (terrainCreateIsland < 0)
            {
                if (getRandom(100) < 10) // chance that island is generated
                {
                    makeIsland();
                    return;
                }
            }

            if (terrainCreateIsland == 1)
            {
                makeNakedIsland();
            }
            else
            {
                clearMap();
            }

            // Lay a river.
            if (terrainCurveLevel != 0)
            {
                var terrainXStart = 40 + getRandom(WORLD_W - 80);
                var terrainYStart = 33 + getRandom(WORLD_H - 67);

                var terrainPos = new Position(terrainXStart, terrainYStart);

                doRivers(terrainPos);
            }

            // Lay a few lakes.
            if (terrainLakeLevel != 0)
            {
                makeLakes();
            }

            smoothRiver();

            // And add trees.
            if (terrainTreeLevel != 0)
            {
                doTrees();
            }
        }

        /// <summary>
        /// Clear the whole world to ::DIRT tiles
        /// </summary>
        private void clearMap()
        {
            for (var x = 0; x < WORLD_W; x++)
            {
                for (var y = 0; y < WORLD_H; y++)
                {
                    map[x, y] = (ushort) MapTileCharacters.DIRT;
                }
            }
        }

        /// <summary>
        /// Clear everything from all land
        /// TODO function is never called
        /// </summary>
        private void clearUnnatural()
        {
            for (var x = 0; x < WORLD_W; x++)
            {
                for (var y = 0; y < WORLD_H; y++)
                {
                    if (map[x, y] > (ushort) MapTileCharacters.WOODS)
                    {
                        map[x, y] = (ushort)MapTileCharacters.DIRT;
                    }
                }
            }
        }

        /// <summary>
        /// Construct a plain island as world, surrounded by 5 tiles of river.
        /// </summary>
        private void makeNakedIsland()
        {
            const int terrainIslandRadius = ISLAND_RADIUS;
            int x, y;

            for (x = 0; x < WORLD_W; x++)
            {
                for (y = 0; y < WORLD_H; y++)
                {
                    if ((x < 5) || (x >= WORLD_W - 5) ||
                        (y < 5) || (y >= WORLD_H - 5))
                    {
                        map[x,y] = (ushort) MapTileCharacters.RIVER;
                    }
                    else
                    {
                        map[x,y] = (ushort) MapTileCharacters.DIRT;
                    }
                }
            }

            for (x = 0; x < WORLD_W - 5; x += 2)
            {
                int mapY = getERandom(terrainIslandRadius);
                plopBRiver(new Position(x, mapY));

                mapY = (WORLD_H - 10) - getERandom(terrainIslandRadius);
                plopBRiver(new Position(x, mapY));

                plopSRiver(new Position(x, 0));
                plopSRiver(new Position(x, WORLD_H - 6));
            }

            for (y = 0; y < WORLD_H - 5; y += 2)
            {
                int mapX = getERandom(terrainIslandRadius);
                plopBRiver(new Position(mapX, y));

                mapX = (WORLD_W - 10) - getERandom(terrainIslandRadius);
                plopBRiver(new Position(mapX, y));

                plopSRiver(new Position(0, y));
                plopSRiver(new Position(WORLD_W - 6, y));
            }
        }

        /// <summary>
        /// Construct a new world as an island
        /// </summary>
        private void makeIsland()
        {
            makeNakedIsland();
            smoothRiver();
            doTrees();
        }

        /// <summary>
        /// Make a number of lakes, depending on the terrainLakeLevel.
        /// </summary>
        private void makeLakes()
        {
            short numLakes;

            if (terrainLakeLevel < 0)
            {
                numLakes = getRandom(10);
            }
            else
            {
                numLakes = (short) (terrainLakeLevel / 2);
            }

            while (numLakes > 0)
            {
                int x = getRandom(WORLD_W - 21) + 10;
                int y = getRandom(WORLD_H - 20) + 10;

                makeSingleLake(new Position(x, y));

                numLakes--;
            }
        }

        /// <summary>
        /// Make a random lake at pos.
        /// </summary>
        /// <param name="pos">Rough position of the lake.</param>
        private void makeSingleLake(Position pos)
        {
            int numPlops = getRandom(12) + 2;

            while (numPlops > 0)
            {
                Position plopPos = new Position(pos, getRandom(12) -6, getRandom(12) - 6);

                if (getRandom(4) != 0)
                {
                    plopSRiver(plopPos);
                }
                else
                {
                    plopBRiver(plopPos);
                }

                numPlops--;
            }
        }

        /// <summary>
        /// Splash a bunch of trees down near (xloc, yloc).
        /// 
        /// Amount of trees is controlled by terrainTreeLevel.
        /// </summary>
        /// <remarks>Trees are not smoothed.</remarks>
        /// <param name="xloc">Horizontal position of starting point for splashing trees.</param>
        /// <param name="yloc">Vertical position of starting point for splashing trees.</param>
        private void treeSplash(short xloc, short yloc)
        {
            short numTrees;

            if (terrainTreeLevel < 0)
            {
                numTrees = (short) (getRandom(150) + 50);
            }
            else
            {
                numTrees = (short) (getRandom((short) (100 + (terrainTreeLevel * 2))) + 50);
            }

            Position treePos = new Position(xloc, yloc);

            while (numTrees > 0)
            {
                Direction2 dir = (Direction2)(Direction2.DIR2_NORTH + getRandom(7));
                treePos.move(dir);

                if (!treePos.testBounds())
                {
                    return;
                }

                if ((map[treePos.posX,treePos.posY] & (ushort)MapTileBits.LOMASK) == (ushort)MapTileCharacters.DIRT)
                {
                    map[treePos.posX,treePos.posY] = (ushort)MapTileCharacters.WOODS | (ushort)MapTileBits.BLBNBIT;
                }

                numTrees--;
            }
        }

        /// <summary>
        /// Splash trees around the world.
        /// </summary>
        private void doTrees()
        {
            short Amount, x, xloc, yloc;

            if (terrainTreeLevel < 0)
            {
                Amount = (short) (getRandom(100) + 50);
            }
            else
            {
                Amount = (short) (terrainTreeLevel + 3);
            }

            for (x = 0; x < Amount; x++)
            {
                xloc = getRandom(WORLD_W - 1);
                yloc = getRandom(WORLD_H - 1);
                treeSplash(xloc, yloc);
            }

            smoothTrees();
            smoothTrees();
        }

        private void smoothRiver()
        {
            short[] dx = { -1, 0, 1, 0 };
            short[] dy = { 0, 1, 0, -1 };
            short[] REdTab = {
                13 | (short)MapTileBits.BULLBIT,   13 | (short)MapTileBits.BULLBIT,     17 | (short)MapTileBits.BULLBIT,     15 | (short)MapTileBits.BULLBIT,
                5 | (short)MapTileBits.BULLBIT,    2,                19 | (short)MapTileBits.BULLBIT,     17 | (short)MapTileBits.BULLBIT,
                9 | (short)MapTileBits.BULLBIT,    11 | (short)MapTileBits.BULLBIT,     2,                13 | (short)MapTileBits.BULLBIT,
                7 | (short)MapTileBits.BULLBIT,    9 | (short)MapTileBits.BULLBIT,      5 | (short)MapTileBits.BULLBIT,      2 };

            short bitIndex, z, xTemp, yTemp;
            short temp, x, y;

            for (x = 0; x < WORLD_W; x++)
            {
                for (y = 0; y < WORLD_H; y++)
                {
                    if (map[x,y] == (short)MapTileCharacters.REDGE)
                    {
                        bitIndex = 0;

                        for (z = 0; z < 4; z++)
                        {
                            bitIndex = (short) (bitIndex << 1);
                            xTemp = (short) (x + dx[z]);
                            yTemp = (short) (y + dy[z]);
                            if (Position.testBounds(xTemp, yTemp) &&
                                ((map[xTemp,yTemp] & (short)MapTileBits.LOMASK) != (short)MapTileCharacters.DIRT) &&
                                (((map[xTemp,yTemp] & (short)MapTileBits.LOMASK) < (short)MapTileCharacters.WOODS_LOW) ||
                                 ((map[xTemp,yTemp] & (short)MapTileBits.LOMASK) > (short)MapTileCharacters.WOODS_HIGH)))
                            {
                                bitIndex++;
                            }
                        }

                        temp = REdTab[bitIndex & 15];

                        if ((temp != (short)MapTileCharacters.RIVER) &&
                            getRandom(1) != 0)
                        {
                            temp++;
                        }

                        map[x,y] = (ushort) temp;
                    }
                }
            }
        }

        private bool isTree(ushort cell)
        {
            if ((cell & (ushort)MapTileBits.LOMASK) >= (ushort)MapTileCharacters.WOODS_LOW && (cell & (ushort)MapTileBits.LOMASK) <= (ushort)MapTileCharacters.WOODS_HIGH)
            {
                return true;
            }
            return false;
        }

        private void smoothTrees()
        {
            short x, y;
            for (x = 0; x < WORLD_W; x++)
            {
                for (y = 0; y < WORLD_H; y++)
                {
                    if (isTree((ushort) map[x,y]))
                    {
                        smoothTreesAt(x, y, false);
                    }
                }
            }
        }

        /// <summary>
        /// Temporary function to prevent breaking a lot of code.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="preserve"></param>
        private void smoothTreesAt(int x, int y, bool preserve)
        {
            var effects = new ToolEffects(this);

            smoothTreesAt(x, y, preserve, effects);
            effects.modifyWorld();
        }

        /// <summary>
        /// Smooth trees at a position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="preserve"></param>
        /// <param name="effects"></param>
        private void smoothTreesAt(int x, int y, bool preserve, ToolEffects effects)
        {
            short[] dx = { -1, 0, 1, 0 };
            short[] dy = { 0, 1, 0, -1 };
            short[] treeTable = {
                0,  0,  0,  34,
                0,  0,  36, 35,
                0,  32, 0,  33,
                30, 31, 29, 37,
            };

            if (!isTree(effects.getMapValue(x, y)))
            {
                return;
            }

            int bitIndex = 0;
            int z;
            for (z = 0; z < 4; z++)
            {
                bitIndex = bitIndex << 1;
                int xTemp = x + dx[z];
                int yTemp = y + dy[z];
                if (Position.testBounds((short) xTemp, (short) yTemp)
                    && isTree(effects.getMapValue(xTemp, yTemp)))
                {
                    bitIndex++;
                }
            }

            int temp = treeTable[bitIndex & 15];
            if (temp != 0)
            {
                if (temp != (short)MapTileCharacters.WOODS)
                {
                    if (((x + y) & 1) != 0)
                    {
                        temp = temp - 8;
                    }
                }
                effects.setMapValue(x, y, (ushort)(temp | (ushort)MapTileBits.BLBNBIT));
            }
            else
            {
                if (!preserve)
                {
                    effects.setMapValue(x, y, (ushort) temp);
                }
            }
        }

        /// <summary>
        /// Construct rivers.
        /// </summary>
        /// <param name="terrainPos">Coordinate to start making a river.</param>
        private void doRivers(Position terrainPos)
        {
            Direction2 riverDir;   // Global direction of the river
            Direction2 terrainDir; // Local direction of the river

            riverDir = (Direction2)(Direction2.DIR2_NORTH + (getRandom(3) * 2));
            doBRiver(terrainPos, riverDir, riverDir);

            riverDir = DirectionUtils.rotate180(riverDir);
            terrainDir = doBRiver(terrainPos, riverDir, riverDir);

            riverDir = (Direction2)(Direction2.DIR2_NORTH + (getRandom(3) * 2));
            doSRiver(terrainPos, riverDir, terrainDir);
        }

        /// <summary>
        /// Make a big river.
        /// </summary>
        /// <param name="riverPos">Start position of making a river.</param>
        /// <param name="riverDir">Global direction of the river.</param>
        /// <param name="terrainDir">Local direction of the terrain.</param>
        /// <returns>Last used local terrain direction.</returns>
        private Direction2 doBRiver(Position riverPos, Direction2 riverDir, Direction2 terrainDir)
        {
            int rate1, rate2;

            if (terrainCurveLevel < 0)
            {
                rate1 = 100;
                rate2 = 200;
            }
            else
            {
                rate1 = terrainCurveLevel + 10;
                rate2 = terrainCurveLevel + 100;
            }

            Position pos = new Position(riverPos);

            while (Position.testBounds((short) (pos.posX + 4), (short) (pos.posY + 4)))
            {
                plopBRiver(pos);
                if (getRandom((short) rate1) < 10)
                {
                    terrainDir = riverDir;
                }
                else
                {
                    if (getRandom((short) rate2) > 90)
                    {
                        terrainDir = DirectionUtils.rotate45(terrainDir);
                    }
                    if (getRandom((short) rate2) > 90)
                    {
                        terrainDir = DirectionUtils.rotate45(terrainDir, 7);
                    }
                }
                pos.move(terrainDir);
            }

            return terrainDir;
        }

        /// <summary>
        /// Make a small river.
        /// </summary>
        /// <param name="riverPos">Start position of making a river.</param>
        /// <param name="riverDir">Global direction of the river.</param>
        /// <param name="terrainDir">Local direction of the terrain.</param>
        /// <returns>Last used local terrain direction.</returns>
        private Direction2 doSRiver(Position riverPos, Direction2 riverDir, Direction2 terrainDir)
        {
            int rate1, rate2;

            if (terrainCurveLevel < 0)
            {
                rate1 = 100;
                rate2 = 200;
            }
            else
            {
                rate1 = terrainCurveLevel + 10;
                rate2 = terrainCurveLevel + 100;
            }

            Position pos = new Position(riverPos);

            while (Position.testBounds((short) (pos.posX + 3), (short) (pos.posY + 3)))
            {
                //printf("doSRiver %d %d td %d rd %d\n", pos.posX, pos.posY, terrainDir, riverDir);
                plopSRiver(pos);
                if (getRandom((short) rate1) < 10)
                {
                    terrainDir = riverDir;
                }
                else
                {
                    if (getRandom((short) rate2) > 90)
                    {
                        terrainDir = DirectionUtils.rotate45(terrainDir);
                    }
                    if (getRandom((short) rate2) > 90)
                    {
                        terrainDir = DirectionUtils.rotate45(terrainDir, 7);
                    }
                }
                pos.move(terrainDir);
            }

            return terrainDir;
        }

        /// <summary>
        /// Put down a big river diamond-like shape.
        /// </summary>
        /// <param name="pos">Base coordinate of the blob (top-left position).</param>
        private void plopBRiver(Position pos)
        {
            short x, y;
            ushort[,] BRMatrix =
            {
                {
                    0, 0, 0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.REDGE,
                    (ushort) MapTileCharacters.REDGE, 0, 0, 0
                },
                {
                    0, 0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.REDGE, 0, 0
                },
                {
                    0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.REDGE, 0
                },
                {
                    (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.REDGE
                },
                {
                    (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.CHANNEL, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.REDGE
                },
                {
                    (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.REDGE
                },
                {
                    0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.REDGE,
                    0
                },
                {
                    0, 0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.REDGE, 0, 0
                },
                {
                    0, 0, 0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.REDGE,
                    (ushort) MapTileCharacters.REDGE, 0, 0, 0
                },
            };

            for (x = 0; x < 9; x++)
            {
                for (y = 0; y < 9; y++)
                {
                    putOnMap(BRMatrix[y,x], (short) (pos.posX + x), (short) (pos.posY + y));
                }

            }
        }

        /// <summary>
        /// Put mChar onto the map at position xLoc, yLoc if possible.
        /// </summary>
        /// <param name="mChar">Map value to put ont the map.</param>
        /// <param name="xLoc">Horizontal position at the map to put mChar</param>
        /// <param name="yLoc">Vertical position at the map to put mChar</param>
        private void putOnMap(ushort mChar, short xLoc, short yLoc)
        {
            if (mChar == 0)
            {
                return;
            }

            if (!Position.testBounds(xLoc, yLoc))
            {
                return;
            }

            ushort temp = map[xLoc,yLoc];

            if (temp != (ushort) MapTileCharacters.DIRT)
            {
                temp = (ushort)(temp & (ushort)MapTileBits.LOMASK);
                if (temp == (ushort)MapTileCharacters.RIVER)
                {
                    if (mChar != (ushort)MapTileCharacters.CHANNEL)
                    {
                        return;
                    }
                }
                if (temp == (ushort)MapTileCharacters.CHANNEL)
                {
                    return;
                }
            }
            map[xLoc,yLoc] = mChar;
        }

        /// <summary>
        /// Put down a small river diamond-like shape.
        /// </summary>
        /// <param name="pos">Base coordinate of the blob (top-left position).</param>
        public void plopSRiver(Position pos)
        {
            short x, y;
            ushort[,] SRMatrix =
            {
                {
                    0, 0, 0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.REDGE,
                    (ushort) MapTileCharacters.REDGE, 0, 0, 0
                },
                {
                    0, 0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.REDGE, 0, 0
                },
                {
                    0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.REDGE, 0
                },
                {
                    (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.REDGE
                },
                {
                    (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.CHANNEL, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.REDGE
                },
                {
                    (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.REDGE
                },
                {
                    0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.REDGE, 0
                },
                {
                    0, 0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.RIVER, (ushort) MapTileCharacters.RIVER,
                    (ushort) MapTileCharacters.REDGE, 0, 0
                },
                {
                    0, 0, 0, (ushort) MapTileCharacters.REDGE, (ushort) MapTileCharacters.REDGE,
                    (ushort) MapTileCharacters.REDGE, 0, 0, 0
                },
            };

            for (x = 0; x < 6; x++)
            {
                for (y = 0; y < 6; y++)
                {
                    putOnMap(SRMatrix[y,x], (short) (pos.posX + x), (short) (pos.posY + y));
                }
            }
        }

        /// <summary>
        /// TODO function is never called
        /// </summary>
        public void smoothWater()
        {
            int x, y;
            ushort tile;
            Direction2 dir;

            for (x = 0; x < WORLD_W; x++)
            {
                for (y = 0; y < WORLD_H; y++)
                {

                    tile = (ushort)(map[x,y] & (ushort)MapTileBits.LOMASK);

                    /* If (x, y) is water: */
                    if (tile >= (ushort)MapTileCharacters.WATER_LOW && tile <= (ushort)MapTileCharacters.WATER_HIGH)
                    {

                        var pos = new Position(x, y);
                        for (dir = Direction2.DIR2_BEGIN; dir < Direction2.DIR2_END; dir = DirectionUtils.increment90(dir))
                        {
                            /* If getting a tile off-map, condition below fails. */
                            // @note I think this may have been a bug, since it always uses DIR2_WEST instead of dir.
                            //tile = getTileFromMap(pos, DIR2_WEST, WATER_LOW);
                            tile = getTileFromMap(pos, dir, (ushort) MapTileCharacters.WATER_LOW);

                            /* If nearest object is not water: */
                            if (tile < (ushort)MapTileCharacters.WATER_LOW || tile > (ushort)MapTileCharacters.WATER_HIGH)
                            {
                                map[x,y] = (ushort) MapTileCharacters.REDGE; /* set river edge */
                                break; // Continue with next tile
                            }
                        }
                    }
                }
            }

            for (x = 0; x < WORLD_W; x++)
            {
                for (y = 0; y < WORLD_H; y++)
                {

                    tile = (ushort)(map[x,y] & (ushort)MapTileBits.LOMASK);

                    /* If water which is not a channel: */
                    if (tile != (ushort)MapTileCharacters.CHANNEL && tile >= (ushort)MapTileCharacters.WATER_LOW && tile <= (ushort)MapTileCharacters.WATER_HIGH)
                    {

                        bool makeRiver = true; // make (x, y) a river

                        Position pos = new Position(x, y);
                        for (dir = Direction2.DIR2_BEGIN; dir < Direction2.DIR2_END; dir = DirectionUtils.increment90(dir))
                        {

                            /* If getting a tile off-map, condition below fails. */
                            // @note I think this may have been a bug, since it always uses DIR2_WEST instead of dir.
                            //tile = getTileFromMap(pos, DIR2_WEST, WATER_LOW);
                            tile = getTileFromMap(pos, dir, (ushort)MapTileCharacters.WATER_LOW);

                            /* If nearest object is not water: */
                            if (tile < (ushort)MapTileCharacters.WATER_LOW || tile > (ushort)MapTileCharacters.WATER_HIGH)
                            {
                                makeRiver = false;
                                break;
                            }
                        }

                        if (makeRiver)
                        {
                            map[x,y] = (ushort) MapTileCharacters.RIVER; /* make it a river */
                        }
                    }
                }
            }

            for (x = 0; x < WORLD_W; x++)
            {
                for (y = 0; y < WORLD_H; y++)
                {

                    tile = (ushort)(map[x,y] & (ushort)MapTileBits.LOMASK);

                    /* If woods: */
                    if (tile >= (ushort)MapTileCharacters.WOODS_LOW && tile <= (ushort)MapTileCharacters.WOODS_HIGH)
                    {

                        Position pos = new Position(x, y);
                        for (dir = Direction2.DIR2_BEGIN; dir < Direction2.DIR2_END; dir = DirectionUtils.increment90(dir))
                        {

                            /* If getting a tile off-map, condition below fails. */
                            // @note I think this may have been a bug, since it always uses DIR2_WEST instead of dir.
                            //tile = getTileFromMap(pos, DIR2_WEST, WATER_LOW);
                            tile = getTileFromMap(pos, dir, unchecked((ushort)MapTileCharacters.TILE_INVALID));

                            if (tile == (ushort)MapTileCharacters.RIVER || tile == (ushort)MapTileCharacters.CHANNEL)
                            {
                                map[x,y] = (ushort) MapTileCharacters.REDGE; /* make it water's edge */
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
