
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using Bot.Dominio;
using System.Text;

namespace BotAgainstCorona
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 
        public bool erroFormulario { get; set; }
        public Dictionary<string, string> json { get; set; } = new Dictionary<string, string>();
        ConversationControle conversasionControle = new ConversationControle();

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            try
            {
                if (activity.Type == ActivityTypes.Message)
                {
                    activity.Type = ActivityTypes.Message;
                    var connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                    Activity isTyping = activity.CreateReply();
                    Thread.Sleep(500);
                    isTyping.Type = ActivityTypes.Typing;
                    await connector.Conversations.ReplyToActivityAsync(isTyping);
                    if (activity.Value != null)
                    {
                        activity.Text = conversasionControle.RetornarProximaIntent(activity.Value.ToString());
                        
                        if(activity.Value.ToString() == "")
                        { 
                            throw new Exception("Não houve retorno de dados do formulário");
                        }
                    }

                    await Conversation.SendAsync(activity, () => new Dialogs.InicioDialog());
                }

                else if (activity.Type == ActivityTypes.ConversationUpdate)
                {
                    foreach (var member in activity.MembersAdded)
                    {
                        if (member.Name.Trim().Contains("JuntosComVoce") || member.Name == "Bot")
                        {
                            activity.Text = "Olá";
                            await Conversation.SendAsync(activity, () => new Dialogs.InicioDialog());
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                activity.Text = "ocorreu o seguinte erro" + ex.Message.ToString() +"Tack " + ex.StackTrace; 
                await Conversation.SendAsync(activity, () => new Dialogs.InicioDialog());

            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}