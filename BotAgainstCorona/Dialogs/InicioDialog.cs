using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Bot.Dados;
using Bot.Data;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;


namespace BotAgainstCorona.Dialogs
{
    [Serializable]
    [LuisModel("9e0cadd0-e44d-4c83-b40f-824a2266c6ee", "fdfa4dfc5a714e7aab1f42ad817acf4a")]

    public class InicioDialog : LuisDialog<object>
    {
        public readonly UtilDialog Util = new UtilDialog();
        ApiGoogleSheets dados = new ApiGoogleSheets();

        private string _erro { get; set; }
        private Dictionary<string, dynamic> dictonary = new Dictionary<string, dynamic>();
        private bool _cardSintomas { get; set; }
        private bool _cardResultadoMal = true;
        private bool _erroCardOutrosSintomas { get; set; }
        public string NomeCard { get; set; }
        private string _JSON { get; set; } 

        [LuisIntent("inicio")]
        public async Task Inicio(IDialogContext context, LuisResult result)
        {
            NomeCard = "Idade";
            dictonary.Clear();
            await Util.Inicio(context);
        }

        [LuisIntent("Sexo")]
        public async Task Sexo(IDialogContext context, LuisResult result)
        {
            string json = string.Empty;
            AppendResultsJson(context, out json);
            _JSON += json + ", \n";
            await Util.Sexo(context);
 
        }

        [LuisIntent("Doencas")]
        public async Task Doencas(IDialogContext context, LuisResult result)
        {
            if (!result.Query.Contains("formulário de doencas que o usuário já teve está preenchido de forma incorreta"))
            {
                string json = string.Empty;
                AppendResultsJson(context, out json);
                _JSON += json + "\n";
                dados.CreateEntry(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), _JSON, Planilhas.DadosPessoais);
                await Util.Doencas(context, "Doencas", false);
            }
            else
            {
                string json = string.Empty;
                AppendResultsJson(context, out json);
                await Util.Doencas(context, "Doencas", true);
            }
        }

        [LuisIntent("Sintomas")]
        public async Task Sintomas(IDialogContext context, LuisResult result)
        {
            if (!_cardSintomas || result.Query.Contains("formulário de sintomas preenchido de forma incorreta"))
            {
                NomeCard = "Doencas";
                bool erroFormulario = false;
                if (result.Query.Contains("formulário de sintomas preenchido de forma incorreta"))
                {
                    erroFormulario = true;
                }

                string json = string.Empty;
                AppendResultsJson(context, out json, true);
                _JSON = json;
                dados.CreateEntry(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), _JSON, Planilhas.DadosDoencas);
                if (!erroFormulario)
                   // dados.InserirDadosConversa(NomeCard, json);

                _cardSintomas = false;
                await Util.Sintomas(context, "Sintomas", _cardSintomas, erroFormulario);
                _cardSintomas = true; //Existem dois formularios de sintomas, deste modo é necessário setar a variavel pra true pra exibir o outro formulario de sintomas
            }
            else
            {
                string json = string.Empty;
                AppendResultsJson(context, out json);
                _JSON = json;
                await Util.Sintomas(context, "OutrosSintomas", _cardSintomas);
            }
        }

        [LuisIntent("Respiracao")]
        public async Task Respiracao(IDialogContext context, LuisResult result)
        {
            string json = string.Empty;
            AppendResultsJson(context, out json);
            _JSON += json;
            
            await Util.Respiracao(context);
        }

        [LuisIntent("DiferentesSintomas")]
        public async Task DiferentesSintomas(IDialogContext context, LuisResult result)
        {
            string json = string.Empty;
            AppendResultsJson(context, out json);
            _JSON += json;
            await Util.DiferentesSintomas(context);
        }

        [LuisIntent("PeriodoSintomas")]
        public async Task PeriodoSintomas(IDialogContext context, LuisResult result)
        {
            string json = string.Empty;
            AppendResultsJson(context, out json, true);
            _JSON += json;
            await Util.PeriodoSintomas(context);
        }
        [LuisIntent("Substancias")]
        public async Task Substancias(IDialogContext context, LuisResult result)
        {
            string jsonSintomas = string.Empty;
            AppendResultsJson(context, out jsonSintomas);
            dados.CreateEntry(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), _JSON, Planilhas.DadosSintomas, jsonSintomas);
            await Util.Substancias(context);
        }

        [LuisIntent("Resultado")]
        public async Task Resultado(IDialogContext context, LuisResult result)
        {
            string jsonSintomas = string.Empty;
            AppendResultsJson(context, out jsonSintomas);
            //TODO: Salvar dados das substnacias
            //dados.CreateEntry(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), _JSON, Planilhas.DadosSintomas);
            var Sintomas = dictonary["Sintomas"].Count;
            var OutrosSintomas = dictonary["OutrosSintomas"].Count;
            var Respiracao = dictonary["Respiracao"].Count;
            var DiferentesSintomas = dictonary["DiferentesSintomas"].Count;
            if (Sintomas >= 2)
            {
                if (OutrosSintomas >= 1)
                {
                    if (Respiracao >= 1 && DiferentesSintomas >= 1)
                    {
                        await Util.ResultadoRuim(context, _cardResultadoMal);
                        _cardResultadoMal = false;
                    }
                }
            }
            await Util.ResultadoBom(context);
        }

        [LuisIntent("Erro")]
        public async Task Erro(IDialogContext context, LuisResult result)
        {
            await Util.Erro(context, result.Query);
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await Util.None(context);
        }

        private void AppendResultsJson(IDialogContext context, out string JSONRetorno, bool necessarioValidarUltimaPosicao = false)
        {
            JSONRetorno = "";
            Activity activity = (Activity)context.Activity;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var retornoJSON = jss.Deserialize<dynamic>(activity.Value.ToString().Replace("{{", "{{").Replace("}}", "}"));
            if (retornoJSON.Count > 1)
            {
                Dictionary<string, dynamic> dicAlter = new Dictionary<string, dynamic>();
                string[] chaves = new string[retornoJSON.Count];
                string[] valores = new string[retornoJSON.Count];
                retornoJSON.Keys.CopyTo(chaves, 0);
                retornoJSON.Values.CopyTo(valores, 0);
                for (int i = 1; i < retornoJSON.Count; i++)
                {
                    if (Boolean.Parse(valores[i]) == true && chaves[i] != "Nenhum" && chaves[i] != "Nenhuma")
                    {
                        dicAlter.Add(chaves[i], valores[i]);
                    }
                }

                var index = 0;
                var count = dicAlter.Count(); //ou Count() - você sabe
                foreach (var item in dicAlter)
                {
                    if (necessarioValidarUltimaPosicao)
                    {
                        if (!(++index == count))
                        {
                            JSONRetorno += @"""" + item.Key + @"""" + ": " + @"""" + item.Value + @"""" + ",\n";
                        }
                        else
                        {
                            JSONRetorno += @"""" + item.Key + @"""" + ": " + @"""" + item.Value + @"""" + "\n";
                        }
                    }
                    else
                    {
                        JSONRetorno += @"""" + item.Key + @"""" + ": " + @"""" + item.Value + @"""" + ",\n";
                    }
                }
            }
            else
            {
                foreach (var item in retornoJSON)
                {
                    if (!dictonary.ContainsKey(item.Key))
                    {
                        dictonary.Add(item.Key, item.Value);
                       
                    }
                }
              
                foreach (var item in dictonary)
                {
                    JSONRetorno = @"""" + item.Key + @""""+ ": " + @""""+ item.Value + @"""";
                }

            }

        }
    }
}