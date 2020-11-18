using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AuthorizationService.Models
{
    [NotMapped]
    public class JwtUser
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
