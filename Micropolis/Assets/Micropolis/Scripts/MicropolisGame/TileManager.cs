using MicropolisCore;
using MicropolisEngine;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MicropolisGame
{
    /// <summary>
    /// Handles all the drawing and updates to the tile engine from the Micropolis map
    /// </summary>
    public class TileManager
    {
        /// <summary>
        /// Reference to the engine so we can fetch data from the simulator. Passed into the constructor.
        /// </summary>
        private readonly MicropolisUnityEngine _engine;

        /// <summary>
        /// The <see cref="Tilemap"/> object in the scene to draw on.
        /// </summary>
        private readonly Tilemap _mapLayer;

        /// <summary>
        /// The <see cref="TileEngine"/> object to create to fetch <see cref="Tile"/> objects from.
        /// </summary>
        private readonly TileEngine _tileEngine;

        /// <summary>
        /// Constructs the engine and hooks it up to the <see cref="Micropolis"/> engine.
        /// Also finds the <see cref="Tilemap"/> in the scene to do the drawing to.
        /// </summary>
        /// <param name="engine"></param>
        public TileManager(MicropolisUnityEngine engine)
        {
            _engine = engine;
            _tileEngine = new TileEngine();
            _mapLayer = GameObject.Find("MapLayer").GetComponent<Tilemap>();
        }

        public void Draw()
        {
            // TODO should we clear the layer on every pass?
            _mapLayer.ClearAllTiles();

            // TODO should only draw visible tiles not the entire map
            for (int x = 0; x < Micropolis.WORLD_W; x++)
            {
                for (int y = 0; y < Micropolis.WORLD_H; y++)
                {
                    var tile = _engine.map[x, y];
                    var tileId = tile & (ushort)MapTileBits.LOMASK;
                    
                    // TODO handle animated lightning bolt for tiles without power
                    var hasPower = tile & unchecked((ushort) MapTileBits.PWRBIT);

                    // TODO handle animated tiles like drawbridges, traffic, etc.
                    var isAnimated = tile & (ushort) MapTileBits.ANIMBIT;

                    // map is defined from top to bottom but Tilemap works from bottom to top so invert 
                    // the y value here and offset by 1 so we start at 0, -1 instead of 0, 0 in the grid
                    _mapLayer.SetTile(new Vector3Int(x, -y - 1, 0), _tileEngine.GetTile(tileId));
                }
            }
        }
    }
}