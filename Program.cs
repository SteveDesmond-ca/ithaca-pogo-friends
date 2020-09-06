using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace IthacaPoGoFriends
{
    public static class Program
    {

        public static async Task Main(string[] args)
        {
            var contents = await File.ReadAllLinesAsync("friends.txt");
            var friends = new List<Friend>();
            foreach (var line in contents)
            {
                var fields = line.Split("\t");
                var friend = new Friend();
                friend.name = fields[0];
                friend.username = fields[1];
                friend.code = fields[2];
                friend.raids = Convert.ToBoolean(fields[3]);
                friend.friendship = Convert.ToBoolean(fields[4]);
                friend.pvp = Convert.ToBoolean(fields[5]);
                friend.trading = Convert.ToBoolean(fields[6]);
                friends.Add(friend);
            }
            var json = JsonSerializer.Serialize(friends);
            await File.WriteAllTextAsync("friends.json", json);
        }
    }
}
