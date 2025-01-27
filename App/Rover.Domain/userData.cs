using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class userData : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string emailUser { get; set; } 
        public string telnumUser { get; set; } 
        public string passwordUser { get; set; }
        public string nameUser { get; set; }
        public string surnameUser { get; set; }
        public DateTime birthdayUser { get; set; }
        public string sexUser { get; set; } 
    }
}
