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

    public partial class item {
        [JsonProperty ("ID")]
        public int ID { get; set; }

       [JsonProperty("Name")]
       public string name { get; set; }

       [JsonProperty("Description")]
       public string description { get; set; }

       [JsonProperty("Value")]
       public double value { get; set; }
       
       [JsonProperty("Weight")]
       public double weight;

       public item() {

       }

       public item(string n, string descr, double v, double w) {
           name = n;
           description = descr;
           value = v;
           weight = w;
       }

       public item(string n, string d, double c, int MX, int D, double V, double w, int q) {
           name = n;
           description = d;
           value = V;
           weight = w;
       }

       public EmbedBuilder toItemEmbed() {
           var embed = new EmbedBuilder();
           embed.Title = name;
           embed.Description = description;
           embed.AddField("Value in Gold",value,true);
           embed.AddField("Weight in Pounds",weight,true);
           embed.WithColor(221, 162, 0);
           embed.WithFooter("Base item ID: " + ID);
           return embed;
       }

    }
    public partial class item
    {
        public static item[] FromJson(string json) => JsonConvert.DeserializeObject<item[]>(json, Converter.Settings);

        public static List<item> get_item () {
            var store = new DataStore ("item.json");

            // Get employee collection
            var rtrner = store.GetCollection<item> ().AsQueryable ().ToList();
            store.Dispose();
            return rtrner;
        }

        public static item get_item (int id) {
            var store = new DataStore ("item.json");

            // Get employee collection
            var rtrner = store.GetCollection<item> ().AsQueryable ().FirstOrDefault (e => e.ID == id);
            store.Dispose();
            return rtrner;
        }

        public static item get_item (string name) {
            var store = new DataStore ("item.json");

            // Get employee collection
            var rtrner = store.GetCollection<item> ().AsQueryable ().FirstOrDefault (e => e.name.Equals(name,StringComparison.InvariantCultureIgnoreCase));
            store.Dispose();
            return rtrner;
        }

        public static void insert_item (item item) {
            var store = new DataStore ("item.json");

            // Get employee collection
            store.GetCollection<item> ().InsertOneAsync (item);

            store.Dispose();
        }

        public static void update_item (item item) {
            var store = new DataStore ("item.json");

            store.GetCollection<item> ().ReplaceOneAsync (e => e.ID == item.ID, item);
            store.Dispose();
        }

        public static void delete_item (item item) {
            var store = new DataStore ("item.json");

            store.GetCollection<item> ().DeleteOne (e => e.ID == item.ID);
            store.Dispose();
        }
    }
}