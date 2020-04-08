using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace BotAgainstCorona.Dialogs
{
    [Serializable]
    [LuisModel("4200aec6-fbaa-4c59-bc55-6b98bbfd3e39", "454872db14ad488183f35cff776f91df", domain: "brazilsouth.api.cognitive.microsoft.com")]

    public class InicioDialog : LuisDialog<object>
    {
        public readonly UtilDialog Util = new UtilDialog();
        private string _erro { get; set; }
        private Dictionary<string, string> dictonary = new Dictionary<string, string>();
        private bool _cardSintomas { get; set; }
        private bool _erroCardOutrosSintomas { get; set; }

        [LuisIntent("inicio")]
        public async Task Inicio(IDialogContext context, LuisResult result)
        {
            await Util.Inicio(context);
        }

        [LuisIntent("Sexo")]
        public async Task Sexo(IDialogContext context, LuisResult result)
        {
            AppendResultsJson(context);
            await Util.Sexo(context);
        }

        [LuisIntent("Doencas")]
        public async Task Doencas(IDialogContext context, LuisResult result)
        {
            if (!result.Query.Contains("formulário de doencas que o usuário já teve está preenchido de forma incorreta"))
            {
                await Util.Doencas(context, "Doencas", false);
                AppendResultsJson(context);
            }
            else
            {
                await Util.Doencas(context, "Doencas", true);
            }
        }

        [LuisIntent("Sintomas")]
        public async Task Sintomas(IDialogContext context, LuisResult result)
        {
            if (!_cardSintomas || result.Query.Contains("formulário de sintomas preenchido de forma incorreta"))
            {
                bool erroFormulario = false;
                if(result.Query.Contains("formulário de sintomas preenchido de forma incorreta"))
                {
                    erroFormulario = true;
                }

                _cardSintomas = false;
                AppendResultsJson(context);
                await Util.Sintomas(context, "Sintomas", _cardSintomas, erroFormulario);
                _cardSintomas = true; //Existem dois formularios de sintomas, deste modo é necessário setar a variavel pra true pra exibir o outro formulario de sintomas
            }
            else
            {
                await Util.Sintomas(context, "OutrosSintomas", _cardSintomas);
            }
        }

        [LuisIntent("Respiracao")]
        public async Task Respiracao(IDialogContext context, LuisResult result)
        {
            await Util.Respiracao(context);
        }

        [LuisIntent("Erro")]
        public async Task Erro(IDialogContext context, LuisResult result)
        {
            await Util.Erro(context, _erro);
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await Util.None(context);
        }

        private void AppendResultsJson(IDialogContext context)
        {
            Activity activity = (Activity)context.Activity;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var retornoJSON = jss.Deserialize<dynamic>(activity.Value.ToString().Replace("{{", "{{").Replace("}}", "}"));
            //foreach (var item in retornoJSON)
            //{
            //    dictonary.Add(item.Key, item.Value);
            //}
        }
    }
}