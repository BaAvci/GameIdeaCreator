using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace GameIdeaCreator
{
    public class Handler
    {
        public List<GameInfo> Games { get; }
        public Handler()
        {
            Games = new List<GameInfo>();
        }
        public void SetGameInfo(string responseBody)
        {
            JObject json = JObject.Parse(responseBody);
            var appList = json["applist"]["apps"];
            foreach (var app in appList)
            {
                Games.Add(new GameInfo { Id = Int32.Parse(app["appid"].ToString()), Name = app["name"].ToString() });
            }
        }
        public void SetGameDescription(string responseBody, GameInfo game)
        {
            if (string.IsNullOrEmpty(responseBody)) return;
            try
            {
                JObject gameJson = JObject.Parse(responseBody);
                var gameAppList = gameJson[game.Id.ToString()]["data"];
                var gameList = new List<string>();

                if (gameAppList == null) return;

                var genreList = gameAppList["genres"];
                if (genreList != null)
                {
                    foreach (var genre in genreList)
                    {
                        gameList.Add(genre["description"].ToString());
                    }
                }
                if (gameAppList["fullgame"] != null)
                {
                    game.ParentID = Int32.Parse(gameAppList["fullgame"]["appid"].ToString());
                }
                game.Free = gameAppList["is_free"].ToObject<bool>();
                game.Type = gameAppList["type"].ToString();
                game.Genre = gameList;
            }
            catch (Exception)
            {
                Console.WriteLine($"{responseBody} | {game.Id} , {game.Name}");
                throw;
            }

        }
    }
}
