using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotAgainstCorona.Dialogs
{
    [Serializable]
    public class StartDialog : IDialog<object>
    {

        public readonly Util Util = new Util();

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(IniciarConversa);

            return Task.CompletedTask;
        }

        private async Task IniciarConversa(IDialogContext context, IAwaitable<object> result)
        {
            await Util.Inicio(context);
        }
    }

}