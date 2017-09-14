using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Requisa
{
    public class CrearRequisaViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Numero Requisa")]
        public string nro_requisa { get; set; }

        [Required]
        [Display(Name = "Finca")]
        public int id_finca { get; set; }
        [Required]
        [Display(Name = "Fecha Transaccio")]
        public DateTime fecha_transaccion { get; set; }

        public int semana { get; set; }
        public int periodo { get; set; }
    }
}