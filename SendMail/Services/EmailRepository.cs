using SendMail.Data;
using SendMail.Entities;
using SendMail.Models;
using System.Linq;

namespace SendMail.Services
{
    public class EmailRepository : IEmailRepository
    {
        private readonly EmailDbContext _context;

        public EmailRepository(EmailDbContext context)
        {
            _context = context;
        }

        public bool EmailAddressExists(string emailAddress)
        {
            return _context.EmailAddresses.Any(a => a.Email == emailAddress);
        }

        public EmailAddress GetEmailAddress(EmailAddressModel emailAddress)
        {
            var emailAddressFromDb = _context.EmailAddresses
                .Where(e => e.Email== emailAddress.Email)
                .Select(e => e)
                .SingleOrDefault();

            if (emailAddressFromDb.Name != emailAddress.Name)
            {
                emailAddressFromDb.Name = emailAddress.Name;
            }

            return emailAddressFromDb;
        }
        
        public void AddDeliveredEmail(DeliveredEmail deliveredEmail)
        {
            _context.DeliveredEmails.Add(deliveredEmail);
        }

        /// <summary>
        /// Saves any updates made to the database context
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            // Retuns the number of state entries written to the database.
            return (_context.SaveChanges() >= 0);
        }

    }
}
