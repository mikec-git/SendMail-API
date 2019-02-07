using SendMail.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SendMail.Entities
{
    public class EmailAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        [InverseProperty("Sender")]
        public List<DeliveredEmail> EmailsSent { get; set; }
        [InverseProperty("Receiver")]
        public List<DeliveredEmail> EmailsReceived { get; set; }
    }
}
