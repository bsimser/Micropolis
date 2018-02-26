using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Make firerate map from firestation map.
        /// </summary>
        public void fireAnalysis()
        {
            smoothStationMap(fireStationMap);
            smoothStationMap(fireStationMap);
            smoothStationMap(fireStationMap);

            fireStationEffectMap = fireStationMap;

            newMapFlags[(int) MapType.MAP_TYPE_FIRE_RADIUS] = 1;
            newMapFlags[(int) MapType.MAP_TYPE_DYNAMIC] = 1;
        }

        /// <summary>
        /// Smooth a station map.
        /// 
        /// Used for smoothing fire station and police station coverage maps.
        /// </summary>
        /// <param name="map">Map to smooth.</param>
        private void smoothStationMap(MapShort8 map)
        {
            short x, y, edge;
            var tempMap = new MapShort8(map);

            for (x = 0; x < tempMap.MAP_W; x++)
            {
                for (y = 0; y < tempMap.MAP_H; y++)
                {
                    edge = 0;
                    if (x > 0)
                    {
                        edge += tempMap.get(x - 1, y);
                    }
                    if (x < tempMap.MAP_W - 1)
                    {
                        edge += tempMap.get(x + 1, y);
                    }
                    if (y > 0)
                    {
                        edge += tempMap.get(x, y - 1);
                    }
                    if (y < tempMap.MAP_H - 1)
                    {
                        edge += tempMap.get(x, y + 1);
                    }
                    edge = (short) (tempMap.get(x, y) + edge / 4);
                    map.set(x, y, (short) (edge / 2));
                }
            }
        }

        public void populationDensityScan()
        {
            tempMap1.clear();
            long Xtot = 0;
            long Ytot = 0;
            long Ztot = 0;
            for (int x = 0; x < WORLD_W; x++)
            {
                for (int y = 0; y < WORLD_H; y++)
                {
                    ushort mapValue = map[x,y];
                    if ((ushort)(mapValue & (ushort) MapTileBits.ZONEBIT) != 0)
                    {
                        ushort mapTile = (ushort)(mapValue & (ushort)MapTileBits.LOMASK);
                        int pop = getPopulationDensity(new Position(x, y), mapTile) * 8;
                        pop = Math.Min(pop, 254);

                        tempMap1.worldSet(x, y, (Byte)pop);
                        Xtot += x;
                        Ytot += y;
                        Ztot++;
                    }
                }
            }

            doSmooth1(); // tempMap1 -> tempMap2
            doSmooth2(); // tempMap2 -> tempMap1
            doSmooth1(); // tempMap1 -> tempMap2

            //assert(populationDensityMap.MAP_W == tempMap2.MAP_W);
            //assert(populationDensityMap.MAP_H == tempMap2.MAP_H);

            // Copy tempMap2 to populationDensityMap
            for (int x = 0; x < tempMap2.MAP_W; x++)
            {
                for (int y = 0; y < tempMap2.MAP_H; y++)
                {
                    populationDensityMap.set(x, y, tempMap2.get(x, y));
                }
            }

            computeComRateMap();          /* Compute the comRateMap */


            // Compute new city center
            if (Ztot > 0)
            {               /* Find Center of Mass for City */
                cityCenterX = (short)(Xtot / Ztot);
                cityCenterY = (short)(Ytot / Ztot);
            }
            else
            {
                cityCenterX = WORLD_W / 2;  /* if pop==0 center of map is city center */
                cityCenterY = WORLD_H / 2;
            }

            // Set flags for updated maps
            newMapFlags[(int)MapType.MAP_TYPE_POPULATION_DENSITY] = 1;
            newMapFlags[(int)MapType.MAP_TYPE_RATE_OF_GROWTH] = 1;
            newMapFlags[(int)MapType.MAP_TYPE_DYNAMIC] = 1;
        }

        /// <summary>
        /// Get population of a zone.
        /// </summary>
        /// <param name="pos">Position of the zone to count.</param>
        /// <param name="tile">Tile of the zone.</param>
        /// <returns>Population of the zone.</returns>
        private int getPopulationDensity(Position pos, ushort tile)
        {
            int pop;

            if (tile == (ushort) MapTileCharacters.FREEZ)
            {
                pop = doFreePop(pos);
                return pop;
            }

            if (tile < (ushort) MapTileCharacters.COMBASE)
            {
                pop = getResZonePop(tile);
                return pop;
            }

            if (tile < (ushort) MapTileCharacters.INDBASE)
            {
                pop = getComZonePop(tile) * 8;
                return pop;
            }

            if (tile < (ushort) MapTileCharacters.PORTBASE)
            {
                pop = getIndZonePop(tile) * 8;
                return pop;
            }

            return 0;
        }

        /// <summary>
        /// Does pollution, terrain, land value
        /// </summary>
        public void pollutionTerrainLandValueScan()
        {
            //TODO
        }

        /// <summary>
        /// Return pollution of a tile value
        /// </summary>
        /// <param name="loc">Tile character</param>
        /// <returns>Value of the pollution (0..255, bigger is worse)</returns>
        public int getPollutionValue(int loc)
        {
            if (loc < (int) MapTileCharacters.POWERBASE)
            {

                if (loc >= (int)MapTileCharacters.HTRFBASE)
                {
                    return /* 25 */ 75;     /* heavy traf  */
                }

                if (loc >= (int)MapTileCharacters.LTRFBASE)
                {
                    return /* 10 */ 50;     /* light traf  */
                }

                if (loc < (int)MapTileCharacters.ROADBASE)
                {

                    if (loc > (int)MapTileCharacters.FIREBASE)
                    {
                        return /* 60 */ 90;
                    }

                    /* XXX: Why negative pollution from radiation? */
                    if (loc >= (int)MapTileCharacters.RADTILE)
                    {
                        return /* -40 */ 255; /* radioactivity  */
                    }

                }
                return 0;
            }

            if (loc <= (int)MapTileCharacters.LASTIND)
            {
                return 0;
            }

            if (loc < (int)MapTileCharacters.PORTBASE)
            {
                return 50;        /* Ind  */
            }

            if (loc <= (int)MapTileCharacters.LASTPOWERPLANT)
            {
                return /* 60 */ 100;      /* prt, aprt, cpp */
            }

            return 0;
        }

        /// <summary>
        /// Compute Manhattan distance between given world position and center of the city.
        /// </summary>
        /// <remarks>
        /// For long distances (&gt; 64), value 64 is returned.
        /// </remarks>
        /// <param name="x">X world coordinate of given position</param>
        /// <param name="y">Y world coordinate of given position</param>
        /// <returns>Manhattan distance (dx+dy) between both positions</returns>
        public int getCityCenterDistance(int x, int y)
        {
            int xDis, yDis;

            if (x > cityCenterX)
            {
                xDis = x - cityCenterX;
            }
            else
            {
                xDis = cityCenterX - x;
            }

            if (y > cityCenterY)
            {
                yDis = y - cityCenterY;
            }
            else
            {
                yDis = cityCenterY - y;
            }

            return Math.Min(xDis + yDis, 64);
        }

        /// <summary>
        /// Smooth police station map and compute crime rate
        /// </summary>
        public void crimeScan()
        {
            // TODO
        }

        public void smoothTerrain()
        {
            // TODO
        }

        /// <summary>
        /// Smooth tempMap1 to tempMap2
        /// </summary>
        public void doSmooth1()
        {
            smoothDitherMap(tempMap1, tempMap2, (donDither & 2) != 0);
        }

        /// <summary>
        /// Smooth tempMap2 to tempMap1
        /// </summary>
        public void doSmooth2()
        {
            smoothDitherMap(tempMap2, tempMap1, (donDither & 4) != 0);
        }

        /// <summary>
        /// Perform smoothing with or without dithering.
        /// </summary>
        /// <param name="srcMap">Source map.</param>
        /// <param name="destMap">Destination map.</param>
        /// <param name="ditherFlag">Function should apply dithering.</param>
        private void smoothDitherMap(MapByte2 srcMap, MapByte2 destMap, bool ditherFlag)
        {
            if (ditherFlag)
            {
                int x, y = 0, z = 0, dir = 1;

                for (x = 0; x < srcMap.MAP_W; x++)
                {
                    for (; y != srcMap.MAP_H && y != -1; y += dir)
                    {
                        z += srcMap.get((x == 0) ? x : (x - 1), y) +
                             srcMap.get((x == srcMap.MAP_W - 1) ? x : (x + 1), y) +
                             srcMap.get(x, (y == 0) ? (0) : (y - 1)) +
                             srcMap.get(x, (y == (srcMap.MAP_H - 1)) ? y : (y + 1)) +
                             srcMap.get(x, y);
                        destMap.set(x, y, (byte)(z / 4));
                        z &= 3;
                    }
                    dir = -dir;
                    y += dir;
                }
            }
            else
            {
                short x, y, z;

                for (x = 0; x < srcMap.MAP_W; x++)
                {
                    for (y = 0; y < srcMap.MAP_H; y++)
                    {
                        z = 0;
                        if (x > 0)
                        {
                            z += srcMap.get(x - 1, y);
                        }
                        if (x < srcMap.MAP_W - 1)
                        {
                            z += srcMap.get(x + 1, y);
                        }
                        if (y > 0)
                        {
                            z += srcMap.get(x, y - 1);
                        }
                        if (y < (srcMap.MAP_H - 1))
                        {
                            z += srcMap.get(x, y + 1);
                        }
                        z = (short) ((z + srcMap.get(x, y)) >> 2);
                        if (z > 255)
                        {
                            z = 255;
                        }
                        destMap.set(x, y, (byte) z);
                    }
                }
            }
        }

        /// <summary>
        /// Compute distance to city center for the entire map.
        /// </summary>
        public void computeComRateMap()
        {
            short x, y, z;

            for (x = 0; x < comRateMap.MAP_W; x++)
            {
                for (y = 0; y < comRateMap.MAP_H; y++)
                {
                    z = (short)(getCityCenterDistance(x * 8, y * 8) / 2); // 0..32
                    z = (short) (z * 4);  // 0..128
                    z = (short) (64 - z); // 64..-64
                    comRateMap.set(x, y, z);
                }
            }
        }
    }
}
