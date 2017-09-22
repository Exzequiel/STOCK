using System.ComponentModel.DataAnnotations;

namespace CASMUL.Models.Usuario
{
    public class CambiarContrasenaViewModel
    {
        public int IdUser { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe de ser de por lo menos {2} caracteres de largo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("NewPassword", ErrorMessage = "La contraseña nueva y la contraseña de  confirmación no son iguales.")]
        public string ConfirmPassword { get; set; }
    }
}