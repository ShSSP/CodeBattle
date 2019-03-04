using StarMarines.Models;
using StarMarines.Strategies;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StarMarines.Strategies
{
    class ForestPaths
    {
        public class NodePlanet
        {
            public Planet Planet;
            public NodePlanet Up; // куда будем отправлять ботов
            public List<NodePlanet> Downs = new List<NodePlanet>(); // от куда будем принимать ботов
            public int ReceivedDroids = 0; // боты которые заплпнировано отправить на эту планету

            public NodePlanet(Planet planet, NodePlanet up)
            {
                Planet = planet;
                Up = up;
            }
        }

        private List<Planet> OurPlanets;
        private Screen Screen; // используется для поиска соседей
        private List<NodePlanet> TargetNodes = new List<NodePlanet>();
        private List<NodePlanet> Leafs = new List<NodePlanet>();
        private HashSet<int> AreInForest = new HashSet<int>();

        public ForestPaths(Screen screen, List<Planet> orderedTargets, List<Planet> ourPlanets) //конструктор делает много работы
        {
            OurPlanets = ourPlanets;
            Screen = screen;

            foreach (var target in orderedTargets)
            {
                var nodePlanet = new NodePlanet(target, null); //null так как это конечная цель
                TargetNodes.Add(nodePlanet); //заполняем основание дерева
                Leafs.Add(nodePlanet); //на первой итерации корень(таргет) является листом
            }

            while (Leafs.Count > 0) // работает пока можно нарастить новый слой листьев
                Leafs = AddNewSheet();
        }

        private List<NodePlanet> AddNewSheet() //добавляет новый слой листьев в дерево
        {
            var newLeafs = new List<NodePlanet>(); //формерует новый список листов
            foreach (var leaf in Leafs) // проходимся по листьям
            {
                var neighborsByUs = leaf.Planet.GetNeighbors(Screen)    //поиск соседей //используется ПОЛЕ Screen (можно обойтись без поля и просто спускать)
                    .Where(x => OurPlanets.Contains(x))               //выбераем из них наши
                    .Where(p =>                                    //смотрим добавляли ли мы их в дерево
                    {       // здесь можно связать деревья -> можно отправлять часть сил в соседнее дерево
                        if (AreInForest.Contains(p.id))
                            return false; // если уже добавлено в дерево пропускаем

                        AreInForest.Add(p.id);
                        return true;
                    })
                    .ToList();

                foreach (var neighbor in neighborsByUs) // обход соседей листа
                {
                    var neighborNodePlanet = new NodePlanet(neighbor, leaf);
                    newLeafs.Add(neighborNodePlanet);   //отбираем только листья будущего дерева
                    leaf.Downs.Add(neighborNodePlanet); //строим дерево
                }
            }
            return newLeafs;
        }
        
        //SAVE
        public Command SaveCreateComand()
        {
            var command = new Command();
            var stackTraversing = new Stack<NodePlanet>(TargetNodes); //нужен для обхода дерева
            var stackCommand = new Stack<NodePlanet>();

            while (stackTraversing.Count > 0)   //обходим дерево
            {
                foreach (var target in stackTraversing.Pop().Downs) // спускаемся по дереву не возвращаясь к основанию
                {
                    stackTraversing.Push(target);   //добавляем с стек все ноды которые ниже текущего уровня 
                    stackCommand.Push(target);      //заполняем стек содержащий ноды наших планет
                }
            }

            while (stackCommand.Count > 0) 
            {
                var planetNodeFrom = stackCommand.Pop();
                var planetFrom = planetNodeFrom.Planet;
                var droids = System.Math.Min(CanReturnDroids(planetFrom)
                                             + planetNodeFrom.ReceivedDroids, planetFrom.droids);
                if (droids > 0)
                {
                    planetNodeFrom.Up.ReceivedDroids += droids;
                    command.actions.Add(new Action()
                    {
                        from = planetFrom.id,
                        to = planetNodeFrom.Up.Planet.id,
                        unitsCount = droids
                    });
                }
            }

            return command;
        }

        private int CanReturnDroids(Planet planet)
            => planet.droids - planet.GetOptimal(Screen);
    }
}