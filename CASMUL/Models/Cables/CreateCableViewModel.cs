using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Cables
{
    public class CreateCableViewModel
    {
        [Display(Name = "Acres")]
        [Required]
        public decimal? Acres { get; set; }
        [Display(Name = "Hectarias")]
        [Required]
        public decimal? Hectarias { get; set; }

        [Display(Name = "Grupo")]
        [Required]
        public int? IdGrupo { get; set; }
    }
}