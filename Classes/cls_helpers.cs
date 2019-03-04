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

        public enum damageType {
        Acid,
        Bludgeoning,
        Cold,
        Fire,
        Force,
        Lightning,
        Necrotic,
        Piercing,
        Poison,
        Psychic,
        Radiant,
        Slashing,
        Thunder,
        Null
    }

    public class helpers {
        public static string damageTypeToString(damageType t) {
            switch (t) {
                case damageType.Acid:
                return "Acid";
                case damageType.Bludgeoning:
                return "Bludgeoning";
                case damageType.Cold:
                return "Cold";
                case damageType.Fire:
                return "Fire";
                case damageType.Force:
                return "Force";
                case damageType.Lightning:
                return "Lightning";
                case damageType.Necrotic:
                return "Necrotic";
                case damageType.Piercing:
                return "Piercing";
                case damageType.Poison:
                return "Poison";
                case damageType.Psychic:
                return "Psychic";
                case damageType.Radiant:
                return "Radiant";
                case damageType.Slashing:
                return "Slashing";
                case damageType.Thunder:
                return "Thunder";
                default:
                return null;
            }
        }

        public static damageType stringToDamageType(string s) {
            if (s.Equals("acid",StringComparison.InvariantCultureIgnoreCase)) return damageType.Acid;
            else if (s.Equals("Bludgeoning",StringComparison.InvariantCultureIgnoreCase)) return damageType.Bludgeoning;
            else if (s.Equals("cold",StringComparison.InvariantCultureIgnoreCase)) return damageType.Cold;
            else if (s.Equals("fire",StringComparison.InvariantCultureIgnoreCase)) return damageType.Fire;
            else if (s.Equals("force",StringComparison.InvariantCultureIgnoreCase)) return damageType.Force;
            else if (s.Equals("lightning",StringComparison.InvariantCultureIgnoreCase)) return damageType.Lightning;
            else if (s.Equals("necrotic",StringComparison.InvariantCultureIgnoreCase)) return damageType.Necrotic;
            else if (s.Equals("piercing",StringComparison.InvariantCultureIgnoreCase)) return damageType.Piercing;
            else if (s.Equals("poison",StringComparison.InvariantCultureIgnoreCase)) return damageType.Poison;
            else if (s.Equals("psychic",StringComparison.InvariantCultureIgnoreCase)) return damageType.Psychic;
            else if (s.Equals("radiant",StringComparison.InvariantCultureIgnoreCase)) return damageType.Radiant;
            else if (s.Equals("slashing",StringComparison.InvariantCultureIgnoreCase)) return damageType.Slashing;
            else if (s.Equals("thunder",StringComparison.InvariantCultureIgnoreCase)) return damageType.Thunder;
            else return damageType.Null;
        }
    }
}