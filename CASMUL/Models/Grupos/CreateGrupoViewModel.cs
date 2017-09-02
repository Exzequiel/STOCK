using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Grupos
{
    public class CreateGrupoViewModel
    {
        [Display(Name = "Descripcion")]
        [Required]
        public string Description { get; set; }

        //[Display(Name = "Finca")]
        //[Required]
        //public int? IdFinca { get; set; }
    }
}