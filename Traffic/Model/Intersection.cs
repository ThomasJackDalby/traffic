namespace Traffic.Model
{
    public class Intersection
    {
        public int ID { get; }
        public List<Street> InStreets { get; } = new List<Street>();
        public List<Street> OutStreets { get; } = new List<Street>();

        public Intersection(int id)
        {
            ID = id;
        }
    }
}
