using System;
using System.ComponentModel.DataAnnotations;

namespace CASMUL.Models.Usuario
{
    public class CrearUsuarioViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "N° Identidad")]
        //[StringLength(13, MinimumLength = 13, ErrorMessage = "No tiene el formato correcto (La identidad tiene que ser de 13 digitos)")]
        public string Identidad { get; set; }

        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNac { get; set; }

        [Required]
        [Display(Name = "N° de Telefono o Celular")]
        public string Telefono { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Correo Electronico")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La nueva contraseña y la confirmacion no son las mismas")]
        public string ConfirmPassword { get; set; }

        public string IdAspNetUser { get; set; }

        [Required]
        [Display(Name = "Tipo de Usuario")]
        public string RoleUsuario { get; set; }

        public bool Estado { get; set; }
    }
}