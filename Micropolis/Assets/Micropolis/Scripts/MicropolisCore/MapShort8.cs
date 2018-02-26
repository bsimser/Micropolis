namespace MicropolisCore
{
    public class MapShort8 : Map<short>
    {
        public MapShort8(short defaultValue) : base(defaultValue, 8)
        {
        }

        public MapShort8(MapShort8 map) : base(map)
        {
        }
    }
}