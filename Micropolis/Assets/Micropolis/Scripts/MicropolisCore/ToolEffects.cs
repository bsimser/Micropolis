using System;
using System.Collections.Generic;

namespace MicropolisCore
{
    public class ToolEffects
    {
        private Micropolis sim; // Simulator to get map values from, and to apply changes.
        private int cost; // Accumlated costs 
        private Dictionary<Position, ushort> modifications = new Dictionary<Position, ushort>(); // Collected world modifications

        public ToolEffects(Micropolis mpolis)
        {
            sim = mpolis;
            clear();
        }

        private void clear()
        {
            cost = 0;
            modifications.Clear();
            
            // TODO free all frontend messages
        }

        public void modifyWorld()
        {
            sim.spend(cost); // Spend the costs
            sim.updateFunds();

            // Modify the world
            foreach (var entry in modifications)
            {
                sim.map[entry.Key.posX, entry.Key.posY] = (ushort) entry.Value;
            }

            // And finally send the messages
            // TODO

            clear();
        }

        /// <summary>
        /// Get the value of a map position
        /// </summary>
        /// <param name="x">Horizontal coordinate of position being queried.</param>
        /// <param name="y">Vertical coordinate of position being queried.</param>
        /// <returns>Map value at the specified position.</returns>
        public ushort getMapValue(int x, int y)
        {
            return getMapValue(new Position(x, y));
        }

        /// <summary>
        /// Get a map value from the world.
        /// Unlike the simulator world, this method takes modifcations made
        /// previously by (other) tools into account.
        /// </summary>
        /// <param name="position">Position of queried map value. Position must be on-map.</param>
        /// <returns>Map value of the queried position.</returns>
        private ushort getMapValue(Position pos)
        {
            if (modifications.ContainsKey(pos))
            {
                return modifications[pos];
            }
            return (ushort) sim.map[pos.posX, pos.posY];
        }

        /// <summary>
        /// Set a new map value.
        /// </summary>
        /// <param name="x">Horizontal coordinate of position to set.</param>
        /// <param name="y">Vertical coordinate of position to set.</param>
        /// <param name="mapVal">Value to set.</param>
        public void setMapValue(int x, int y, ushort mapVal)
        {
            setMapValue(new Position(x, y), mapVal);
        }

        /// <summary>
        /// Set a new map value.
        /// </summary>
        /// <param name="pos">Position to set.</param>
        /// <param name="mapVal">Value to set.</param>
        private void setMapValue(Position pos, ushort mapVal)
        {
            modifications[pos] = mapVal;
        }
    }
}