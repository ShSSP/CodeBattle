using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarMarines.Models;
using Action = StarMarines.Models.Action;

namespace StarMarines.Strategies
{
    /// <summary>
    /// Базовый класс для всех стратегий
    /// </summary>
    public abstract class AbstratctStrategy : IStrategy
    {
        private string _botName;

        public string BotName { get => _botName ; set => _botName = value; }

        public virtual Command OnReceived(Screen gameState)
        {
            return new Command();
        }

        public void Statistics()
        {
            Console.WriteLine($"Bot {BotName} alive");
        }

        public void Some(Screen gameState)
        {

        }

        public List<Planet> GetPriorityPlanets(Screen gameState)
        {
            var result = new List<Planet>();
            var allAvailible = GetOurPlanets(gameState)
                .Where(x=>x.droids<x.GetOptimal(gameState))
                .ToList();
            allAvailible.AddRange(GetZoneNeighbours(gameState)
                .Where(x => x.droids < x.GetOptimal(gameState))
                .ToList());
            allAvailible
                .OrderByDescending(x => x.GetOptimal(gameState))
                .ThenBy(x =>
            {
                if (x.owner == _botName) return int.MaxValue;
                else return x.id;
            }).ToList();
            int j = 0;
            var enemyCount = allAvailible.Where(x => x.owner != _botName && x.owner != null).Count();
            for (int i = 0; i < allAvailible.Count; i++)
            {
                Planet planet = allAvailible[i];
                if (planet.owner != _botName && planet.owner != null)
                {
                    allAvailible.RemoveAt(i);
                    allAvailible.Add(planet);
                    i--;
                    j++;
                    if (j == enemyCount - 1) break;//отлов зацикленности
                }

            }
            return result;
        }

        public List<Planet> GetZoneNeighbours(Screen gameState)
        {
            var result = new List<Planet>();
            foreach (var planet in GetOurPlanets(gameState))
            {
                foreach (var neighbour in planet.neighbours)
                {
                    var currentPlanet = new Planet();
                    foreach (var gameStatePlanet in gameState.planets)
                    {
                        if (gameStatePlanet.id == neighbour)
                            currentPlanet = gameStatePlanet;
                        break;
                    }

                    if (currentPlanet.owner != _botName && !result.Contains(currentPlanet))
                        result.Add(currentPlanet);
                }
            }
            return result;
        }

        public List<Planet> GetOurPlanets(Screen gameState) =>
            gameState.planets.Where(x => x.owner == _botName).ToList();

    }
}