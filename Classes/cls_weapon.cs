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

    public enum weaponProperty {
        Ammunition,
        Finesse,
        Heavy,
        Lance,
        Light,
        Loading,
        Net,
        Range,
        Reach,
        Thrown,
        Two_Handed,
        Versatile,
        Silvered,
        Null
    }

    public enum weaponType {
        simpleMelee,
        simpleRanged,
        martialMelee,
        martialRanged,
        improvised
    }

    public partial class weapon : marketItem {
        [JsonProperty("Properties")]        
        public List<weaponProperty> properties { get; set; } = new List<weaponProperty>();

        [JsonProperty("Damage Dice")]
        public string damage { get; set; }

        [JsonProperty("Damage Type")]
        public damageType dType{ get; set;}

        [JsonProperty("Weapon Type")]
        public weaponType wType { get; set; }

        public weapon(string name, double value, double weight, string damage, string Dtype, string Wtype) {
            this.name = name;
            this.value = value;
            this.weight = weight;
            this.damage = damage;
            this.dType = helpers.stringToDamageType(Dtype);
            this.wType = weapon.stringToType(Wtype);
        }

        public EmbedBuilder toWeaponEmbed() {
            EmbedBuilder embed = this.toItemEmbed();
            embed.WithFooter("Weapon Item ID: " + ID);
            embed.AddField("Damage",damage,true);
            embed.AddField("Weapon Type", weapon.typeToString(wType));
            foreach (weaponProperty w in properties) {
                embed.AddField(weapon.propertyToName(w),weapon.propertyDetail[w]);
            }
            return embed;
        }

        public string propertiesList() {
            string rtn = "";
            foreach (var p in properties) {
                rtn += " | " + weapon.propertyToName(p);
            }
            return rtn;
        }

    }

    public partial class weapon  
    {
        public static weaponType stringToType(string s) {
            if (s.Equals("simple ranged",StringComparison.InvariantCultureIgnoreCase)) return weaponType.simpleRanged;
            else if (s.Equals("simple melee",StringComparison.InvariantCultureIgnoreCase)) return weaponType.simpleMelee;
            else if (s.Equals("martial ranged",StringComparison.InvariantCultureIgnoreCase)) return weaponType.martialRanged;
            else if (s.Equals("martial melee",StringComparison.InvariantCultureIgnoreCase)) return weaponType.martialRanged;
            else return weaponType.improvised;
        }
        public static string typeToString(weaponType t) {
            switch(t) {
                case weaponType.simpleRanged:
                return "Simple Ranged";
                case weaponType.simpleMelee:
                return "Simple Melee";
                case weaponType.martialRanged:
                return "Martial Range";
                case weaponType.martialMelee:
                return "Martial Melee";
                case weaponType.improvised:
                return "Improvised";
                default:
                return null;
            }
        }
        public static Dictionary<weaponProperty,string> propertyDetail = new Dictionary<weaponProperty, string> {
            {weaponProperty.Ammunition,"You can use a weapon that has the Ammunition property to make a ranged Attack only if you have ammuni⁠tion to fire from the weapon. Each time you atta⁠ck with the weapon, you expend one piece of ammu⁠nition. Drawing the ammuni⁠tion from a Quiver, case, or other container is part of the atta⁠ck (you need a free hand to load a one-handed weapon). At the end of the battle, you can recover half your expended ammu⁠nition by taking a minute to sear⁠ch the battlefield."},
            {weaponProperty.Finesse,"When Making an Attack with a finesse weapon, you use your choice of your Strength or Dexterity modifier for the Attack and Damage Rolls. You must use the same modifier for both rolls."},
            {weaponProperty.Heavy,"Small creatures have disadvantage on Attack rolls with heavy Weapons. A heavy weapon’s size and bulk make it too large for a Small creature to use effectively. "},
            {weaponProperty.Lance,"You have disadvantage when you use a lance to Attack a target within 5 feet of you. Also, a lance requires two hands to wield when you aren’t mounted."},
            {weaponProperty.Light,"A lig⁠ht weapon is small and easy to handle, making it ideal for use when fighting with two Weapons. See the rules for two-weapon fighting in \" Making an Attack \" section."},
            {weaponProperty.Loading,"Because of the time required to load this weapon, you can fire only one piece of Ammunition from it when you use an action, Bonus Action, or Reaction to fire it, regardless of the number of attacks you can normally make."},
            {weaponProperty.Net,"A Large or smaller creature hit by a net is Restrained until it is freed. A net has no effect on creatures that are formless, or creatures that are Huge or larger. A creature can use its action to make a DC 10 Strength check, freeing itself or another creature within its reach on a success. Dealing 5 slashing damage to the net (AC 10) also frees the creature without harming it, ending the effect and destroying the net." + System.Environment.NewLine + System.Environment.NewLine + "When you use an action, Bonus Action, or Reaction to Attack with a net, you can make only one atta⁠ck regardless of the number of attacks you can normally make."},
            {weaponProperty.Range,"A weapon that can be used to make a ranged Attack has a range in parentheses after the Ammunition or thrown property. The range lists two numbers. The first is the weapon’s normal range in feet, and the second indicates the weapon’s long range. When attacking a target beyond normal range, you have disadvantage on the Attack roll. You can’t Attack a target beyond the weapon’s long range."},
            {weaponProperty.Reach,"This weapon adds 5 feet to your reach when you Attack with it, as well as when determining your reach for Opportunity Attacks with it (see Making an Attack)."},
            {weaponProperty.Silvered,"Some Monsters that have immunity or Resistance to nonmagical Weapons are susceptible to silver weap⁠ons, so cautious adventurers invest extra coin to plate their weap⁠ons with silver. You can silver a single weapon or ten pieces of Ammunition for 100 gp. This cost represents not only the price of the silver, but the time and expertise needed to add silver to the weapon without making it less effective."},
            {weaponProperty.Thrown,"If a weapon has the thrown property, you can throw the weapon to make a ranged Attack. If the weapon is a melee weapon, you use the same ability modifier for that atta⁠ck roll and damage roll that you would use for a melee att⁠ack with the weapon. For example, if you throw a Handaxe, you use your Strength, but if you throw a Dagger, you can use either your Strength or your Dexterity, since the Dagger has the finesse property."},
            {weaponProperty.Two_Handed,"This weapon requires two hands when you Attack with it."},
            {weaponProperty.Versatile,"This weapon can be used with one or two hands. A damage value in parentheses appears with the property—the damage when the weapon is used with two hands to make a melee Attack."}
        };

        public static weaponProperty stringToProperty(string s) {
            if (s.Equals("ammunition",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Ammunition;
            else if (s.Equals("finesse",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Finesse;
            else if (s.Equals("heavy",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Heavy;
            else if (s.Equals("lance",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Lance;
            else if (s.Equals("light",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Light;
            else if (s.Equals("loading",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Loading;
            else if (s.Equals("net",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Net;
            else if (s.Equals("range",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Range;
            else if (s.Equals("reach",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Reach;
            else if (s.Equals("silvered",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Silvered;
            else if (s.Equals("thrown",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Thrown;
            else if (s.Equals("Two Handed",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Two_Handed;
            else if (s.Equals("Versatile",StringComparison.InvariantCultureIgnoreCase)) return weaponProperty.Versatile;
            else return weaponProperty.Null;
        }

        public static string propertyToName(weaponProperty p) {
            switch(p) {
                case weaponProperty.Ammunition:
                return "Ammunition";
                case weaponProperty.Finesse:
                return "Finesse";
                case weaponProperty.Heavy:
                return "Heavy";
                case weaponProperty.Lance:
                return "Lance";
                case weaponProperty.Light:
                return "Light";
                case weaponProperty.Loading:
                return "Loading";
                case weaponProperty.Net:
                return "Net";
                case weaponProperty.Range:
                return "Range";
                case weaponProperty.Reach:
                return "Reach";
                case weaponProperty.Silvered:
                return "Silvered";
                case weaponProperty.Thrown:
                return "Thrown";
                case weaponProperty.Two_Handed:
                return "Two Handed";
                case weaponProperty.Versatile:
                return "Versatile";
                default:
                return null;
            }
        }
        public static weapon[] FromJson(string json) => JsonConvert.DeserializeObject<weapon[]>(json, Converter.Settings);

        public static List<weapon> get_weapon () {
            var store = new DataStore ("weapon.json");

            // Get employee collection
            var rtrner = store.GetCollection<weapon> ().AsQueryable ().ToList();
            store.Dispose();
            return rtrner;
        }

        public static weapon get_weapon (int id) {
            var store = new DataStore ("weapon.json");

            // Get employee collection
            var rtrner = store.GetCollection<weapon> ().AsQueryable ().FirstOrDefault (e => e.ID == id);
            store.Dispose();
            return rtrner;
        }

        public static weapon get_weapon (string name) {
            var store = new DataStore ("weapon.json");

            // Get employee collection
            var rtrner = store.GetCollection<weapon> ().AsQueryable ().FirstOrDefault (e => e.name.Equals(name,StringComparison.InvariantCultureIgnoreCase));
            store.Dispose();
            return rtrner;
        }

        public static void insert_weapon (weapon weapon) {
            var store = new DataStore ("weapon.json");

            // Get employee collection
            store.GetCollection<weapon> ().InsertOneAsync (weapon);

            store.Dispose();
        }

        public static void update_weapon (weapon weapon) {
            var store = new DataStore ("weapon.json");

            store.GetCollection<weapon> ().ReplaceOneAsync (e => e.ID == weapon.ID, weapon);
            store.Dispose();
        }

        public static void delete_weapon (weapon weapon) {
            var store = new DataStore ("weapon.json");

            store.GetCollection<weapon> ().DeleteOne (e => e.ID == weapon.ID);
            store.Dispose();
        }
    }
}