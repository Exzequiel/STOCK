using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Pedido
{
    public class CrearPedidoViewModel
    {
        public int id_pedido { get; set; }
        public string nro_pedido { get; set; }
        public int id_finca { get; set; }
        [Display(Name ="Proveedor")]
        [Required]
        public int id_proveedor { get; set; }
        public DateTime fecha_transaccion { get; set; }
        public int semana { get; set; }
        public int periodo { get; set; }
        public bool activo { get; set; }
        public bool confirmado { get; set; }
        public List<CrearDetallePedidoViewModel> ListaDetalle { get; set; }

        public string NombreFinca { get; set; }
        public string NombreProveedor { get; set; }
        public bool EsEditar { get; set; }
    }
}