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
    
        [JsonProperty("Quantity")]
        public int quantity;

        [JsonProperty("ShortDescription")]
        public string shortDesc;
        [JsonProperty("Seller")]
        public ulong sellerID;
        public marketItem(string n, string d, double c, double V, double w, int q, string sd) {
            name = n;
            description = d;
            value = V;
            weight = w;
            cost = c;
            quantity = q;
            shortDesc = sd;
        }
        
        public marketItem(item Item, double c, int q, ulong sI) {
            name = Item.name;
            description = Item.description;
            value = Item.value;
            weight = Item.weight;
            cost = c;
            quantity = q;
            sellerID = sI;
        }

        public marketItem(item Item, double c, int q, string d) {
            name = Item.name;
            description = Item.description;
            value = Item.value;
            weight = Item.weight;
            cost = c;
            quantity = q;
            shortDesc = d;
        }
        public marketItem() { //Needed for inheritence

        }

        public EmbedBuilder toMarketEmbed() {
            var embed = toItemEmbed();
            embed.Title = name;
            embed.Description = description + System.Environment.NewLine + System.Environment.NewLine + shortDesc; /// @todo Check if this is over 2048 characters
            embed.AddField("Cost in Gold",cost,true);
            embed.AddField("Quantity in Stock",quantity,true);
            embed.WithColor(21, 181, 135);
            return embed;
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