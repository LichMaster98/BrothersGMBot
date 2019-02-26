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