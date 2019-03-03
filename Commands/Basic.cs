using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using brothersGM.Classes;
using RestSharp;

namespace brothersGM.Commands
{
    public class Basic : ModuleBase<SocketCommandContext>
    {
        [Command("gear")]
        public async Task showGearAsync() {
            List<item> inv = item.get_item();
            List<string> str = new List<string>();
            str.Add("```md");
            str.Add("< ID | Item Name | Weight | Value >");
            int count = 50;
            for (int i = 0; i < inv.Count; i++) {
                string toAdd = "";
                if ( i%2 == 0) {
                    toAdd = "> ";
                } else {
                    toAdd = "# ";
                }
                toAdd += i + ". | " + inv[i].name + " | " + inv[i].weight + " | " + inv[i].value;
                count += 2 + toAdd.Length;
                if (count >= 2000 ) {
                    str.Add("```");
                    await Context.User.SendMessageAsync(String.Join(System.Environment.NewLine,str));
                    str = new List<string>();
                    str.Add("```md");
                    str.Add("< ID | Item Name | Weight >");
                    count = 50 + toAdd.Length;
                }
                str.Add(toAdd);
            }
            str.Add("```");
            await Context.User.SendMessageAsync(String.Join(System.Environment.NewLine,str));
        }

        [Command("gear")]
        public async Task showItemAsync(int i) {
            item display = item.get_item(i);
            if ( display != null) await Context.Channel.SendMessageAsync("", false, display.toItemEmbed().Build());
            else await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry that ID doesn't match an item. Please use `bg!gear` for a list of items");
        }

        [Command("armor")]
        public async Task showArmorAsync() {
            List<armorItem> inv = armorItem.get_armorItem();
            List<string> str = new List<string>();
            str.Add("```md");
            str.Add("< ID | Item Name | Weight | Value | AC | Disadvantage on Stealth | Type >");
            int count = 100;
            armorItem aI;
            for (int i = 0; i < inv.Count; i++) {
                aI = inv[i];
                string toAdd = "";
                if ( i%2 == 0) {
                    toAdd = "> ";
                } else {
                    toAdd = "# ";
                }
                toAdd += i + ". | " + aI.name + " | " + aI.weight + " | " + aI.value + " | " + aI.ac + " | " + aI.disadvantage + " | " + armorItem.armorToString(aI.type);
                count += 2 + toAdd.Length;
                if (count >= 2000 ) {
                    str.Add("```");
                    await Context.User.SendMessageAsync(String.Join(System.Environment.NewLine,str));
                    str = new List<string>();
                    str.Add("```md");
                    str.Add("< ID | Item Name | Weight | Value | AC | Disadvantage on Stealth | Type >");
                    count = 100 + toAdd.Length;
                }
                str.Add(toAdd);
            }
            str.Add("```");
            await Context.User.SendMessageAsync(String.Join(System.Environment.NewLine,str));
        }

        [Command("armor")]
        public async Task showArmorItemAsync(int i) {
            armorItem display = armorItem.get_armorItem(i);
            if ( display != null) await Context.Channel.SendMessageAsync("", false, display.toArmorEmbed().Build());
            else await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry that ID doesn't match an item. Please use `bg!gear` for a list of items");
        }

        [Command("add")]
        public async Task addItemAsync(string name, string description, double value, double weight) {
            if ( Context.User.Id != 106768024857501696) return;
            item i = new item(name, description, value, weight);
            item.insert_item(i);
            await Context.Channel.SendMessageAsync("Item added",false,i.toItemEmbed().Build());
        }

        [Command("armAdd")]
        public async Task addArmorItemAsync(string name, string description, double value, double weight, int ac, bool stealth, string type) {
            if ( Context.User.Id != 106768024857501696) return;
            armorItem i = new armorItem(name,description,value,weight,ac,stealth,armorItem.stringToArmor(type));
            armorItem.insert_armorItem(i);
            await Context.Channel.SendMessageAsync("Armor Added",false,i.toArmorEmbed().Build());
        }
    }

}