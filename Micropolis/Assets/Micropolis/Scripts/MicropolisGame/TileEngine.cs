using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MicropolisGame
{
    public class TileEngine
    {
        private Dictionary<int, Tile> _tiles = new Dictionary<int, Tile>();

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