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
            _tiles.Add(tileId, asset);
            return _tiles[tileId];
        }
    }
}