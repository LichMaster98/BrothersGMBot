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
    public class Market : ModuleBase<SocketCommandContext>
    {
        [Command("market")]
        public async Task marketHandlerAsync(params string[] inputs) {
            if ( inputs.Length == 0) {
                await Context.Channel.SendMessageAsync("Welcome");
            } else {
                if ( inputs[0].Equals("add",StringComparison.InvariantCultureIgnoreCase)) { // GMs adding Item to Store
                    if (Context.Guild.GetUser(Context.User.Id).Roles.FirstOrDefault(e => e.Name.Equals("Moderator God",StringComparison.InvariantCultureIgnoreCase)) != null || Context.User.Id == 106768024857501696) {
                        if (inputs.Length <= 2) {
                            await Context.Channel.SendMessageAsync(Context.User.Mention + ", please signify what item to add to the market either by ID or \"Item Name\"");
                            return;
                        }
                        int value;
                        if (Int32.TryParse(inputs[1], out value)) { // Received Item ID to build from
                            item i = item.get_item(value);
                            if ( i == null ) {
                                await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry. You didn't specify a valid item ID");
                                return;
                            }
                            double cost;
                            if (!Double.TryParse(inputs[2], out cost)) {
                                await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry. You didn't specify a valid item cost in GP. Saw: " + inputs[2]);
                                return;
                            }
                            int quantity;
                            if (!Int32.TryParse(inputs[3], out quantity)) {
                                await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry. You didn't specify a valid quantity of items");
                                return; 
                            }
                            marketItem mI = new marketItem(i, cost, quantity, 548668447719161858);
                            marketItem.insert_marketItem(mI);
                            await Context.Channel.SendMessageAsync(Context.User.Mention + ", you have added the following item to the market.",false,mI.toMarketEmbed().Build());
                        } else { // Received Item Name
                            string name;
                            int index = 1;
                        }
                    }
                } else if (inputs[0].Equals("gear",StringComparison.InvariantCultureIgnoreCase)) { // Standard Adventuring Gear
                    List<marketItem> market = marketItem.get_marketItem();
                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithTitle("Fairmaden Market - Adventuring Gear");
                    embed.WithCurrentTimestamp();
                    embed.WithDescription("For more details about any item on the market please type `bg!market [ID]`");
                    if (inputs.Length >= 2) {
                        int value;
                        if (Int32.TryParse(inputs[1], out value)) {
                            int start = (value - 1) * 25;
                            if (start > market.Count) {
                                await Context.Channel.SendMessageAsync(Context.User.Mention + ", sorry. There are only " + (market.Count / 25 + 1) + " pages available.");
                                return;
                            }
                            embed.WithFooter("Page " + value + " out of " + (market.Count / 25 + 1));
                            for (int i = 0; i < 25 && i+start < market.Count; i++) {
                                embed.AddField(market[i+start].name,"Cost: " + market[i+start].cost + " GP | Quantity: " + market[i+start].quantity + " | ID: " + market[i+start].ID);
                            }
                        } else {
                            // Couldn't parse page, display page 1
                            embed.WithFooter("Page 1 out of " + (market.Count / 25 + 1));
                            for (int i = 0; i < 25 && i < market.Count; i++) {
                                embed.AddField(market[i].name,"Cost: " + market[i].cost + " GP | Quantity: " + market[i].quantity + " | ID: " + market[i].ID);
                            }
                        }
                        await Context.Channel.SendMessageAsync("",false,embed.Build());
                        return;
                    }
                    // Default Output
                    embed.WithFooter("Page 1 out of " + (market.Count / 25 + 1));
                    for (int i = 0; i < 25 && i < market.Count; i++) {
                        embed.AddField(market[i].name,"Cost: " + market[i].cost + " GP | Quantity: " + market[i].quantity + " | ID: " + market[i].ID);
                    }
                    await Context.Channel.SendMessageAsync("",false,embed.Build());
                } else if (inputs[0].Equals("sell",StringComparison.InvariantCultureIgnoreCase)) {
                    if (inputs.Length <3) {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", try `bg!market sell [Inv ID] [Price]`");
                        return;
                    }
                    player p = player.get_player(Context.User.Id);
                    if (p == null) {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", there isn't a player with that ID in the system");
                        return;
                    }
                    // input[1] - Item to Sell
                    // input[2] - Cost on the Market
                    if (Int32.TryParse(inputs[1], out int numItem)) {
                        if (numItem >= p.inventory.Count) {
                            await Context.Channel.SendMessageAsync(Context.User.Mention + ", the item ID doesn't exist. Try `bg!market sell [Inv ID] [Price]` or check `bg!inventory`");
                            return;
                        }
                        if (Double.TryParse(inputs[2], out double price)) {
                            armorItem aI = p.inventory[numItem] as armorItem;
                            // Add Weapon Item
                            if (aI != null) {
                                aI.cost = price;
                                aI.sellerID = p.discordID;
                                aI.quantity = 1;
                                p.inventory.RemoveAt(numItem);
                                player.update_player(p);
                                marketItem.insert_marketItem(aI);
                                await Context.Channel.SendMessageAsync(Context.User.Mention + ", added a new market item.",false,aI.toMarketEmbed().Build());
                                return;
                            }
                            marketItem itm = new marketItem(p.inventory[numItem],price,1,p.discordID);
                            p.inventory.RemoveAt(numItem);
                            marketItem.insert_marketItem(itm);
                            player.update_player(p);
                            await Context.Channel.SendMessageAsync(Context.User.Mention + ", added a new market item.",false,itm.toMarketEmbed().Build());
                            return;
                        } else {
                            await Context.Channel.SendMessageAsync(Context.User.Mention + ", the price was invalid. Try `bg!market sell " + numItem + " [Price]`");
                            return;
                        }
                    } else {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", the item ID was invalid. Try `bg!market sell [Inv ID] [Price]`");
                        return;
                    }
                } else if (inputs[0].Equals("buy",StringComparison.InvariantCultureIgnoreCase)) {
                    // inputs[1] = ID to buy
                    if (inputs.Length != 2) {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", try `bg!market buy [Market ID]`");
                    }
                    player p = player.get_player(Context.User.Id);
                    if (p == null) {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", there isn't a player with that ID in the system");
                        return;
                    }
                    if (Int32.TryParse(inputs[1],out int itemToBuy)) {
                        marketItem i = marketItem.get_marketItem(itemToBuy);
                        if (p.gold-i.cost < 0) {
                            await Context.Channel.SendMessageAsync(Context.User.Mention + ", you don't have the gold to afford this purchase.");
                            return;
                        }
                        if(p.discordID == i.sellerID) {
                            await Context.Channel.SendMessageAsync(Context.User.Mention + ", You can't buy your own item. To cancel an item on the market use `bg!market cancel [Market ID]`");
                            return;
                        }
                        p.gold -= i.cost;
                        i.quantity--;
                        if (i.quantity == 0) {
                            marketItem.delete_marketItem(i);
                        }
                        string sellerName = "Market"; /// Set basio name here
                        if (i.sellerID != 548668447719161858) {
                            player seller = player.get_player(i.sellerID);
                            seller.gold += i.cost;
                            sellerName = seller.name;
                            await Context.Guild.GetUser(seller.discordID).SendMessageAsync("Your " + i.name + " has sold on the market and you have earned " + i.cost + " gold value");
                            player.update_player(seller);
                        }
                        // Try for Weapon
                        armorItem aI = i as armorItem;
                        if (aI != null) {
                            p.inventory.Add(aI);
                            await Context.Channel.SendMessageAsync(Context.User.Mention + " you have purchased " + i.name + " from " + sellerName + " for " + i.cost + " gold equivelent.");
                            player.update_player(p);
                            return;
                        }
                        p.inventory.Add(i);
                        await Context.Channel.SendMessageAsync(Context.User.Mention + " you have purchased " + i.name + " from " + sellerName + " for " + i.cost + " gold equivelent.");
                        player.update_player(p);
                    } else {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", you didn't provide a valid item ID to buy");
                    }
                } else if (Int32.TryParse(inputs[0], out int id)) {
                    marketItem item = marketItem.get_marketItem(id);
                    if (item != null) {
                        await Context.Channel.SendMessageAsync("",false, item.toMarketEmbed().Build());
                    } else {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + ", this item ID doesn't exist. Please try again");
                    }
                } else {
                    await Context.Channel.SendMessageAsync("Fail 3");
                }
            }
        }
    }

}