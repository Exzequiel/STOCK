using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Requisa
{
    public class ListaRequisaViewModel
    {
        public int id_requisa { get; set; }
        [Display(Name="Numero Requisa")]
        public string nro_requisa { get; set; }
        [Display(Name = "Finca")]
        public string finca { get; set; }
        [Display(Name = "Fecha Transaccion")]
        public DateTime fecha_transaccion { get; set; }
        [Display(Name = "Semana")]
        public int semana { get; set; }
        [Display(Name = "Periodo")]
        public int periodo { get; set; }
        public bool activo { get; set; }
        public bool confirmado { get; set; }

        public int IdFinca { get; set; }
    }
}