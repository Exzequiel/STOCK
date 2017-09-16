using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Pedido
{
    public class CrearDetallePedidoViewModel
    {
        public int id_detalle_pedido { get; set; }
        public int id_pedido { get; set; }
        public int id_item { get; set; }
        public int cant_solicitada { get; set; }

        public string categoria { get; set; }
        public string unidad_medida { get; set; }
        public string descripcion { get; set; }
        public int cant_disponible { get; set; }
    }
}