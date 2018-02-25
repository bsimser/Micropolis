using MicropolisCore;
using MicropolisEngine;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MicropolisGame
{
    /// <summary>
    /// Handles all the drawing and updates to the tile engine
    /// </summary>
    public class TileManager
    {
        private readonly MicropolisUnityEngine _engine;
        private readonly Tilemap _mapLayer;
        private readonly TileEngine _tileEngine;

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

            // TODO should only setup/update visible tiles
            for (int x = 0; x < Micropolis.WORLD_W; x++)
            {
                for (int y = 0; y < Micropolis.WORLD_H; y++)
                {
                    var tile = _engine.map[x, y];
                    var tileId = tile & (ushort)MapTileBits.LOMASK;
                    var hasPower = tile & unchecked((ushort) MapTileBits.PWRBIT);
                    var isAnimated = tile & (ushort) MapTileBits.ANIMBIT;

                    // map is defined from top to bottom but Tilemap works from bottom to top so invert 
                    // the y value here and offset by 1 so we start at 0, -1 instead of 0, 0 in the grid
                    _mapLayer.SetTile(new Vector3Int(x, -y - 1, 0), _tileEngine.GetTile(tileId));

                    // TODO handle animated lightning bolt for tiles without power
                    // TODO handle animated tiles like drawbridges, traffic, etc.
                }
            }
        }
    }
}