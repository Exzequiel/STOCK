using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Entrega
{
    public class CrearEntregaViewModel
    {
        public int id_entrega { get; set; }

        public string nro_entrega { get; set; }
        [Display(Name ="Fecha de Transaccion")]
        public DateTime fecha_transaccion { get; set; }
        [Required]
        [Display(Name = "Grupo")]
        public int id_grupo { get; set; }
        [Required]
        public string solicitante { get; set; }
        [Required]
        [Display(Name = "Cables")]
        public int[] id_cable { get; set; }

        public int semana { get; set; }
        public int periodo { get; set; }
        public string NombreFinca { get; set; }

        public List<CrearDetalleEntregaViewModel> ListaDetalle { get; set; }

        public bool EsEditar { get; set; }

    }
}