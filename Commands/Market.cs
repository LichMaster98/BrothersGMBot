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
                    } else { // Regular User adding to store 
                        await Context.Channel.SendMessageAsync("Fail 2");
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
                            embed.WithFooter("Page " + value + " out of " + (market.Count / 25 + 1));
                            for (int i = 0; i < 25 && i+start < market.Count; i++) {
                                embed.AddField(market[i+start].name,"Cost: " + market[i+start].cost + " GP | Quantity: " + market[i+start].quantity + " | ID: " + market[i+start].ID);
                            }
                        }
                        await Context.Channel.SendMessageAsync("",false,embed.Build());
                        return;
                    }
                    // Default Output
                    embed.WithFooter("Page 1 out of " + (market.Count / 25 + 1));
                    for (int i = 0; i < 25 && i < market.Count; i++) {
                        embed.AddField(market[i].name,"Cost: " + market[i].cost + " GP | Quantity: " + market[i].quantity+ " | ID: " + market[i].ID);
                    }
                    await Context.Channel.SendMessageAsync("",false,embed.Build());
                } else if (Int32.TryParse(inputs[0], out int id)) {
                    marketItem item = marketItem.get_marketItem(id);
                    if (item != null) {

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