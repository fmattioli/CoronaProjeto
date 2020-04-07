using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;
using BotAgainstCorona.Utilitarios.Cards;
using BotAgainstCorona.Utilitarios.QuickReplies;

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
    public class UtilDialog
    {
        private string Nome { get; set; }
        public List<string> opcaoTresBotoesTitle { get; set; } = new List<string>();
        BotAdaptiveCards cards = new BotAdaptiveCards();
        QuickReply reply = new QuickReply();

        public async Task Inicio(IDialogContext context)
        {
            try
            {
                await reply.QuickReplyMessage(context, "Olá, Vamos ver como você está se sentindo hoje?");
                await reply.QuickReplyMessage(context, "Antes de falarmos da sua saúde precisamos fazer algumas perguntas básicas");
                await MontarAdaptiveCard(context, "Inicio");
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }


        public async Task MontarAdaptiveCard(IDialogContext context, string nomeJSON)
        {
            try
            {
                await cards.AdaptiveCard(context, nomeJSON);
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        public async Task Sexo(IDialogContext context)
        {
            try
            {
                await reply.QuickReplyMessage(context, "Certo... Vamos prosseguir!");
                await MontarAdaptiveCard(context, "Sexo");
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        
        public async Task InicioBem(IDialogContext context, LuisResult result)
        {
            try
            {
                await reply.QuickReplyDoisBotoes(context, $"Isto é ótimo! {Nome} Mesmo assim, me sinto na obrigação de perguntar... Você deseja checar os sintomas que o CoronaVirus causa?", "Checagem de sintomas (tosse, febre, dor de garganta)", "Não tenho interesse");
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
                await reply.QuickReplyMessage(context, $"{Nome}, me diga o que você está sentindo?");
                await cards.AdaptiveCard(context, nomeJSON);
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }

        }

        public async Task Doencas(IDialogContext context, string nomeJSON, bool retorno)
        {
            try
            {
                if (!retorno)
                {
                    await reply.QuickReplyMessage(context, $"Certo, agora selecione abaixo as doenças que você possui");
                    await reply.QuickReplyMessage(context, $"Ahhh e lembre-se, caso não tenha nenhuma basta clicar em: Nenhuma das anteriores");
                    await cards.AdaptiveCard(context, nomeJSON);
                }
                else
                {
                    await reply.QuickReplyMessage(context, $"Opa! Lembre-se de preencher ao menos uma das opções, selecione abaixo as doenças que você possui");
                    await reply.QuickReplyMessage(context, $"Ahhh e lembre-se, caso não tenha nenhuma basta clicar em: Nenhuma das anteriores");
                    await cards.AdaptiveCard(context, nomeJSON);
                }
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }

        }

        public async Task Sintomas(IDialogContext context, string nomeJSON, bool cardSintomas)
        {
            try
            {
                if (cardSintomas)
                {
                    await reply.QuickReplyMessage(context, $"Anotei sua resposta! Agora preciso que você me diga como você está se sentindo...");
                    await reply.QuickReplyMessage(context, $"Selecione abaixo os sintomas que você está tendo. Ahhh e lembre-se, caso não tenha nenhuma basta clicar em: Nenhuma das anteriores");
                    await cards.AdaptiveCard(context, nomeJSON);
                }
                else
                {

                    await reply.QuickReplyMessage(context, $"Anotei sua resposta! Agora a minha pergunta continua sendo sobre seus sintomas, mas veja que os sintomas mudaram...");
                    await reply.QuickReplyMessage(context, $"Selecione abaixo os sintomas que você está tendo. Ahhh e lembre-se, caso não tenha nenhuma basta clicar em: Nenhuma das anteriores");
                    await cards.AdaptiveCard(context, nomeJSON);
                }
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
                await cards.AdaptiveCard(context, nomeJSON);
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        private async Task retornoIntentInformacoesPessoaisSexo(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string WelcomeMsg = "Qual é a sua idade?";

                var PromptOptions = new string[] { "Menor que 18 anos", "18 a 39 anos", "40 a 59 anos", "Mais que 60 anos" };
                PromptDialog.Choice(
                context,
                retornoInformacoesPessoaisIdade,
                PromptOptions,
                WelcomeMsg, promptStyle: PromptStyle.Auto
                );
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }


        private async Task retornoInformacoesPessoaisIdade(IDialogContext context, IAwaitable<string> result)
        {
            Nome = await result;
            //await Doencas(context, "Doencas", Nome);
        }

        public async Task InformacoesPessoa(IDialogContext context)
        {
            try
            {
                string WelcomeMsg = "Qual é o seu sexo?";

                var PromptOptions = new string[] { "Masculino", "Feminino" };
                PromptDialog.Choice(
                context,
                retornoIntentInformacoesPessoaisSexo,
                PromptOptions,
                WelcomeMsg, promptStyle: PromptStyle.Auto
                );
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }

        public async Task Sinais(IDialogContext context, string nomeJSON)
        {
            try
            {
                await cards.AdaptiveCard(context, nomeJSON);
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }
        public async Task CoronaVirus(IDialogContext context)
        {
            try
            {
                await reply.QuickReplyMessage(context, "Nossa! pela a minha inteligência os seus sintomas são perigosos!!! Sigam as instruções abaixo");
                await reply.QuickReplyMessage(context, "Ligue para o SAMU (192) ou caso prefira ligue para o atendimento pré-cliníco: 0800 591 889");
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
                await reply.QuickReplyMessage(context, "Opa opa opa. Tive uns problemas internos. Vamos voltar pro inicio");
                await cards.AdaptiveCard(context, "Inicio");
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
                reply.QuickReplyTresBotoes(context, $"Ótimo: {Nome}, em casos de dúvidas: ", opcaoTresBotoesTitle);
                
            }
            catch (Exception erro)
            {
                var mensagem = context.MakeMessage();
                mensagem.Type = ActivityTypes.Typing;
                await context.PostAsync("Ocorreu o seguinte erro: " + erro.Message.ToString());
            }
        }
        
    }
}