using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CASMUL.Models.Item
{
    public class CreateItemVIewModel
    {
        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }

        [Display(Name = "Categoría")]
        [Required]
        public int IdCategoria { get; set; }

        [Display(Name = "Unidad de Medida")]
        [Required]
        public int IdUnidad { get; set; }

        [Display(Name = "Cantidad Disponible")]
        [Required]
        public int CantidadDisponible { get; set; }

        [Display(Name = "Código Producto")]
        [Required]
        public string cod_item { get; set; }
    }
}