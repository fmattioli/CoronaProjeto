using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using BotAgainstCorona.Classes;
using BotAgainstCorona.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using static BotAgainstCorona.Classes.RetornoFormulario;

namespace BotAgainstCorona
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        ConversationControle conversation = new ConversationControle();
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            try
            {
                if (activity.Type == ActivityTypes.Message)
                {
                    activity.Type = ActivityTypes.Message;
                    var connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                    Activity isTyping = activity.CreateReply();
                    Thread.Sleep(2000);
                    isTyping.Type = ActivityTypes.Typing;
                    await connector.Conversations.ReplyToActivityAsync(isTyping);
                    if (activity.Value != null)
                    {
                        string nomeForm = string.Empty;
                        activity.Value = activity.Value.ToString().Replace("{{", "{{").Replace("}}", "}");
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        var retornoJSON = jss.Deserialize<dynamic>(activity.Value.ToString());
                        foreach (var item in retornoJSON)
                        {
                            nomeForm = item.Key;
                            break; //necessário passar só uma vez
                        }
                        string retornoValidacao = conversation.ValidarDadosFormulario($"form-{nomeForm}", activity.Value.ToString());
                        activity.Text = retornoValidacao;
                    }
                    await Conversation.SendAsync(activity, () => new Dialogs.Inicio());
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                
                //create response activity
                Activity msg = activity.CreateReply(error);
                

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