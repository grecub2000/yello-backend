using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yello.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string KeycloakId { get; set; }
        [Required, MinLength(2), MaxLength(20)]
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
