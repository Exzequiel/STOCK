using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Grupos
{
    public class ListGruposViewModel : CreateGrupoViewModel
    {
        public int IdGrupo { get; set; }
        public string NombreFinca { get; set; }
        public bool? Activo { get; set; }
    }
}