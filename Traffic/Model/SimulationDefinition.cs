using Traffic.Tools;

namespace Traffic.Model
{
    public class SimulationDefinition
    {
        public int Duration { get; set; }
        public int NumberOfCars { get; set; }
        public int NumberOfStreets { get; set; }
        public int NumberOfIntersections { get; set; }
        public int BonusPoints { get; set; }

        public Car[] Cars { get; set; } = new Car[0];
        public Street[] Streets { get; set; } = new Street[0];
        public Intersection[] Intersections { get; set; } = new Intersection[0];


        public static SimulationDefinition Load(string filePath)
        {
            using Stream stream = File.OpenRead(filePath);
            using FileReader reader = new FileReader(stream);

            SimulationDefinition definition = new SimulationDefinition();
            definition.Duration = reader.Read<int>();
            definition.NumberOfIntersections = reader.Read<int>();
            definition.NumberOfStreets = reader.Read<int>();
            definition.NumberOfCars = reader.Read<int>();
            definition.BonusPoints = reader.Read<int>();

            definition.Streets = new Street[definition.NumberOfStreets];
            definition.Cars = new Car[definition.NumberOfCars];
            definition.Intersections = Enumerable.Range(0, definition.NumberOfIntersections).Select(i => new Intersection(i)).ToArray();


            Dictionary<string, Street> nameToStreetMap = new Dictionary<string, Street>();
            for (int streetID = 0; streetID < definition.NumberOfStreets; streetID++)
            {
                int startID = reader.Read<int>();
                int endID = reader.Read<int>();
                string streetName = reader.Read<string>();
                int length = reader.Read<int>();

                Intersection start = definition.Intersections[startID];
                Intersection end = definition.Intersections[endID];

                Street street = new Street(streetID, streetName, length)
                {
                    Start = start,
                    End = end
                };
                definition.Streets[streetID] = street;
                start.OutStreets.Add(street);
                end.InStreets.Add(street);
                nameToStreetMap[streetName] = street;
            }

            for (int carID = 0; carID < definition.NumberOfCars; carID++)
            {
                int numberOfStreets = reader.Read<int>();
                Street[] route = new Street[numberOfStreets];
                for (int i = 0; i < numberOfStreets; i++)
                {
                    string streetName = reader.Read<string>();
                    Street street = nameToStreetMap[streetName];
                    route[i] = street;
                }
                definition.Cars[carID] = new Car(carID) { Route = route };
            }

            return definition;
        }
    }
}
