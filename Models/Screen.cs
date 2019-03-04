using System.Collections.Generic;
using System.Linq;

namespace StarMarines.Models
{
    public class Screen {
        public Planet[] planets;
        public Disaster[] disasters;
        public Portal[] portals;
        public string[] errors;
        
        public List<Planet> GetNeighbors(Planet planet)
        {
            var neighbors = planet.neighbours.ToList(); // находим соседей
            if (portals != null)
                foreach (var portal in portals) // добавл€ем порталы
                    if (planet.id == portal.source)
                        neighbors.Add(portal.target);
            if (disasters != null)
                foreach (var dis in disasters)
                {
                    if (dis.type == "BLACK_HOLE" && dis.sourcePlanetId == planet.id) // исключаем черные дыры
                        neighbors.Remove(dis.planetId);
                    if (dis.type == "METEOR") // исключаем планеты с катоклизмом
                        neighbors.Remove(dis.planetId);
                }

            return planets
                .Where(x => neighbors.Contains(x.id))
                .ToList();
        }
    }
}