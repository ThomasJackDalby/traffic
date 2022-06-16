namespace Traffic.Model
{
    public class Simulation
    {
        private readonly SimulationDefinition definition;

        private readonly Street?[] intersections;
        private readonly Street[][] schedules;
        private readonly Queue<int>[] streetQueues;
        private readonly Queue<(int Car, int ArrivalTime)>[] streetDrivings;
        private readonly int[] carPositions;

        public Simulation(SimulationDefinition definition, Street[][] schedules)
        {
            this.definition = definition;
            this.schedules = schedules;

            intersections = new Street[this.definition.NumberOfIntersections];
            carPositions = new int[this.definition.NumberOfCars];
            streetQueues = Enumerable.Range(0, this.definition.NumberOfStreets).Select(i => new Queue<int>()).ToArray();
            streetDrivings = Enumerable.Range(0, this.definition.NumberOfStreets).Select(i => new Queue<(int, int)>()).ToArray();

            foreach (Car car in definition.Cars) streetQueues[car.Route[0].ID].Enqueue(car.ID);
        }

        public int Run()
        {
            Console.WriteLine($"Duration: {definition.Duration}");
            Console.WriteLine($"Bonus: {definition.BonusPoints}");
            foreach (Car car in definition.Cars) Console.WriteLine($"[Car {car.ID}] {String.Join(" ", car.Route.Select(street => $"{street.Name} {street.End.ID}"))}");
            Console.WriteLine(new string('-', 10));
            int score = 0;
            int t = 0;
            int cars = definition.NumberOfCars;
            while (cars > 0)
            {
                // make all cars arrive at the end of street if there
                for (int i = 0; i < streetDrivings.Length; i++)
                {
                    if (streetDrivings[i].Count > 0)
                    {
                        (int carID, int arrivalTime) = streetDrivings[i].Peek();
                        if (t >= arrivalTime)
                        {
                            streetDrivings[i].Dequeue();

                            // if this is the last street in the cars journey, it is removed
                            int carPosition = carPositions[carID];
                            if (carPosition >= definition.Cars[carID].Route.Length - 1)
                            {
                                int carScore = t < definition.Duration ? 1000 + (definition.Duration - t) : 0;
                                score += carScore;
                                Console.WriteLine($"T{t}: [Car {carID}] has finished it's journey ({carScore}).");
                                cars--;
                            }
                            else
                            {
                                streetQueues[i].Enqueue(carID);
                            }
                        }
                    }
                }

                for (int intersection = 0; intersection < intersections.Length; intersection++)
                {
                    // change the light according to the schedule
                    Street[] schedule = schedules[intersection];
                    Street? activeStreet = schedule.Length > 0 ? schedule[t % schedule.Length] : null;
                    intersections[intersection] = activeStreet;

                    // if their are cars at the active street, move one to the next street on its route
                    if (activeStreet is not null)
                    {
                        Queue<int> streetQueue = streetQueues[activeStreet.ID];
                        if (streetQueue.Count > 0)
                        {
                            int carID = streetQueue.Dequeue();
                            Street[] route = definition.Cars[carID].Route;
                            Street nextStreet = route[++carPositions[carID]];
                            Queue<(int, int)> nextDriving = streetDrivings[nextStreet.ID];
                            int arrivalTime = t + nextStreet.Length;
                            nextDriving.Enqueue((carID, arrivalTime));
                            Console.WriteLine($"T{t}: [Car {carID}] started {nextStreet.Name} (will arrive @ T{arrivalTime})");
                        }
                    }

                    foreach ((Street street, Queue<int> queue) in definition.Intersections[intersection].InStreets.Select(street => (street, streetQueues[street.ID])))
                    {
                        foreach (int carID in queue) Console.WriteLine($"T{t}: [Car {carID}] is queued at a red light on {street.Name}.");
                    }
                }

                // increment the time
                t++;
            }

            return score;
        }
    }
}
