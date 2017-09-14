using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Requisa
{
    public class ListaRequisaViewModel
    {
        public int Id { get; set; }
        [Display(Name="Numero Requisa")]
        public string nro_requisa { get; set; }
        [Display(Name = "Descripcion Item")]
        public string item { get; set; }
        [Display(Name = "Finca")]
        public string finca { get; set; }
        [Display(Name = "Fecha Transaccion")]
        public DateTime fecha_transaccion { get; set; }
        [Display(Name = "Cantidad Solicitada")]
        public int cant_solicitada { get; set; }
        [Display(Name = "Semana")]
        public int semana { get; set; }
        [Display(Name = "Periodo")]
        public int periodo { get; set; }
    }
}