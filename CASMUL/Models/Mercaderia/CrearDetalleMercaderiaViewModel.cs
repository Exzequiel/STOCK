using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Mercaderia
{
    public class CrearDetalleMercaderiaViewModel
    {
        public int id_detalle_mercaderia { get; set; }
        public int id_mercaderia { get; set; }
        public int id_item { get; set; }
        public int cant_recibida { get; set; }
        public bool activo { get; set; }

        public string categoria { get; set; }
        public string unidad_medida { get; set; }
        public string descripcion { get; set; }
        public int cant_disponible { get; set; }
    }
}