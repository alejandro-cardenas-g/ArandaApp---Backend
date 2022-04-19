using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArandaApp.Models
{
    public class Usuario
    {

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Rol { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }
    }
}
