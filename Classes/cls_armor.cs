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

    public enum armorType {
        Light,
        Medium,
        Heavy,
        Shield,
        none
    }

    public enum shieldMaterial {
        Cloth,
        Paper,
        Crystal,
        Glass,
        Ice,
        Wood,
        Bone,
        Stone,
        Iron,
        Steel,
        Mithral,
        Adamantine
    }

    public partial class armorItem : marketItem {
        [JsonProperty("MaxDur")]
        public int maxDur { get; set; }

        [JsonProperty("Durability")]
        public int dur { get; set; }

        [JsonProperty("AC")]
        public int ac { get; set; }

        [JsonProperty("Stealth")]
        public bool disadvantage { get; set; }

        [JsonProperty("Type")]
        public armorType type { get; set; }

        [JsonProperty("Material")]
        public shieldMaterial material { get; set; }

        public armorItem(string name, string description, double value, double weight, int a, bool s, armorType aT) {
            this.name = name;
            this.description = description;
            this.value = value;
            this.weight = weight;
            this.ac = a;
            this.disadvantage = s;
            this.type = aT;
        }

        public EmbedBuilder toArmorEmbed() {
            EmbedBuilder embed = this.toItemEmbed();
            embed.WithFooter("Armor Item ID: " + ID);
            embed.AddField("AC", ac, true);
            if (type != armorType.Shield) embed.AddField("Type", armorItem.armorToString(type), true);
            else embed.AddField("Shield Material", material, true);
            embed.AddField("Durability", dur + " / " + maxDur, true);
            return embed;
        }
    }

    public partial class armorItem
    {
        public static string armorToString(armorType t) {
            switch (t) {
                case armorType.Light:
                return "Light";
                case armorType.Medium:
                return "Medium";
                case armorType.Heavy:
                return "Heavy";
                case armorType.Shield:
                return "Shield";
                default:
                return null;
            }
        }
        public static armorType stringToArmor(string s) {
            if (s.Equals("light",StringComparison.InvariantCultureIgnoreCase)) {
                return armorType.Light;
            } else if (s.Equals("medium",StringComparison.InvariantCultureIgnoreCase)) {
                return armorType.Medium;
            } else if (s.Equals("heavy",StringComparison.InvariantCultureIgnoreCase)) {
                return armorType.Heavy;
            } else if (s.Equals("shield",StringComparison.InvariantCultureIgnoreCase)) {
                return armorType.Shield;
            } else {
                return armorType.none;
            }
        }
        public static Dictionary<armorType,string> armorDur = new Dictionary<armorType, string> {
            {armorType.Light,"3d6"},
            {armorType.Medium,"4d8"},
            {armorType.Heavy,"5d10"}
        };

        public static Dictionary<shieldMaterial,int> shieldDur = new Dictionary<shieldMaterial, int> {
            {shieldMaterial.Cloth,5},
            {shieldMaterial.Paper,5},
            {shieldMaterial.Crystal,5},
            {shieldMaterial.Glass,5},
            {shieldMaterial.Ice,5},
            {shieldMaterial.Wood,10},
            {shieldMaterial.Bone,10},
            {shieldMaterial.Stone,15},
            {shieldMaterial.Iron,20},
            {shieldMaterial.Steel,20},
            {shieldMaterial.Mithral,25},
            {shieldMaterial.Adamantine,Int32.MaxValue},
        };

        public static armorItem[] FromJson(string json) => JsonConvert.DeserializeObject<armorItem[]>(json, Converter.Settings);

        public static List<armorItem> get_armorItem () {
            var store = new DataStore ("armorItem.json");

            // Get employee collection
            var rtrner = store.GetCollection<armorItem> ().AsQueryable ().ToList();
            store.Dispose();
            return rtrner;
        }

        public static armorItem get_armorItem (int id) {
            var store = new DataStore ("armorItem.json");

            // Get employee collection
            var rtrner = store.GetCollection<armorItem> ().AsQueryable ().FirstOrDefault (e => e.ID == id);
            store.Dispose();
            return rtrner;
        }

        public static armorItem get_armorItem (string name) {
            var store = new DataStore ("armorItem.json");

            // Get employee collection
            var rtrner = store.GetCollection<armorItem> ().AsQueryable ().FirstOrDefault (e => e.name == name);
            store.Dispose();
            return rtrner;
        }

        public static void insert_armorItem (armorItem armorItem) {
            var store = new DataStore ("armorItem.json");

            // Get employee collection
            store.GetCollection<armorItem> ().InsertOneAsync (armorItem);

            store.Dispose();
        }

        public static void update_armorItem (armorItem armorItem) {
            var store = new DataStore ("armorItem.json");

            store.GetCollection<armorItem> ().ReplaceOneAsync (e => e.ID == armorItem.ID, armorItem);
            store.Dispose();
        }

        public static void delete_armorItem (armorItem armorItem) {
            var store = new DataStore ("armorItem.json");

            store.GetCollection<armorItem> ().DeleteOne (e => e.ID == armorItem.ID);
            store.Dispose();
        }
    }
}