using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
namespace ConsoleWebApiClient
{
    class Program
    {

        public class Repository
        {
            public string name { get; set; }
            public string language { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        private static async Task<List<Repository>> ProcessRepositories()
        {
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            //var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
            //var msg = await stringTask;
            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            foreach (var repo in repositories)
                Console.WriteLine(repo.name + " " + repo.language);
            return repositories;
           
           
        }

        static void Main(string[] args)
        {
            Task.WhenAny(ProcessRepositories());
            Task t =Task.CompletedTask;
            
            
           
            Console.ReadLine();
        }
       
    }
}
