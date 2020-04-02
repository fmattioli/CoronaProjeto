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
        private string Nome { get; set; }
        public List<string> opcaoTresBotoesTitle { get; set; } = new List<string>();
        public async Task Inicio(IDialogContext context)
        {
            try
            {
                DateTime horario = DateTime.Now;
                var mensagem = context.MakeMessage();
                if (horario.Hour > 6 && horario.Hour < 12)
                {
                    PromptDialog.Text(context, retornoIntentInicio, "Bom dia, seja bem vindo. Antes de iniciarmos me diga seu nome? :) ");
                }
                else if (horario.Hour >= 12 && horario.Hour < 18)
                {
                    PromptDialog.Text(context, retornoIntentInicio, "Boa tarde, seja bem vindo. Antes de iniciarmos me diga seu nome? :) ");
                }
                else if (horario.Hour >= 19 && horario.Hour < 23 || horario.Hour >= 0)
                {
                    PromptDialog.Text(context, retornoIntentInicio, "Boa noite, seja bem vindo. Antes de iniciarmos me diga seu nome? :) ");
                }
                mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("");
            }
            catch(Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        private async Task retornoIntentInicio(IDialogContext context, IAwaitable<string> result)
        {
            var horario = DateTime.Now;
            var mensagem = context.MakeMessage();
            mensagem = context.MakeMessage();
            Nome = await result;
            //QuickReply(context, "Mal", true);
            QuickReplyDoisBotoes(context, $"{Nome}, Como está a sua saúde no momento?", "Bem", "Mal");
        }
        
        public async Task InicioBem(IDialogContext context, LuisResult result)
        {
            try
            {
                QuickReplyDoisBotoes(context, $"Isto é ótimo, {Nome}! Obrigado por contribuir", "Dicas Oficias", "Notícias");
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
        
        public async Task CasoImprovavel(IDialogContext context)
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

        private async Task AdaptiveCard(IDialogContext context, string nomeJSON)
        {
            var returnMessage = context.MakeMessage();
            var json = URL($"https://testedofelipe.000webhostapp.com/json/{nomeJSON}.json");
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

        private async void QuickReplyDoisBotoes(IDialogContext context, string msg, string titleFirstButton, string titleSecondtButton)
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
        
        public Attachment Botoes(string valorsim, string valornao, string pergunta)
        {
            var Botao = new HeroCard()
            {
                Text = pergunta,
                Title = "Olá",
                Buttons = new List<CardAction>
                {
                    new CardAction(){ Title = "Sim", Type=ActionTypes.ImBack, Value = valorsim},
                    new CardAction(){ Title = "Não", Type=ActionTypes.ImBack, Value = valornao}
                }
            };
            return Botao.ToAttachment();

        }

        public string URL(String url)
        {
            var json = new WebClient().DownloadString(url);
            return json;
        }

    }
}