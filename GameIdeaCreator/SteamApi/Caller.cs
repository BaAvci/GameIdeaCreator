
namespace GameIdeaCreator
{
    public class Caller
    {
        static HttpClient client = new HttpClient();
        public async Task<string> GetContent(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Following URL could not be called: {url}");
                throw;
            }
            return "";
        }
    }
}
