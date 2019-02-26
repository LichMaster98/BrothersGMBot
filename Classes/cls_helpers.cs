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

    public class helpers {
        public static string center(string s, int i) {
            string spaces = "";
            int toCenter = i - s.Length;
            for(int j = 0; j < toCenter/2; j++) {
                spaces += " ";
            }
            if(toCenter % 2 == 1) {
                return " " + spaces + s + spaces;
            } else {
                return spaces + s + spaces;
            }
        }
    }
}