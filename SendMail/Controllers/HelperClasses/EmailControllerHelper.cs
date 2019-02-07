using SendMail.Models;
using System;

namespace SendMail.Controllers.HelperMethods
{
    public static class EmailControllerHelper
    {
        public static EmailToSendModel CreateEmailToSendDto(string senderName, string senderEmailAddress, string message)
        {
            return new EmailToSendModel()
            {
                Receiver = new EmailAddressModel()
                {
                    Name = Startup.Configuration["EmailConfiguration:ToName"],
                    Email = Startup.Configuration["EmailConfiguration:ToEmail"]
                },
                Sender = new EmailAddressModel()
                {
                    Name = senderName ?? "Stranger",
                    Email = senderEmailAddress
                },
                Email = new EmailMessageModel()
                {
                    Subject = Startup.Configuration["EmailConfiguration:Subject"] + senderName,
                    MessageText = $"You've got a message from: \n" +
                    $"{senderName}\n" +
                    $"Reply to: \n" +
                    $"{senderEmailAddress}\n" +
                    $"The email is shown below: \n" +
                    $"{message}",
                    DateSent = DateTime.Now
                }
            };
        }
    }
}
