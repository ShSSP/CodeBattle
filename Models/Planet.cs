using StarMarines.Strategies;
using System.Collections.Generic;
using System.Linq;

namespace StarMarines.Models
{
    public class Planet
    {
        public int id;
        public int droids;
        public string owner;
        public string type;
        public int[] neighbours;

        public int GetOptimal(Screen screen)
        {
            if (screen.disasters != null)
                foreach (var dis in screen.disasters)
                    if (dis.type == "METEOR" && dis.planetId == id)
                        return 0;
                    
            return OptimumForMaxProfitOfType[type];
        }
        public int GetSize() => SizeOfType[type];
        public int GetProfitProcent() => ProfitProcentOfType[type];

        static private readonly Dictionary<string, int> ProfitProcentOfType =
            new Dictionary<string, int>() { { "TYPE_A", 110 }, { "TYPE_B", 115 }, { "TYPE_C", 120 }, { "TYPE_D", 130 } };
        static private readonly Dictionary<string, int> SizeOfType =
            new Dictionary<string, int>() { { "TYPE_A", 100 }, { "TYPE_B", 200 }, { "TYPE_C", 500 }, { "TYPE_D", 1000 } };
        static private readonly Dictionary<string, int> OptimumForMaxProfitOfType =
            new Dictionary<string, int>() { { "TYPE_A", 91 }, { "TYPE_B", 174 }, { "TYPE_C", 417 }, { "TYPE_D", 770 } };

        public List<Planet> GetNeighbors(Screen screen)
        {
            var neighbors = neighbours.ToList(); // находим соседей
            if (screen.portals != null)
                foreach (var portal in screen.portals) // добавляем порталы
                    if (id == portal.source)
                        neighbors.Add(portal.target);
            if (screen.disasters != null)
                foreach (var dis in screen.disasters)
                    if (dis.type == "BLACK_HOLE" && dis.sourcePlanetId == id) // исключаем черные дыры
                        neighbors.Remove(dis.planetId);

            return screen.planets
                .Where(x => neighbors.Contains(x.id))
                .ToList();
        }

        public int NeededDroids(Screen gamestate)
        {
            return System.Math.Min(GetOptimal(gamestate) - droids, 0);
        }
    }
}