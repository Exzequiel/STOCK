using System.ComponentModel.DataAnnotations;

namespace CASMUL.Models.Usuario
{
    public class DetalleUsuarioViewModel
    {
        public int PersonaID { get; set; }
        public string IdAspnetUser { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identidad { get; set; }
        public System.DateTime FechaNac { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public string TipoUsuario { get; set; }

        [Display(Name = "Finca")]
        public string NombreFinca { get; set; }
    }
}