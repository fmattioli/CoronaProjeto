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
using System.Text;

namespace BotAgainstCorona.Dialogs
{
    [Serializable]
    public class Util
    {
        private string Nome { get; set; }
        public List<string> opcaoTresBotoesTitle { get; set; } = new List<string>();
        public async Task Inicio(IDialogContext context)
        {
            try
            {
                DateTime horario = DateTime.Now;
                var mensagem = context.MakeMessage();
                PromptDialog.Text(context, retornoIntentInicio, $"Bom dia, seja bem vindo. Antes de iniciarmos me diga seu nome? :)");
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("");
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        private async Task retornoIntentInicio(IDialogContext context, IAwaitable<string> result)
        {
            Nome = await result;
            await Escala(context, "Escala", Nome);
        }

        public async Task InicioBem(IDialogContext context, LuisResult result)
        {
            try
            {
                await QuickReplyDoisBotoes(context, $"Isto é ótimo! {Nome} Mesmo assim, me sinto na obrigação de perguntar... Você deseja checar os sintomas que o CoronaVirus causa?", "Checagem de sintomas (tosse, febre, dor de garganta)", "Não tenho interesse");
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        public async Task InicioMal(IDialogContext context, string nomeJSON)
        {
            try
            {
                await QuickReply(context, $"{Nome}, me diga o que você está sentindo?");
                await AdaptiveCard(context, nomeJSON);
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }

        }

        public async Task PeriodoSintomas(IDialogContext context, string nomeJSON)
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
        public async Task Escala(IDialogContext context, string nomeJSON, string wordReplace)
        {
            try
            {
                await AdaptiveCard(context, nomeJSON, "{Nome}", wordReplace);
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }

        }
        public async Task Erro(IDialogContext context, string erro)
        {
            try
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro);
            }
            catch (Exception e)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + e.Message.ToString());
            }

        }

        public async Task None(IDialogContext context)
        {
            try
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ainda não tenho a capacidade de entender isso... Aguarde que logo terei!!!");
            }
            catch (Exception e)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + e.Message.ToString());
            }

        }

        public async Task FimInteracao(IDialogContext context)
        {
            try
            {
                opcaoTresBotoesTitle.Add("Em caso de dúvidas, ligue o DiskSaúde(136)");
                opcaoTresBotoesTitle.Add("Dicas oficiais");
                opcaoTresBotoesTitle.Add("Notícias");
                QuickReplyTresBotoes(context, $"Ótimo: {Nome}, em casos de dúvidas: ", opcaoTresBotoesTitle);
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        private async Task AdaptiveCard(IDialogContext context, string nomeJSON, string origemReplace = "", string destinoReplace = "")
        {
            var returnMessage = context.MakeMessage();
            var json = URL($"https://testedofelipe.000webhostapp.com/json/{nomeJSON}.json");
            if (!string.IsNullOrEmpty(destinoReplace))
            {
                json = json.Replace(origemReplace, origemReplace);
            }
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

        private async void QuickReplyTresBotoes(IDialogContext context, string msg, List<string> titlesButtons)
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

        private async Task QuickReplyDoisBotoes(IDialogContext context, string msg, string titleFirstButton, string titleSecondtButton)
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


        private static async Task QuickReply(IDialogContext ctx, string message)
        {
            var reply = ctx.MakeMessage();
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
            reply.Text = message;

            await ctx.PostAsync(reply);
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