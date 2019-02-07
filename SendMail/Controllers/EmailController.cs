﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SendMail.Controllers.HelperMethods;
using SendMail.Entities;
using SendMail.Models;
using SendMail.Services;
using System;
using System.Threading.Tasks;

namespace SendMail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateSender _emailTemplateSender;
        private readonly IEmailRepository _emailRepository;

        public EmailController(
            IEmailSender emailSender,
            IEmailTemplateSender emailTemplateSender,
            IMapper mapper,
            IEmailRepository emailRepository
        )
        {
            _mapper = mapper;
            _emailSender = emailSender;
            _emailTemplateSender = emailTemplateSender;
            _emailRepository = emailRepository;
        }

        /// <summary>
        /// Route for sending an email
        /// </summary>
        /// <param name="emailFromForm">This is the email content received from the clientside</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendEmailAsync(EmailFromContactFormModel emailFromForm)
        {
            // Return bad request if no email sent
            if (emailFromForm == null)
            {
                return BadRequest();
            }
            
            // Add new model error if email and name are the same
            if (emailFromForm.Name == emailFromForm.Email)
            {
                ModelState.AddModelError("FromEmail", "Your name should be different from your email");
            }

            // Return bad request if the model state failed
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create new DTO for sending the email
            var email = EmailControllerHelper.CreateEmailToSendDto(
                emailFromForm.Name, 
                emailFromForm.Email, 
                emailFromForm.Message);

            // Send the email using the general template and get the response
            var response = await _emailTemplateSender.SendGeneralEmailAsync(
                email,
                $"You've got a message from:<br/>{emailFromForm.Name}",
                $"Reply to: {emailFromForm.Email}",
                emailFromForm.Message
            );

            // If the email was not sent...
            if(!response.Successful)
            {
                return StatusCode(500, "A problem occurred while trying to send the email.");
            }

            // If the email was sent successfully...
            try
            {
                // Add Html content to email
                email.Email.MessageHtml = response.Email.Email.MessageHtml;

                // Map email body to email entity
                var deliveredEmailEntity = _mapper.Map<EmailMessageModel, DeliveredEmail>(email.Email);
                
                // Populate entity Sender field 
                PopulateDeliveredEmailAddress(email.Sender,
                    (senderEmailAddress) => { deliveredEmailEntity.Sender = senderEmailAddress; });

                // Populate entity Receiver field 
                PopulateDeliveredEmailAddress(email.Receiver, 
                    (receiverEmailAddress) => { deliveredEmailEntity.Receiver = receiverEmailAddress; });

                // Add new entry into DB
                _emailRepository.AddDeliveredEmail(deliveredEmailEntity);

                // Save all outstanding changes to DB
                if (!_emailRepository.Save())
                {
                    return StatusCode(500, "A problem occurred while saving to the database.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem occurred while saving to the database.");
            }

            // Return email response to client
            return StatusCode(201, response.Successful);

            // Helper method that checks if sender/receiver of email already exists in DB - uses those entries if they exist
            void PopulateDeliveredEmailAddress(EmailAddressModel person, Action<EmailAddress> setResult)
            {
                // If current person's email address exists in DB...
                if (_emailRepository.EmailAddressExists(person.Email))
                {
                    // Set result to be email address from DB
                    setResult(_emailRepository.GetEmailAddress(person));
                }
                // If person is not in DB...
                else
                {
                    // Set result to be new email address
                    setResult(_mapper.Map<EmailAddressModel, EmailAddress>(person));
                }
            }
        }
    }
}