using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.UnidadDeMedida
{
    public class CreateUnidadViewModel
    {
        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }
        [Display(Name = "Abreviatura")]
        [Required]
        public string Abreviatura { get; set; }
    }
}