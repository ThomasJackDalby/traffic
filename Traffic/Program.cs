using System;
using Traffic.Model;
using Traffic.Tools;

namespace Traffic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SimulationDefinition definition = SimulationDefinition.Load("Resources/a.txt");
            Street[][] schedules = CreateSchedule2(definition);

            Simulation simulation = new Simulation(definition, schedules);

            int score = simulation.Run();

            Console.WriteLine(score);
            Console.ReadLine();
        }

        public static Street[][] CreateBasicSchedule(SimulationDefinition definition)
        {
            return definition.Intersections.Select(i => i.InStreets.ToArray()).ToArray();
        }

        public static Street[][] CreateSchedule2(SimulationDefinition definition)
        {
            Dictionary<Intersection, Street[]> schedule = definition.Intersections.ToDictionary(i => i, i => new Street[0]);

            Dictionary<Street, int>[] cars = new Dictionary<Street, int>[definition.NumberOfIntersections];
            foreach (Intersection intersection in definition.Intersections) cars[intersection.ID] = intersection.InStreets.ToDictionary(s => s, s => 0);

            foreach (Car car in definition.Cars)
            {
                foreach(Street street in car.Route)
                {
                    cars[street.End.ID][street] += 1;
                }
            }
            foreach(Intersection intersection in definition.Intersections)
            {
                Dictionary<Street, int> traffic = cars[intersection.ID];
                Console.WriteLine($"{intersection.ID} | {String.Join(" ", traffic.Select(kvp => $"{kvp.Key.Name}:{kvp.Value}"))}");

                if (traffic.Count(street => street.Value > 0) == 1)
                {
                    if (traffic.Count == 1) schedule[intersection] = new Street[] { traffic.First().Key };
                }
                else
                {
                    schedule[intersection] = intersection.InStreets.OrderByDescending(i => i.ID).ToArray();
                }
            }

            // for intersections with only one street, or where only one street has cars, set to green



            return schedule.OrderBy(kvp => kvp.Key.ID).Select(kvp => kvp.Value).ToArray();
        }
    }
}