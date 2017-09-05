using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CASMUL.Models.Proveedores
{
    public class ListaProveedorViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string nombre_proveedor { get; set; }

        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        [Display(Name = "Telefono")]
        public string Telefono { get; set; }
        
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Display(Name = "Contacto")]
        public string Contacto { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }
    }
}