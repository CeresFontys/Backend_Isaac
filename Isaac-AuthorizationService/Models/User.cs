using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AuthorizationService.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        //moet veranderd worden naar Email
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
