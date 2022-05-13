namespace BusinessLogic.models
{
    public class Weather
    {
        public Main Main { get; set; }
        public Coordinate Coord { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }
    public class Coordinate
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }
    public class Main
    {
        public double Temp { get; set; }
    }
}
