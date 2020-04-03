using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BotAgainstCorona.Utilitarios.QuickReplies
{
    [Serializable]
    public class QuickReply
    {
        public async Task QuickReplyMessage(IDialogContext ctx, string message)
        {
            var reply = ctx.MakeMessage();
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
            reply.Text = message;
         

            await ctx.PostAsync(reply);
        }

        public async Task QuickReplyDoisBotoes(IDialogContext context, string msg, string titleFirstButton, string titleSecondtButton)
        {
            var reply = context.MakeMessage();
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
            reply.Text = msg;
       


            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = titleFirstButton, Type=ActionTypes.ImBack, Value=titleFirstButton },
                        new CardAction(){ Title = titleSecondtButton, Type=ActionTypes.ImBack, Value=titleSecondtButton },
                    }
            };

            await context.PostAsync(reply);
        }
        public async void QuickReplyTresBotoes(IDialogContext context, string msg, List<string> titlesButtons)
        {
            var reply = context.MakeMessage();
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
            reply.Text = msg;
          

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = titlesButtons[0], Type=ActionTypes.OpenUrl, Value="https://globoesporte.globo.com/" },
                        new CardAction(){ Title = titlesButtons[1], Type=ActionTypes.OpenUrl, Value="https://globoesporte.globo.com/" },
                        new CardAction(){ Title = titlesButtons[2], Type=ActionTypes.OpenUrl, Value="https://globoesporte.globo.com/"},
                    }
            };

            await context.PostAsync(reply);
        }
    }
}