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
            var neighbors = planet.neighbours.ToList(); // ������� �������
            if (portals != null)
                foreach (var portal in portals) // ��������� �������
                    if (planet.id == portal.source)
                        neighbors.Add(portal.target);
            if (disasters != null)
                foreach (var dis in disasters)
                {
                    if (dis.type == "BLACK_HOLE" && dis.sourcePlanetId == planet.id) // ��������� ������ ����
                        neighbors.Remove(dis.planetId);
                    if (dis.type == "METEOR") // ��������� ������� � �����������
                        neighbors.Remove(dis.planetId);
                }

            return planets
                .Where(x => neighbors.Contains(x.id))
                .ToList();
        }
    }
}