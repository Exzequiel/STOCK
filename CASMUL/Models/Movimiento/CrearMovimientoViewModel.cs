using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Movimiento
{
    public class CrearMovimientoViewModel
    {
        public int id { get; set; }
        public string nro_movimiento { get; set; }
        public int id_requisa { get; set; }
        public DateTime fecha_transaccion { get; set; }
        public int semana { get; set; }
        public int periodo { get; set; }
        public bool activo { get; set; }
        public List<CrearDetalleMovimientoViewModel> ListaDetalle { get; set; }

        public string nro_requisa { get; set; }
        public string NombreFinca { get; set; }
        public bool EsEditar { get; set; }
    }
}