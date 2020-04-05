
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
        /// 
        public bool erroFormulario { get; set; }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            List<string> stri = new List<string>();
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
                        if (nomeForm != string.Empty)
                        {
                            activity.InputHint = InputHints.IgnoringInput;
                            activity.Text = conversation.ValidarDadosFormulario($"form-{nomeForm}", activity.Value.ToString());
                            if (activity.Text.Contains("preenchido de forma incorreta"))
                            {
                                erroFormulario = true;
                            }
                        }
                        else
                        {
                            throw new Exception("Não houve retorno de dados do formulário");
                        }
                    }

                    await Conversation.SendAsync(activity, () => new Dialogs.Inicio("", erroFormulario == true ? activity.Text : ""));
                }

                else if (activity.Type == ActivityTypes.ConversationUpdate)
                {
                    foreach (var member in activity.MembersAdded)
                    {
                        stri.Add(member.Name);
                        if (member.Name.Trim().Contains("BotAgainstCorona") || member.Id == "BotAgainstCorona")
                        {
                            activity.Text = "Olá";
                            await Conversation.SendAsync(activity, () => new Dialogs.Inicio(activity.Text));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                activity.Text = "ocorreu o seguinte erro" + activity.MembersAdded[0].Name;
                await Conversation.SendAsync(activity, () => new Dialogs.Inicio(activity.Text));

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