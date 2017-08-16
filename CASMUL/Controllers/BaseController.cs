using CASMUL.Models.Base;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            base.OnActionExecuted(filterContext);
        }


        public MensajeRespuestaViewModel  EnviarResultado(bool resultado, string Titulo)
        {
            return new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = resultado ? "Accion exitosa" : "Error",
                Estado = resultado
            };
        }

        public MensajeRespuestaViewModel EnviarResultado(bool resultado, string Titulo, string MensajeExitoso, string MensajeError)
        {
            return new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = resultado ? MensajeExitoso : MensajeError,
                Estado = resultado
            };

        }

        public int ObtenerIdUsuario()
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                return context.usuario.FirstOrDefault(x => x.AspNetUsers.UserName == User.Identity.Name)?.id_usuario??0;

            }
        }
    }
}