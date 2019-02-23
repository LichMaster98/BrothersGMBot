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

    public partial class marketItem : item {
        [JsonProperty("Cost")]
        public double cost { get; set; }

        [JsonProperty("Max Durability")]
        public int maxDurability { get; set; }

        [JsonProperty("Durability")]
        public int durability { get; set; }
    
        [JsonProperty("Quantity")]
        public int quantity;

        public marketItem(string n, string d, double c, int MX, int D, double V, double w, int q) {
            name = n;
            description = d;
            value = V;
            weight = w;
            cost = c;
            maxDurability = MX;
            durability = D;
            quantity = q;
        }
    }
    public partial class marketItem
    {
        public static marketItem[] FromJson(string json) => JsonConvert.DeserializeObject<marketItem[]>(json, Converter.Settings);

        public static List<marketItem> get_marketItem () {
            var store = new DataStore ("marketItem.json");

            // Get employee collection
            var rtrner = store.GetCollection<marketItem> ().AsQueryable ().ToList();
            store.Dispose();
            return rtrner;
        }

        public static marketItem get_marketItem (int id) {
            var store = new DataStore ("marketItem.json");

            // Get employee collection
            var rtrner = store.GetCollection<marketItem> ().AsQueryable ().FirstOrDefault (e => e.ID == id);
            store.Dispose();
            return rtrner;
        }

        public static marketItem get_marketItem (string name) {
            var store = new DataStore ("marketItem.json");

            // Get employee collection
            var rtrner = store.GetCollection<marketItem> ().AsQueryable ().FirstOrDefault (e => e.name == name);
            store.Dispose();
            return rtrner;
        }

        public static void insert_marketItem (marketItem marketItem) {
            var store = new DataStore ("marketItem.json");

            // Get employee collection
            store.GetCollection<marketItem> ().InsertOneAsync (marketItem);

            store.Dispose();
        }

        public static void update_marketItem (marketItem marketItem) {
            var store = new DataStore ("marketItem.json");

            store.GetCollection<marketItem> ().ReplaceOneAsync (e => e.ID == marketItem.ID, marketItem);
            store.Dispose();
        }

        public static void delete_marketItem (marketItem marketItem) {
            var store = new DataStore ("marketItem.json");

            store.GetCollection<marketItem> ().DeleteOne (e => e.ID == marketItem.ID);
            store.Dispose();
        }
    }
}