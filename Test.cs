using StarMarines.Models;
using StarMarines.Strategies;

namespace StarMarines
{
    public class Test
    {
        static private Screen gameState = new Screen()
        {
            planets = new Planet[]
            {
            new Planet() {id = 0, droids = 482, neighbours = new[] {1, 2}, owner = "SSSP", type = "TYPE_C"},
            new Planet() {id = 1, droids = 0, neighbours = new[] {0, 2,4,9}, owner = null, type = "TYPE_B"},
            new Planet() {id = 2, droids = 0, neighbours = new[] {0, 1,7,9}, owner = null, type = "TYPE_B"},

            new Planet() {id = 3, droids = 482, neighbours = new[] {4, 5}, owner = "bot1", type = "TYPE_C"},
            new Planet() {id = 4, droids = 0, neighbours = new[] {3, 5,1,9}, owner = null, type = "TYPE_B"},
            new Planet() {id = 5, droids = 0, neighbours = new[] {3, 4,8,9}, owner = null, type = "TYPE_B"},

            new Planet() {id = 6, droids = 482, neighbours = new[] {7, 8}, owner = "bot2", type = "TYPE_C"},
            new Planet() {id = 7, droids = 0, neighbours = new[] {6, 8,2,9}, owner = null, type = "TYPE_B"},
            new Planet() {id = 8, droids = 0, neighbours = new[] {6, 7,8,9}, owner = null, type = "TYPE_B"},

            new Planet() {id = 9, droids = 0, neighbours = new[] {1, 2, 4, 5, 7, 8}, owner = "b", type = "TYPE_A"}
            },
            disasters = new Disaster[] { new Disaster { planetId = 0, type = "METEOR"} }            
        };
        
        //class Stats
        //{
        //    public string Name;
        //    public int Droids;
        //    public double Profit;

        //    public Stats(string name, int droids, double profit) { Name = name; Droids = droids; Profit = profit; }
        //}

        public static void Do()
        {
            var stranegy = new AgroStartegy();
            stranegy.BotName = "SSSP";
            var stranegy1 = new AgroStartegy();
            stranegy1.BotName = "bot1";
            var stranegy2 = new AgroStartegy();
            stranegy2.BotName = "bot2";

            var command = stranegy.OnReceived(gameState);
            var command1 = stranegy1.OnReceived(gameState);
            var command2 = stranegy2.OnReceived(gameState);
        }
    }
}
