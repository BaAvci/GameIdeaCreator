

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameIdeaCreator
{
    public class Program
    {
        static void Main(string[] args)
        {


            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            var caller = new Caller();
            var handler = new Handler();
            var mainURL = "https://api.steampowered.com/ISteamApps/GetAppList/v0002/?key=STEAMKEY&format=json";
            var gameURL = "https://store.steampowered.com/api/appdetails?appids=";
            var responseBody = await caller.GetContent(mainURL);
            handler.SetGameInfo(responseBody);
            var counter = 0;
            foreach (var game in handler.Games)
            {
                var gameContent = await caller.GetContent($"{gameURL}{game.Id}");
                handler.SetGameDescription(gameContent, game);
                counter++;
                if (counter == 1)
                {
                    break;
                }
            }
            AddDataToDB(handler);
        }

        private static void AddDataToDB(Handler data)
        {
            using (var db = new GameContext())
            {
                var distinctGame = data.Games.GroupBy(g => g.Id)
                    .Select(grp => grp.First())
                    .Where(g => db.Game.Find(g.Id) == null)
                    .ToList();
                foreach (var game in distinctGame)
                {
                    db.Add(game);
                }
                db.SaveChanges();
                var dbgame = db.Game
                    .OrderBy(x => x.Id)
                    .First();

                Console.WriteLine(dbgame.Name);
            }
        }
    }
}
