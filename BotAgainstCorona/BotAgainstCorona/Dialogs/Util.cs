using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;

using System.IO;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using AdaptiveCards;

namespace BotAgainstCorona.Dialogs
{
    [Serializable]
    public class Util
    {
        public async Task Inicio(IDialogContext context, LuisResult result)
        {
            var returnMessage = context.MakeMessage();
            var json = URL("http://52.191.189.59/CoronaImages/Botoes/" + "Botao" + ".json");
            var results = AdaptiveCard.FromJson(json);
            var cards = results.Card;

            returnMessage.Attachments.Add(new Attachment()
            {
                Content = cards,
                ContentType = AdaptiveCard.ContentType,
                Name = "Card"
            });

            await context.PostAsync(returnMessage);

        }

        private async Task retornoIntentInicio(IDialogContext context, IAwaitable<string> result)
        {
            var horario = DateTime.Now;
            var mensagem = context.MakeMessage();
            mensagem = context.MakeMessage();
           


            await context.PostAsync($"Este é o bot de combate ao Corona Virus!!! Subiu porra!");
            QuickReply(context, "Cardápio", true);

        }

        private async void QuickReply(IDialogContext ctx, string valorResposta, bool unicoBotao, string msgLuis = "")
        {

            var reply = ctx.MakeMessage();
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;

            if (string.IsNullOrEmpty(msgLuis))
            {
                reply.SuggestedActions = new SuggestedActions()
                {
                    Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = valorResposta, Type=ActionTypes.ImBack, Value= valorResposta },
                    }
                };
            }
            else
            {
                reply.SuggestedActions = new SuggestedActions()
                {
                    Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = valorResposta, Type=ActionTypes.ImBack, Value= msgLuis },
                    }
                };
            }

            await ctx.PostAsync(reply);
        }

        public string URL(String url)
        {
            var json = new WebClient().DownloadString(url);
            return json;
        }

    }
}