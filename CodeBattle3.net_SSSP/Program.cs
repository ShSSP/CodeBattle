using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StarMarines;
using StarMarines.Strategies;

public class Program
{
    private static Context _context;
    private static IStrategy _strategy;

    public static void Main(string[] args)
    {
        //Test.Do();
        // конфигурируем бота
        while (true)
        {
            var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.bot.json", optional: true, reloadOnChange: true); //добавте данную конфигурацию для своего бота, скопировав appsettings.json и настроив его
            var config = configBuilder.Build();

            /// указываем название бота, игровой сервер и секретный токен с сайта
            //_context = new Context(config["botname"], config["server"], config["token"], Boolean.Parse(config["debug"]));
            _context = new Context(config["SSSP"], config["192.168.1.63:8081"], config["9477bc6b-93d0-44a0-83c1-65c7c8efdd80"], Boolean.Parse(config["debug"]));
            _strategy = new AgroStartegy();
            //_strategy = new BasicStrategy();  //создается стратегия игры для вашего бота . Вы можете создать свою стратегию на базе базовой 
            _context.SetStrategy(_strategy);
            StartConnectionAsync().Wait();
            StopConnectionAsync();
        }
    }

    public static async Task StartConnectionAsync()
    {
        await _context.StartConnectionAsync(async (message) => {
            var command = _strategy.OnReceived(message); // передаем сообщение с сервера в стратегию
            await _context.SendAsync(command); // посылаем сформированный ответ на сервер
        });
    }

    public static async Task StopConnectionAsync()
    {
        await _context.StopConnectionAsync();
    }
}
