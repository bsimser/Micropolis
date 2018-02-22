namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Create and initialize a sprite.
        /// </summary>
        /// <param name="name">Name of the sprite</param>
        /// <param name="type">Type of the sprite.</param>
        /// <param name="x">X coordinate of the sprite (in pixels).</param>
        /// <param name="y">Y coordinate of the sprite (in pixels).</param>
        /// <returns>New sprite object.</returns>
        public SimSprite newSprite(string name, int type, int x, int y)
        {
            return new SimSprite();
        }

        /// <summary>
        /// Re-initialize an existing sprite.
        /// </summary>
        /// <param name="sprite">Sprite to re-use.</param>
        /// <param name="x">New x coordinate of the sprite (in pixels?).</param>
        /// <param name="y">New y coordinate of the sprite (in pixels?).</param>
        public void initSprite(SimSprite sprite, int x, int y)
        {
        }

        /// <summary>
        /// Destroy all sprites by de-activating them all (setting their frame to 0).
        /// </summary>
        public void destroyAllSprite()
        {
        }

        /// <summary>
        /// Destroy the sprite by taking it out of the active list.
        /// </summary>
        /// <param name="sprite">Sprite to destroy.</param>
        public void destroySprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// Return the sprite of the give type, if available.
        /// </summary>
        /// <param name="type">Type of the sprite.</param>
        /// <returns>Pointer to the active sprite if avaiable, else NULL.</returns>
        public SimSprite getSprite(int type)
        {
            return new SimSprite();
        }

        /// <summary>
        /// Make a sprite either by re-using the old one, or by making a new one.
        /// </summary>
        /// <param name="type">Sprite type of the new sprite.</param>
        /// <param name="x">X coordinate of the new sprite.</param>
        /// <param name="y">Y coordinate of the new sprite.</param>
        /// <returns></returns>
        public SimSprite makeSprite(int type, int x, int y)
        {
            return new SimSprite();
        }

        /// <summary>
        /// Get character from the map.
        /// </summary>
        /// <param name="x">X coordinate in pixels.</param>
        /// <param name="y">Y coordinate in pixels.</param>
        /// <returns>Map character if on-map, or -1 if off-map.</returns>
        public short getChar(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Turn.
        /// </summary>
        /// <param name="p">Present direction (1..8).</param>
        /// <param name="d">Destination direction (1..8).</param>
        /// <returns>New direction.</returns>
        public short turnTo(int p, int d)
        {
            return 0;
        }

        public bool tryOther(int Tpoo, int Told, int Tnew)
        {
            return false;
        }

        /// <summary>
        /// Check whether a sprite is still entirely on-map.
        /// </summary>
        /// <param name="sprite">Sprite to check.</param>
        /// <returns>Sprite is at least partly off-map.</returns>
        public bool spriteNotInBounds(SimSprite sprite)
        {
            return false;
        }

        /// <summary>
        /// Get direction (0..8?) to get from starting point to destination point.
        /// </summary>
        /// <param name="orgX">X coordinate starting point.</param>
        /// <param name="orgY">Y coordinate starting point.</param>
        /// <param name="desX">X coordinate destination point.</param>
        /// <param name="desY">Y coordinate destination point.</param>
        /// <returns>Direction to go in.</returns>
        public short getDir(int orgX, int orgY, int desX, int desY)
        {
            return 0;
        }

        /// <summary>
        /// Compute Manhattan distance between two points.
        /// </summary>
        /// <param name="x1">X coordinate first point.</param>
        /// <param name="y1">Y coordinate first point.</param>
        /// <param name="x2">X coordinate second point.</param>
        /// <param name="y2">Y coordinate second point.</param>
        /// <returns>Manhattan distance between both points.</returns>
        public int getDistance(int x1, int y1, int x2, int y2)
        {
            return 0;
        }

        /// <summary>
        /// Check whether two sprites collide with each other.
        /// </summary>
        /// <param name="s1">First sprite.</param>
        /// <param name="s2">Second sprite.</param>
        /// <returns>Sprites are colliding.</returns>
        public bool checkSpriteCollision(SimSprite s1, SimSprite s2)
        {
            return false;
        }

        /// <summary>
        /// Move all sprites.
        /// 
        /// Sprites with frame == 0 are removed.
        /// </summary>
        public void moveObjects()
        {
        }

        /// <summary>
        /// Move train sprite.
        /// </summary>
        /// <param name="sprite">Train sprite.</param>
        public void doTrainSprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// Move helicopter sprite.
        /// </summary>
        /// <param name="sprite">Helicopter sprite.</param>
        public void doCopterSprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// Move airplane sprite.
        /// </summary>
        /// <param name="sprite">Airplane sprite</param>
        public void doAirplaneSprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// Move ship sprite.
        /// </summary>
        /// <param name="sprite">Ship sprite</param>
        public void doShipSprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// Move monster sprite.
        /// 
        /// There are 16 monster sprite frames:
        /// 
        /// Frame 0: NorthEast Left Foot
        /// Frame 1: NorthEast Both Feet
        /// Frame 2: NorthEast Right Foot
        /// Frame 3: SouthEast Right Foot
        /// Frame 4: SouthEast Both Feet
        /// Frame 5: SouthEast Left Foot
        /// Frame 6: SouthWest Right Foot
        /// Frame 7: SouthWest Both Feet
        /// Frame 8: SouthWest Left Foot
        /// Frame 9: NorthWest Left Foot
        /// Frame 10: NorthWest Both Feet
        /// Frame 11: NorthWest Right Foot
        /// Frame 12: North Left Foot
        /// Frame 13: East Left Foot
        /// Frame 14: South Right Foot
        /// Frame 15: West Right Foot
        /// </summary>
        /// <param name="sprite">Monster sprite</param>
        public void doMonsterSprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// Move tornado.
        /// </summary>
        /// <param name="sprite">Tornado sprite to move.</param>
        public void doTornadoSprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// 'Move' fire sprite
        /// </summary>
        /// <param name="sprite">Fire sprite.</param>
        public void doExplosionSprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// Move bus sprite.
        /// </summary>
        /// <param name="sprite">Bus sprite</param>
        public void doBusSprite(SimSprite sprite)
        {
        }

        /// <summary>
        /// Can one drive at the specified tile?
        /// </summary>
        /// <param name="x">X coordinate at map.</param>
        /// <param name="y">Y coordinate at map.</param>
        /// <returns>0 if not, 1 if you can, -1 otherwise</returns>
        public int canDriveOn(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Handle explosion of sprite (mostly due to collision?).
        /// </summary>
        /// <param name="sprite">sprite that should explode.</param>
        public void explodeSprite(SimSprite sprite)
        {
        }

        public bool checkWet(int x)
        {
            return false;
        }

        /// <summary>
        /// Destroy a map tile.
        /// </summary>
        /// <param name="ox">X coordinate in pixels.</param>
        /// <param name="oy">Y coordinate in pixels.</param>
        public void destroyMapTile(int ox, int oy)
        {
        }

        /// <summary>
        /// Start a fire in a zone.
        /// </summary>
        /// <param name="Xloc">X coordinate in map coordinate.</param>
        /// <param name="Yloc">Y coordinate in map coordinate.</param>
        /// <param name="ch">Map character at (Xloc, Yloc).</param>
        public void startFireInZone(int Xloc, int Yloc, int ch)
        {
        }

        /// <summary>
        /// Start a fire at a single tile.
        /// </summary>
        /// <param name="x">X coordinate in map coordinate.</param>
        /// <param name="y">Y coordinate in map coordinate.</param>
        public void startFire(int x, int y)
        {
        }

        /// <summary>
        /// Try to start a new train sprite at the given map tile.
        /// </summary>
        /// <param name="x">X coordinate in map coordinate.</param>
        /// <param name="y">Y coordinate in map coordinate.</param>
        public void generateTrain(int x, int y)
        {
        }

        /// <summary>
        /// Try to start a new bus sprite at the given map tile.
        /// </summary>
        /// <param name="x">X coordinate in map coordinate.</param>
        /// <param name="y">Y coordinate in map coordinate.</param>
        public void generateBus(int x, int y)
        {
        }

        /// <summary>
        /// Try to construct a new ship sprite
        /// </summary>
        public void generateShip()
        {
        }

        /// <summary>
        /// Start a new ship sprite at the given map tile.
        /// </summary>
        /// <param name="x">X coordinate in map coordinate.</param>
        /// <param name="y">Y coordinate in map coordinate.</param>
        public void makeShipHere(int x, int y)
        {
        }

        /// <summary>
        /// Start a new monster sprite.
        /// </summary>
        public void makeMonster()
        {
        }

        /// <summary>
        /// Start a new monster sprite at the given map tile.
        /// </summary>
        /// <param name="x">X coordinate in map coordinate.</param>
        /// <param name="y">Y coordinate in map coordinate.</param>
        public void makeMonsterAt(int x, int y)
        {
        }

        /// <summary>
        /// Ensure a helicopter sprite exists.
        /// 
        /// If it does not exist, create one at the given coordinates.
        /// </summary>
        /// <param name="pos">Start position in map coordinates.</param>
        public void generateCopter(Position pos)
        {
        }

        /// <summary>
        /// Ensure an airplane sprite exists.
        /// 
        /// If it does not exist, create one at the given coordinates.
        /// </summary>
        /// <param name="pos">Start position in map coordinates.</param>
        public void generatePlane(Position pos)
        {
        }

        /// <summary>
        /// Ensure a tornado sprite exists.
        /// </summary>
        public void makeTornado()
        {
        }

        /// <summary>
        /// Construct an explosion sprite.
        /// </summary>
        /// <param name="x">X coordinate of the explosion (in map coordinates).</param>
        /// <param name="y">Y coordinate of the explosion (in map coordinates).</param>
        public void makeExplosion(int x, int y)
        {
        }

        /// <summary>
        /// Construct an explosion sprite.
        /// </summary>
        /// <param name="x">X coordinate of the explosion (in pixels).</param>
        /// <param name="y">Y coordinate of the explosion (in pixels).</param>
        public void makeExplosionAt(int x, int y)
        {
        }
    }
}
