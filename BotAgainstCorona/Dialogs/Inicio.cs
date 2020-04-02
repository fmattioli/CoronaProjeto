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
    public class Inicio : LuisDialog<object>
    {
        public readonly Util Util = new Util();

        
        [LuisIntent("inicio")]
        public async Task InicioDialog(IDialogContext context, LuisResult result)
        {
            await Util.Inicio(context);
        }
        [LuisIntent("InicioBem")]
        public async Task InicioBem(IDialogContext context, LuisResult result)
        {
            await Util.InicioBem(context, result);
        }

        [LuisIntent("InicioMal")]
        public async Task InicioMal(IDialogContext context, LuisResult result)
        {
            await Util.InicioMal(context, "Sintomas");
        }

        [LuisIntent("CasoImprovavel")]
        public async Task CasoImprovavel(IDialogContext context, LuisResult result)
        {
            await Util.CasoImprovavel(context);
        }

        [LuisIntent("PeriodoSintomas")]
        public async Task PeriodoSintomas(IDialogContext context, LuisResult result)
        {
            await Util.PeriodoSintomas(context, "PeriodoSintomas");
        }
      


    }
}