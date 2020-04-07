using System;
using System.Threading.Tasks;
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
        private string _retornoFormulario { get; set; }
        
        
        [LuisIntent("inicio")]
        public async Task Inicio(IDialogContext context, LuisResult result)
        {
            await Util.Inicio(context);
           
        }

        [LuisIntent("Sexo")]
        public async Task Sexo(IDialogContext context, LuisResult result)
        {
            await Util.Sexo(context);
        }


        [LuisIntent("Doencas")]
        public async Task Doencas(IDialogContext context, LuisResult result)
        {
            await Util.Doencas(context, "Doencas");
        }

        [LuisIntent("VerificarSintomas")]
        public async Task InicioMal(IDialogContext context, LuisResult result)
        {
            await Util.InicioMal(context, "Sintomas");
        }

        [LuisIntent("DesejaChecarSintomas")]
        public async Task InicioBem(IDialogContext context, LuisResult result)
        {
            await Util.InicioBem(context, result);
        }

        [LuisIntent("PeriodoSintomas")]
        public async Task PeriodoSintomas(IDialogContext context, LuisResult result)
        {
            await Util.PeriodoSintomas(context, "PeriodoSintomas");
        }

        [LuisIntent("FimInteracao")]
        public async Task CasoImprovavel(IDialogContext context, LuisResult result)
        {
            await Util.FimInteracao(context);
        }

        [LuisIntent("InformacoesPessoa")]
        public async Task InformacoesPessoa(IDialogContext context, LuisResult result)
        {
            await Util.InformacoesPessoa(context);
        }

        [LuisIntent("Sinais")]
        public async Task SintomasCronicos(IDialogContext context, LuisResult result)
        {
            await Util.Sinais(context, "Sinais");
        }

        [LuisIntent("CoronaVirus")]
        public async Task CoronaVirus(IDialogContext context, LuisResult result)
        {
            await Util.CoronaVirus(context);
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
    }
}