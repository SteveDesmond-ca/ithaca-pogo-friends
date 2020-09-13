using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace IthacaPoGoFriends
{
    public static class Program
    {

        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var url = $"https://sheets.googleapis.com/v4/spreadsheets/{config["spreadsheetID"]}/values/A2:H200?key={config["key"]}";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var contents = await response.Content.ReadAsStreamAsync();
            var jsonIn = await JsonSerializer.DeserializeAsync<JsonElement>(contents);
            var friends = new List<Friend>();
            foreach (var person in jsonIn.GetProperty("values").EnumerateArray())
            {
                var fields = person.EnumerateArray().ToArray<JsonElement>();
                var friend = new Friend();

                friend.name = fields[0].GetString();
                friend.username = fields[1].GetString();
                friend.code = fields[2].GetString();

                if (string.IsNullOrWhiteSpace(friend.name) && string.IsNullOrWhiteSpace(friend.username) && string.IsNullOrWhiteSpace(friend.code))
                    continue;

                friend.raids = Convert.ToBoolean(fields[3].GetString());
                friend.friendship = Convert.ToBoolean(fields[4].GetString());
                friend.pvp = Convert.ToBoolean(fields[5].GetString());
                friend.trading = Convert.ToBoolean(fields[6].GetString());

                friends.Add(friend);
            }
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonOut = JsonSerializer.Serialize(friends, options);
            await File.WriteAllTextAsync("friends.json", jsonOut);
        }
    }
}
