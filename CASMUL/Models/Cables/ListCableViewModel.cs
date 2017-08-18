using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Cables
{
    public class ListCableViewModel : CreateCableViewModel
    {
        public int IdCable { get; set; }
        public string Grupo { get; set; }
        public bool? Activo { get; set; }
        public string Finca { get; set; }
    }
}