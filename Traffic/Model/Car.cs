namespace Traffic.Model
{
    public class Car
    {
        public int ID { get; }
        public Street[] Route { get; set; } = new Street[0];

        public Car(int id)
        {
            ID = id;
        }
    }
}
