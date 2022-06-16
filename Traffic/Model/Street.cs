namespace Traffic.Model
{
    public class Street
    {
        public int ID { get; }
        public string Name { get; }
        public int Length { get; }
        public Intersection Start { get; set; }
        public Intersection End { get; set; }

        public Street(int id, string name, int length)
        {
            ID = id;
            Name = name;
            Length = length;
        }
    }
}
