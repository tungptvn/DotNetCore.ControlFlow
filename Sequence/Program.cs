using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sequence
{
      class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadKey();

        }
        public static async Task MainAsync(string[] args)
        {
            var str = await GetGitHubUsers.UsingAsync(
                () =>
                {
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET http client");
                    return httpClient;
                },
                async client => await client.GetStringAsync("https://api.github.com/users")
                );
            Console.Write($"{str}");
        }
    }
    public static class GetGitHubUsers
    {
        public static async Task<TResult> UsingAsync<TDisposable, TResult>(
            Func<TDisposable> httpClient,
            Func<TDisposable, Task<TResult>> getString)
            where TDisposable : IDisposable
        {
            using (var disposable = httpClient())
            {
                return await getString(disposable);
            }
        }

    }
}
