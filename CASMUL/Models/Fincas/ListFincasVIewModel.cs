using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Fincas
{
    public class ListFincasVIewModel : CreateFincasViewModel
    {
        public int IdFincas { get; set; }
        public bool? Activo { get; set; }
    }
}