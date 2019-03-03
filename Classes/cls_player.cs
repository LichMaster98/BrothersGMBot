using System.Collections.Generic;
using System.Globalization;
using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using JsonFlatFileDataStore;
using System.Linq;

namespace brothersGM.Classes {

    public partial class player {
        [JsonProperty ("ID")]
        public int ID { get; set; }

        [JsonProperty("Name")]
        public string name { get; set; }

        [JsonProperty("Description")]
        public string description { get; set; }

        [JsonProperty("DiscordID")]
        public ulong discordID { get; set; }

        [JsonProperty("HP")]
        public int hp { get; set;}

        [JsonProperty("AC")]
        public int ac { get; set; }

        [JsonProperty("Race")]
        public string race { get; set;  }

        [JsonProperty("Class")]
        public string Class { get; set; }

        [JsonProperty("Inventory")]
        public List<item> inventory { get; set; } = new List<item>();

        [JsonProperty("Image")]
        public Tuple<string, bool> image { get; set; } = new Tuple<string, bool>("",false);

        [JsonProperty("Color")]
        public byte[] colors { get; set; } = {0,0,0};

        [JsonProperty("Gold")]
        public double gold { get; set; }

        public player(string n, string d, string r, string c, int h, int a, ulong id) {
            name = n;
            description = d;
            race = r;
            Class = c;
            hp = h;
            ac = a;
            discordID = id;
            gold = 0.0;
        }

        public EmbedBuilder toPlayerEmbed(SocketGuild Guild) {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle(name);
            embed.WithDescription(description);
            embed.WithAuthor(Guild.GetUser(discordID));
            embed.AddField("HP", hp, true);
            embed.AddField("AC", ac, true);
            embed.AddField("Race", race, true);
            embed.AddField("Class", Class, true);
            embed.WithColor(colors[0], colors[1], colors[2]);
            embed.WithFooter("Player ID: " + ID, "https://thebrothersgm.files.wordpress.com/2018/02/cropped-transparent-blog-logo1.png");
            if (image.Item2) embed.WithThumbnailUrl(image.Item1);
            return embed;
        }
    }
    public partial class player
    {
        public static player[] FromJson(string json) => JsonConvert.DeserializeObject<player[]>(json, Converter.Settings);

        public static List<player> get_player () {
            var store = new DataStore ("player.json");

            // Get employee collection
            var rtrner = store.GetCollection<player> ().AsQueryable ().ToList();
            store.Dispose();
            return rtrner;
        }

        public static player get_player (int id) {
            var store = new DataStore ("player.json");

            // Get employee collection
            var rtrner = store.GetCollection<player> ().AsQueryable ().FirstOrDefault (e => e.ID == id);
            store.Dispose();
            return rtrner;
        }

        public static player get_player (string name) {
            var store = new DataStore ("player.json");

            // Get employee collection
            var rtrner = store.GetCollection<player> ().AsQueryable ().FirstOrDefault (e => e.name == name);
            store.Dispose();
            return rtrner;
        }

        public static player get_player (ulong discord) {
            var store = new DataStore ("player.json");

            // Get employee collection
            var rtrner = store.GetCollection<player> ().AsQueryable ().FirstOrDefault (e => e.discordID == discord);
            store.Dispose();
            return rtrner;
        }

        public static void insert_player (player player) {
            var store = new DataStore ("player.json");

            // Get employee collection
            store.GetCollection<player> ().InsertOneAsync (player);

            store.Dispose();
        }

        public static void update_player (player player) {
            var store = new DataStore ("player.json");

            store.GetCollection<player> ().ReplaceOneAsync (e => e.ID == player.ID, player);
            store.Dispose();
        }

        public static void delete_player (player player) {
            var store = new DataStore ("player.json");

            store.GetCollection<player> ().DeleteOne (e => e.ID == player.ID);
            store.Dispose();
        }
    }
}