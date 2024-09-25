

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameIdeaCreator
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder();
            //builder.SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            

            IConfiguration configuration = builder.Build();

            var batchSize = configuration["AutoNumberOptions:BatchSize"];

            Console.WriteLine($"Batch Size {batchSize}");


            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            var caller = new Caller();
            var handler =  new Handler();
            var mainURL = "https://api.steampowered.com/ISteamApps/GetAppList/v0002/?key=STEAMKEY&format=json";
            var gameURL = "https://store.steampowered.com/api/appdetails?appids=";
            var responseBody = await caller.GetContent(mainURL);
            handler.SetGameInfo(responseBody);
            var counter = 0;
            foreach (var game in handler.Games)
            {
                var gameContent = await caller.GetContent($"{gameURL}{game.Id}");
                handler.SetGameDescription(gameContent,game);
                counter++;
            }
            Console.WriteLine(counter);
            AddDataToDB(handler);
        }

        private static void AddDataToDB(Handler data)
        {
            using var db = new GameContext();
            {
                var tableRows = db.Games.Select(x => x.Id).ToList();
                foreach (var game in data.Games)
                {
                    if (tableRows.Contains(game.Id)) continue;
                    db.Add(game);
                }
                db.SaveChanges();
                var dbgame = db.Games
                    .OrderBy(x => x.Id)
                    .First();

                Console.WriteLine(dbgame.Name);
            }
        }
    }
}
