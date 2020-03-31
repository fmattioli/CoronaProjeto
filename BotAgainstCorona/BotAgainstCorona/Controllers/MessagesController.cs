using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace BotAgainstCorona
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
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
                    isTyping.Type = ActivityTypes.Typing;
                    await connector.Conversations.ReplyToActivityAsync(isTyping);
                    if (activity.Value != null)
                    {
                        activity.Text = JsonConvert.SerializeObject(activity.Value);
                        object JsonDe = JsonConvert.DeserializeObject(activity.Text);
                    }
                    await Conversation.SendAsync(activity, () => new Dialogs.Inicio());
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;

                //for debugging in bot emulator, also output stack trace
                if (activity.ChannelId == "emulator"
                     || activity.ChannelId == "facebook" //uncomment this line to get stack traces on facebook too
                   )
                {
                    error += ex.Message + ex.StackTrace;
                }

                //create response activity
                Activity hhhh = activity.CreateReply(error);

                //post back to user
                ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(hhhh);

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