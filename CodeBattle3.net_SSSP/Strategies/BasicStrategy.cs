using System;
using System.Threading.Tasks;
using StarMarines.Models;
using Newtonsoft.Json;
using System.Linq;

namespace StarMarines.Strategies
{
    //Первая конкретная реализация-стратегия 
    public class BasicStrategy : AbstratctStrategy
    {
        public override Command OnReceived(Screen gameState)
        {
            var rand = new Random();
            Command command = new Command(); // формируем команду
            if (gameState.planets.Count() > 0) {  // проверяем наличие планет в ответе
                var myPlanets = gameState.planets.Where(x=> { // выираем только свои планеты
                    if(x.owner != null){
                        return x.owner.Equals(BotName,StringComparison.InvariantCultureIgnoreCase);
                    }
                    return false;                    
                }).ToList(); // берем первую
                if (myPlanets.Count() > 0){ // проверяем а остались ли свои планеты?
                    myPlanets.ForEach(p => {
                        var neighbours = p.neighbours.ToList();  // находим соседей
                        var droid = p.droids / neighbours.Count(); // делим на всех соседей поровну
                        neighbours.ForEach(x=>{
                            command.actions.Add(
                                new Models.Action{ // формируем действие. Можно сформироват ьпо одному действию с каждой вашей планеты.
                                    from = p.id,
                                    to = x,
                                    unitsCount = droid //rand.Next(1, p.droids)
                            });
                        });
                    });
                } else {
                    Console.WriteLine("У вас больше нет планет.");
                }
            }
            if (gameState.errors.Count() > 0) // а есть ли ошибки?
            {
                Console.WriteLine(JsonConvert.SerializeObject(gameState.errors)); // увы есть.. покажем их.
            }
            return command;
        }
    }
}