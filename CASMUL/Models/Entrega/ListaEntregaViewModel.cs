using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Entrega
{
    public class ListaEntregaViewModel
    {
        public int id_entrega { get; set; }
        public string nro_entrega { get; set; }
        public DateTime fecha_transaccion { get; set; }
        public string nombre_item { get; set; }
        public string nombre_cable { get; set; }
        public int cant_aentregar { get; set; }
        public bool confirmado { get; set; }
        public string solicitante { get; set; }
        public int semana { get; set; }
        public int periodo { get; set; }
        public bool activo { get; set; }
    }
}