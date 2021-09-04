using System;
using System.Collections.Generic;
using System.Linq;


namespace CityManagement
{

    public readonly struct City
    {
        public int Id { get; init; }
        public List<int> Connections { get; init; }
    }

    public class CityManager
    {
        private readonly int _numberOfCities;

        // Dictionary to save the City and it's connections.
        public Dictionary<int, City> Cities { get; init; }

        public CityManager(int numberOfCities)
        {
            _numberOfCities = numberOfCities;
            Cities ??= new Dictionary<int, City>();

            BuildCities();
        }

        private void BuildCities()
        {
            Random random = new();

            // For total number of cities, create a new city and form the connections
            // to previously built cities. 
            for (int i = 0; i < _numberOfCities; i++)
            {
                City newCity = new() { Id = i, Connections = new List<int>() };

                if (Cities.Count > 0)
                {
                    City lastAddedCity = Cities[Cities.Keys.Last()];
                    lastAddedCity.Connections.Add(newCity.Id);
                    newCity.Connections.Add(lastAddedCity.Id);

                    if (Cities.Count > 1)
                    {
                        City randomCity = Cities[random.Next(0, Cities.Keys.Last() - 1)];
                        newCity.Connections.Add(randomCity.Id);
                        randomCity.Connections.Add(newCity.Id);
                    }
                }

                Cities[i] = newCity;            
            }
                
        }

        public void PrintAllConnections()
        {
            foreach (var item in Cities)
            {
                Console.Write($"City {item.Key} Connections : [");
                this.PrintCityConnection(item.Value.Connections);
            }

            Console.WriteLine();
        }

        public void PrintCityConnection(List<int> path)
        {
            int lastCityId = path.Last();

            path.ForEach(delegate (int cityId) {

                Console.Write(cityId);

                if (lastCityId != cityId)
                    Console.Write(",");
            });

            Console.WriteLine($"]");
        }

        public void PrintShortestPath(int source, int destination)
        {
            if (source.Equals(destination))
            {
                Console.WriteLine("\nSource and Destination Id are same!");
                return;
            }

            if (!Cities.ContainsKey(source) || !Cities.ContainsKey(destination))
            {
                Console.WriteLine($"\nCity number {source} or {destination} doesn't exist!");
                return;
            }

            // Save city id's to visit their connections. 
            Queue<int> cityIterator = new Queue<int>();

            // Keep track of the parentId's of visited cities to print the connection.
            Dictionary<int, int> parentTrack = new Dictionary<int, int>();
           
            cityIterator.Enqueue(Cities[source].Id);

            bool[] isTraversed = new bool[Cities.Count];
            isTraversed[source] = true;

            // Keep traversing non- visited cities.
            while(cityIterator.Any())
            {
                int cityId = cityIterator.Dequeue();

                foreach (var item in Cities[cityId].Connections)
                {
                    // Found the destination Id in the connections. 
                    if (item.Equals(destination))
                    {    
                        int parentId = cityId;

                        // List to create the path by backtracking parent city Id's
                        List<int> path = new List<int>();
                        path.Add(item);

                        //Backtrack and add elements to list
                        while (source != parentId)
                        {
                            path.Add(parentId);
                            parentId = parentTrack[parentId];
                        }

                        path.Add(source);
                        path.Reverse();

                        Console.Write($"Path from City {path.First()} to City {path.Last()}: [");

                        this.PrintCityConnection(path);

                        return;
                    }


                    // If current cityId doesn't match destinationId and it's not been visited before, 
                    // then add it to the queue and save parentId.
                    if (!isTraversed[item])
                    {
                        isTraversed[item] = true;
                        parentTrack[item] = cityId;

                        cityIterator.Enqueue(item);
                    }
                }
            }

        }
    }

    class CityExpansionSimulation
    {
        static void Main(string[] args)
        {
            // Number of cities to simulate.
            const int numberOfCities = 20;

            // Source and destination city number to calculate shortest path. 
            const int sourceCityId = 0;
            const int destinatonCityId = 1;

            // Takes number of cities and simulates the cities expansion.
            CityManager Manager = new(numberOfCities);

            Manager.PrintAllConnections();

            Manager.PrintShortestPath(sourceCityId, destinatonCityId);

        }
    }
}
