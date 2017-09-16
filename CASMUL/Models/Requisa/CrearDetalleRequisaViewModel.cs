using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Requisa
{
    public class CrearDetalleRequisaViewModel
    {
        public int id_detail_requisa { get; set; }
        public int id_requisa { get; set; }
        public int id_item { get; set; }
        public int cant_enviada { get; set; }

        public string categoria { get; set; }
        public string unidad_medida { get; set; }
        public string descripcion { get; set; }
        public int cant_disponible { get; set; }

    }
}