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
        [Command("ping")]
        public async Task pong() {
            await Context.Channel.SendMessageAsync("PONG");
        }

        [Command("item")]
        public async Task showItem(int i) {
            item display = item.get_item(i);
            if ( display != null) await Context.Channel.SendMessageAsync("", false, display.toItem());
        }

        [Command("item")]
        public async Task showItem(string name) {
            item display = item.get_item(name);
            if ( display != null) await Context.Channel.SendMessageAsync("", false, display.toItem());
        }

        [Command("add")]
        public async Task addItem(string name, string description, double value, double weight) {
            item i = new item(name, description, value, weight);
            item.insert_item(i);
            await Context.Channel.SendMessageAsync("Item added",false,i.toItem());
        }
    }

}