using System.Linq;

namespace CASMUL.Helpers
{
    public static class GetInfoUsuario
    {
        static string Nombre { get; set; }
        static string TipoUsuario { get; set; }

        public static void GetInfo(string UserName)
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                var info = context.usuario.FirstOrDefault(x => x.AspNetUsers.UserName == UserName);
                if (info != null)
                {
                    Nombre = info.nombre + " " + info.apellido;
                    TipoUsuario = info.AspNetUsers.AspNetRoles.FirstOrDefault().Name ?? "";
                }
            }
        }

        public static string ObtenerNombre()
        {
            return Nombre;
        }
        public static string ObtenerTipoUsuario()
        {
            return TipoUsuario;
        }
    }
}
