using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Mercaderia
{
    public class CrearMercaderiaViewModel
    {
        public int id { get; set; }
        public string nro_mercaderia { get; set; }
        public int id_pedido { get; set; }
        public DateTime fecha_transaccion { get; set; }
        public int semana { get; set; }
        public int periodo { get; set; }
        public bool activo { get; set; }

        public List<CrearDetalleMercaderiaViewModel> ListaDetalle { get; set; }

        public string nro_pedido { get; set; }
        public string NombreFinca { get; set; }
        public string NombreProveedor { get; set; }
        public bool EsEditar { get; set; }
    }
}