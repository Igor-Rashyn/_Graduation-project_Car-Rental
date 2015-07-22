using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class Email : BaseEntity
    {

        public Email()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string Subject { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Body { get; set; }

    }
}