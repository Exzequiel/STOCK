using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Entrega
{
    public class CrearDetalleEntregaViewModel
    {
        public int id_detalle_entrega { get; set; }
        public int id_entrega { get; set; }
        public int id_item { get; set; }
        public int cant_aentregar { get; set; }


        public string nombre_item { get; set; }
        public string categoria { get; set; }
        public string unidad_medida { get; set; }
        public string descripcion { get; set; }
        public int cant_disponible { get; set; }
        public bool activo { get; set; }
    }
}