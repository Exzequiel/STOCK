using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Requisa
{
    public class CrearRequisaViewModel
    {
        public int id_requisa { get; set; }
        [Required]
        [Display(Name ="Número Requisa")]
        public string nro_requisa { get; set; }

        [Required]
        [Display(Name = "Finca")]
        public int id_finca { get; set; }
        [Required]
        [Display(Name = "Fecha Transacción")]
        public DateTime fecha_transaccion { get; set; }

        public int semana { get; set; }
        public int periodo { get; set; }

        public string NombreFinca { get; set; }
        public bool EsEditar { get; set; }

        public List<CrearDetalleRequisaViewModel> ListaDetalle { get; set; }
    }
}