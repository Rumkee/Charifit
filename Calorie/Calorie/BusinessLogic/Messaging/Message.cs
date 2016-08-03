using System;
using System.Configuration;
using Calorie.Models;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Calorie.BusinessLogic
{

    public class Messaging
    {                     

        public static bool Add(Message.LevelEnum Level, string MessageBody, Message.TypeEnum Type, ApplicationUser User)
        {


            if (User == null) {return false;}


            var NewMsg = new Message
            {
                Level = Level,
                MessageBody = MessageBody,
                Type = Type,
                Status = Message.StatusEnum.Unread
            };


            User.Messages.Add(NewMsg);
            
            //Type.HasFlag(Message.TypeEnum.TemporaryAlert)
            
            return true;


        }

        public static bool AddHTMLMessage(string HTMLMessage,Type _Type)
        {
            throw new NotImplementedException();
        }

        public static async Task<bool> SendEmail(string Subject, string PlainText,string HTMLText, string To)
        {
            
            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(To);
            myMessage.From = new MailAddress("mail@charifit.com", "ChariFit");
            myMessage.Subject = Subject;
            myMessage.Text = PlainText ;
            myMessage.Html = HTMLText;

            var transportWeb = new SendGrid.Web(ConfigurationManager.AppSettings["SendGridAPIKey"]);
            await transportWeb.DeliverAsync(myMessage);
            
            return true;
        }

    }

  
}