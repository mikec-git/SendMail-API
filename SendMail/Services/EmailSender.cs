﻿using MailKit.Net.Smtp;
using MimeKit;
using SendMail.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SendMail.Services
{
    /// <summary>
    /// Email sending service
    /// </summary>
    public class EmailSender : IEmailSender
    {        
        public async Task<EmailSentResponse> SendEmailAsync(EmailToSendModel email)
        {
            // Create a MIME message object for the email 
            var message = ConstructEmailToSend(email);
           
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(
                        Startup.Configuration["EmailConfiguration:SmtpServer"], 
                        int.Parse(Startup.Configuration["EmailConfiguration:SmtpPort"]), 
                        true
                    ).ConfigureAwait(false);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(
                        Environment.GetEnvironmentVariable("EMAIL_ADDRESS"), 
                        Environment.GetEnvironmentVariable("EMAIL_PASSWORD")
                    ).ConfigureAwait(false);

                    // Send message
                    await client.SendAsync(message).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // If email sending failed...
                    return new EmailSentResponse()
                    {
                        ErrorMessage = new List<string>() { ex.Message }
                    };
                }
            }

            // If email was sent successfully...
            return new EmailSentResponse()
            {
                ErrorMessage = null,
                Email = email
            };
        }

        public MimeMessage ConstructEmailToSend(EmailToSendModel email)
        {
            var message = new MimeMessage();

            //From
            message.From.Add(new MailboxAddress(email.Sender.Name, email.Sender.Email));

            // To
            message.To.Add(new MailboxAddress(email.Receiver.Name, email.Receiver.Email));

            // Subject
            message.Subject = email.Email.Subject;

            // Body (text/Html)
            message.Body = new BodyBuilder()
            {
                TextBody = $@"{email.Email.MessageText} 
                            Email was sent at: 
                            {email.Email.DateSent}",
                HtmlBody = email.IsHtml ? email.Email.MessageHtml : null
            }.ToMessageBody();

            return message;
        }
    }
}
