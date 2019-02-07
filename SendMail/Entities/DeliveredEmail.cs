using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SendMail.Entities
{
    public class DeliveredEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public EmailAddress Receiver { get; set; }
        public EmailAddress Sender { get; set; }
        public Email Email { get; set; }
    }
}
