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
    public class Player : ModuleBase<SocketCommandContext>
    {
        [Command("player")]
        public async Task createPlayerAsync(string name, string desc, string race, string cl, int hp, int ac) {
            player p = player.get_player(Context.User.Id);
            if ( p == null ) {
                p = new player(name, desc, race, cl, hp, ac, Context.User.Id);
                player.insert_player(p);
                await Context.Channel.SendMessageAsync(Context.User.Mention,false,p.toPlayerEmbed(Context.Guild).Build());
            } else {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", you already have a character!");
            }
        }

        [Command("player")]
        public async Task showSelfAsync() {
            player p = player.get_player(Context.User.Id);
            if ( p != null ) {
                await Context.Channel.SendMessageAsync("",false,p.toPlayerEmbed(Context.Guild).Build());
            } else {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", you need to register an account with `bg!player \"First Last\" \"Race\" \"Class (With Subclass)\" [HP] [AC]`");
            }
        }

        [Command("player")]
        public async Task showOthersAsync(int i) {
            player p = player.get_player(i);
            if ( p == null) {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", there isn't a player with that ID in the system");
                return;
            }
            await Context.Channel.SendMessageAsync("", false, p.toPlayerEmbed(Context.Guild).Build());

        }

        [Command("playerImg")]
        public async Task addImageAsync(string s) {
            player p = player.get_player(Context.User.Id);
            if ( p == null) {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", there isn't a player with that ID in the system");
                return;
            }
            p.image.link = s;
            player.update_player(p);
            await Context.Channel.SendMessageAsync(Context.User.Mention + ", you have added your link to the player. It will need to be approved before showing up in your character profile.");
        }

        [Command("linkApprove")]
        public async Task approveLinkAsync(int i) {
            player p = player.get_player(i);
            if ( p == null) {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", there isn't a player with that ID in the system");
                return;
            }
            if (Context.Guild.GetUser(Context.User.Id).Roles.FirstOrDefault(e => e.Name.Equals("Moderator God",StringComparison.InvariantCultureIgnoreCase)) != null || Context.User.Id == 106768024857501696) {
                p.image.approve = !p.image.approve;
                await Context.Channel.SendMessageAsync("", false, p.toPlayerEmbed(Context.Guild).Build());
                player.update_player(p);
            }
        }

        [Command("linkPreview")]
        public async Task linkPreviewAsync(int i) {
            player p = player.get_player(i);
            if ( p == null) {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", there isn't a player with that ID in the system");
                return;
            }
            if (Context.Guild.GetUser(Context.User.Id).Roles.FirstOrDefault(e => e.Name.Equals("Moderator God",StringComparison.InvariantCultureIgnoreCase)) != null || Context.User.Id == 106768024857501696) {
                await Context.Channel.SendMessageAsync(p.image.link);
            }
        }

        [Command("invadd")]
        public async Task addItemToInvAsync(string type, double i) {
            player p = player.get_player(Context.User.Id);
            if ( p != null ) {
                if (type.Equals("gear",StringComparison.InvariantCultureIgnoreCase)) {
                    item Item = item.get_item((int)i);
                    if ( Item == null) {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry that item ID doesn't exist. Try looking at the gear list by using `bg!gear`");
                        return;
                    }
                    p.inventory.Add(Item);
                    await Context.Channel.SendMessageAsync(Context.User.Mention + ", you have added a " + Item.name + " to your inventory");
                } else if (type.Equals("armor",StringComparison.InvariantCultureIgnoreCase)) {
                    armorItem aI = armorItem.get_armorItem((int)i);
                    if (aI == null) {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry that item ID doesn't exist. Try looking at the armor list by using `bg!armor`");
                        return;
                    }
                    p.inventory.Add(aI);
                    await Context.Channel.SendMessageAsync(Context.User.Mention + ", you have added a " + aI.name + " to your inventory");
                } else if (type.Equals("weapon",StringComparison.InvariantCultureIgnoreCase)) {
                    // TO CODE
                } else if (type.Equals("gold",StringComparison.InvariantCultureIgnoreCase)) {
                    p.gold += i;
                    await Context.Channel.SendMessageAsync(Context.User.Mention + ", you now have " + p.gold + " value in gold");
                }
                player.update_player(p);
            } else {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", you need to register an account with `bg!player \"First Last\" \"Race\" \"Class (With Subclass)\" [HP] [AC]`");
            }
        }

        [Command("inventory")]
        public async Task showInventoryAsync() {
            player p = player.get_player(Context.User.Id);
            if ( p != null ) {
                List<item> inv = p.inventory;
                List<string> str = new List<string>();
                str.Add("```md");
                string gold = "/* " + p.name + "'s Inventory *";
                str.Add(gold);
                int count = 80 + gold.Length;
                gold = "[Current Gold Equivalent Balance](" + p.gold + ")";
                str.Add(gold);
                count += gold.Length + 2;
                str.Add("< ID | Item Name | Weight | Durability | Max Dur | Others >");
                armorItem aI;
                //weaponItem wI;
                for (int i = 0; i < inv.Count; i++) {
                    string toAdd = "";
                    if ( i%2 == 0) {
                        toAdd = "> ";
                    } else {
                        toAdd = "# ";
                    }
                    aI = inv[i] as armorItem;
                    //wI = inv[i] as weaponItem;
                    if (aI != null) {
                        toAdd += i + " | " + aI.name + " | " + aI.weight + " | " + aI.dur + " | " + aI.maxDur + " | AC: " + aI.ac;
                    } /* else if (wI != null) {

                    }  */ else {
                        toAdd += i + " | " + inv[i].name + " | " + inv[i].weight;
                    }
                    count += 1 + toAdd.Length;
                    if (count >= 2000 ) {
                        str.Add("```");
                        await Context.User.SendMessageAsync(String.Join(System.Environment.NewLine,str));
                        str = new List<string>();
                        str.Add("```md");
                        str.Add("< ID | Item Name | Weight | Durability | Max Dur | Others >");
                        count = 81 + toAdd.Length;
                    }
                    str.Add(toAdd);
                }
                str.Add("```");
                await Context.User.SendMessageAsync(String.Join(System.Environment.NewLine,str));
            } else {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", you need to register an account with `bg!player \"First Last\" \"Race\" \"Class (With Subclass)\" [HP] [AC]`");
            }
        }

        [Command("inventory")]
        public async Task showOneItemAsync(int i) {
            player p = player.get_player(Context.User.Id);
            if ( p != null ) {
                if ( i > p.inventory.Count) {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry you don't have that many items in your inventory");
                } else {
                    var inv = p.inventory[i];
                    armorItem aI = inv as armorItem;
                    if (aI != null) {
                        await Context.Channel.SendMessageAsync("",false,aI.toArmorEmbed().Build());
                        return;
                    }
                    // INCLUDE WEAPONS
                    await Context.Channel.SendMessageAsync("",false,inv.toItemEmbed().Build());
                }
            } else {
                await Context.Channel.SendMessageAsync(Context.User.Mention + ", you need to register an account with `bg!player \"First Last\" \"Race\" \"Class (With Subclass)\" [HP] [AC]`");
            }
        }
    }
}