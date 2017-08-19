using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Item
{
    public class ListaItemViewModel : EditItemViewModel
    {
        public bool? Activo { get; set; }
        public string Categoria { get; set; }
        public string Unidad { get; set; }
    }
}