using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArandaApp.Models
{
    public class Rolpermiso
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdPermiso { get; set; }

    }
}
