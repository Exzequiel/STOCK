using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Movimiento
{
    public class CrearDetalleMovimientoViewModel
    {
        public int id_detalle_movimiento { get; set; }
        public int id_movimiento { get; set; }
        public int id_item { get; set; }
        public int cant_enviada { get; set; }
        public bool activo { get; set; }

        public string categoria { get; set; }
        public string unidad_medida { get; set; }
        public string descripcion { get; set; }
        public int cant_disponible { get; set; }
    }
}