using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Fincas
{
    public class CreateFincasViewModel
    {
        [Display(Name = "N° de Finca")]
        [Required]
        public string Description { get; set; }
    }
}