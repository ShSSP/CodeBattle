using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarMarines.Models;
using StarMarines.Strategies;

namespace StarMarines.Strategies
{
    public class AgroStartegy : AbstratctStrategy
    {
        private Command agroCommand;
        private HashSet<int> usedPlanet;

        public override Command OnReceived(Screen gameState)
        {
            agroCommand = new Command();
            usedPlanet = new HashSet<int>();

            var result = new Command();
            var usedForAttackPlanet = AgroCreateComand(gameState);

            var targets = GetPriorityPlanets(gameState);
            var ourPlanets = GetOurPlanets(gameState);

            var forestPaths = new ForestPaths(
                gameState,
                targets,
                GetOurPlanets(gameState));

            var saveComands = forestPaths.SaveCreateComand();

            result.actions = agroCommand.actions
                .Concat(saveComands.actions)
                .ToList();

            return result;
        }

        //AGRO
        public HashSet<int> AgroCreateComand(Screen gameState) //Return used planet.Id
        {
            var attackedPlanets = new Stack<Planet>(GetZoneNeighbours(gameState) //планеты в зоне досягаемости
                .OrderByDescending(p => p.GetOptimal(gameState))
                .ThenBy(p => GetDenger(p, gameState)));

            //var our = GetOurPlanets(gameState)
            //    .Where(p => p.droids < p.GetOptimal(gameState));
            //foreach (var planet in our)
            //    attackedPlanets.Push(planet);

            while (attackedPlanets.Count > 0)
            {
                var attackedPlanet = attackedPlanets.Pop();
                var ourAttackersPlanets = GetOurAtackersPlanets(attackedPlanet, gameState); //наши планеты у которых тип хуже

                if (GetDenger(attackedPlanet, gameState) < ourAttackersPlanets.Sum(p => p.droids)) //если сумма наших атакующих дроидов больше чем всех врагов рядом с планетой
                {
                    foreach (var attackers in ourAttackersPlanets)
                    {
                        if (!usedPlanet.Contains(attackers.id))
                            agroCommand.actions.Add(new Action()
                            {
                                from = attackers.id,
                                to = attackedPlanet.id,
                                unitsCount = attackers.droids 
                            });
                        usedPlanet.Add(attackers.id);
                        attackedPlanets.Push(attackers);
                    }
                }
            }
            return usedPlanet;
        }

        public int GetDenger(Planet planet, Screen gameState)
        {
            var planetsInThisArea = planet.GetNeighbors(gameState);
            planetsInThisArea.Add(planet);

            return planetsInThisArea
                .Where(p => p.owner != null && p.owner != BotName)
                .Sum(p => p.droids /** p.GetProfitProcent() / 100*/); // GetProfitProcent в %
        }

        public List<Planet> GetOurAtackersPlanets(Planet attackedPlanet, Screen gameState) //наши планеты у которых тип хуже
        {
            return attackedPlanet.GetNeighbors(gameState)
                .Where(p => p.owner == BotName)
                .Where(p => p.GetOptimal(gameState) < attackedPlanet.GetOptimal(gameState))
                .ToList();
        }
        
        public new List<Planet> GetPriorityPlanets(Screen gameState)
        {
            var result = GetOurPlanets(gameState) //our planet
                .Where(x => x.droids < x.GetOptimal(gameState))
                .OrderByDescending(x => x.GetOptimal(gameState))
                .ThenBy(p => GetDenger(p, gameState))
                .ToList();
            
            //foreach (var planet in gameState.planets)
            //    if (usedPlanet.Contains(planet.id))
            //        ourPlanets.Add(planet);

            var neighborZoneNoEnemy = GetZoneNeighbours(gameState)
                .Where(x => x.droids < x.GetOptimal(gameState))
                .Where(x => x.owner == null)
                .OrderByDescending(x => x.GetOptimal(gameState))
                .ThenBy(p => GetDenger(p, gameState))
                .ToList(); 

            var neighborZoneEnemy = GetZoneNeighbours(gameState)
                .Where(x => x.droids < x.GetOptimal(gameState))
                .Where(x => x.owner != null)
                .OrderByDescending(x => x.GetOptimal(gameState))
                .ThenBy(p => GetDenger(p, gameState))
                .ToList();

            result.AddRange(neighborZoneNoEnemy);
            result.AddRange(neighborZoneEnemy);
            
            return result;
        }

        public new List<Planet> GetZoneNeighbours(Screen gameState)
        {
            var result = new List<Planet>();
            foreach (var planet in GetOurPlanets(gameState))
            {
                foreach (var neighbour in planet.GetNeighbors(gameState))
                    if (neighbour.owner != BotName && !result.Contains(neighbour))
                        result.Add(neighbour);
            }
            return result;
        }

        public new List<Planet> GetOurPlanets(Screen gameState)
        {
            return gameState.planets.Where(x =>
            {
                if (x.owner != null && !usedPlanet.Contains(x.id)) // выираем только свои НЕ ПОЮЗАНЫЕ планеты
                    return x.owner.Equals(BotName, System.StringComparison.InvariantCultureIgnoreCase);
                return false;
            }).ToList();
        }
    }
}
