using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using AdaptiveCards;
using System.Net;
using System.Text;

namespace BotAgainstCorona.Utilitarios.Cards
{
    [Serializable]
    public class BotAdaptiveCards
    {
        public async Task Doencas(IDialogContext context, string nomeJSON, string wordReplace)
        {
            try
            {
                await AdaptiveCard(context, nomeJSON);
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        public async Task AdaptiveCard(IDialogContext context, string nomeJSON)
        {
            var returnMessage = context.MakeMessage();
            var json = URL($"https://coronaagainscorona.000webhostapp.com/Json/{nomeJSON}.json");
            var results = AdaptiveCards.AdaptiveCard.FromJson(json);
            var card = results.Card;

            returnMessage.Attachments.Add(new Attachment()
            {
                Content = card,
                ContentType = AdaptiveCards.AdaptiveCard.ContentType,
                Name = "Card"
            });

            await context.PostAsync(returnMessage);
        }

        public string URL(String url)
        {
            WebClient Client = new WebClient();
            Client.Encoding = Encoding.UTF8;
            var json = Client.DownloadString(url);
            return json;
        }


    }
}