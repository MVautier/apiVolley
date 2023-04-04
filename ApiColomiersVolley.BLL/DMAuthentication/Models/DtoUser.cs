using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Models
{
    public class DtoUser
    {
        public int IdUser { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Mail { get; set; }
        public string? Password { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Role { get; set; }
    }
}
