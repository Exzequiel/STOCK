using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Entrega
{
    public class ListaDetalleEntregaViewModel
    {
        public int id_detalle_entrega { get; set; }
        public string nombre_item { get; set; }
        public int cant_aentregar { get; set; }
        public bool activo { get; set; }

    }
}