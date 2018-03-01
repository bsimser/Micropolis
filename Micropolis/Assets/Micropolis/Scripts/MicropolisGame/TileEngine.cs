using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MicropolisGame
{
    /// <summary>
    /// Keeps a cached copy of rendered Tiles and provides them for drawing onto a TileMap
    /// TODO load all tiles at startup (there's only 960 of them)
    /// TODO load all Tilesets at startup
    /// </summary>
    public class TileEngine
    {
        /// <summary>
        /// Cached copy of all rendered Tiles
        /// </summary>
        private readonly Dictionary<int, Tile> _tiles = new Dictionary<int, Tile>();

        /// <summary>
        /// Fetches a <see cref="Tile"/> object from a cache or adds it 
        /// if it's being requested for the first time.
        /// </summary>
        /// <param name="tileId">Tile id to render from the MicroplisTileCharacter enum</param>
        /// <returns>A <see cref="Tile"/> object that can be renedered onto a <see cref="Tilemap"/></returns>
        public Tile GetTile(int tileId)
        {
            if (_tiles.ContainsKey(tileId))
            {
                return _tiles[tileId];
            }

            var path = string.Format("Tiles/tiles_{0}", tileId);
            var asset = Resources.Load<Tile>(path);

            // dynamic load the sprite for the tile based on our theme
            //var theme = "x11";
            //var theme = "classic";
            var theme = "classic95";
            //var theme = "wildwest";
            //var theme = "mooncolony";
            //var theme = "medievaltimes";
            //var theme = "futureusa";
            //var theme = "futureeurope";
            //var theme = "ancientasia";

            // TODO we don't have to load all the sprites every time
            Sprite[] sprites = Resources.LoadAll<Sprite>("Tilesets/" + theme + "/tiles");
            asset.sprite = sprites[tileId];

            _tiles.Add(tileId, asset);
            return _tiles[tileId];
        }
    }
}