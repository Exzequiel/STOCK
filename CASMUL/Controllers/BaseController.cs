using CASMUL.Models.Base;
using System;
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
                Mensaje = resultado ? "Se guardo exitosamente" : "Error al guardar",
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
        public string getConfiguracion(string Nombre)
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                return context.configuracion.Where(x => x.nombre == Nombre)?.FirstOrDefault()?.valor ?? "No hay configuracion";
            }
        }

        public void SumarCorrelativo(string Nombre)
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                var configuracion= context.configuracion.FirstOrDefault(x => x.nombre == Nombre);
                if (configuracion != null)
                {
                    var total = Convert.ToInt32(configuracion.valor) + 1;
                    configuracion.valor = total.ToString();
                    context.SaveChanges();
                }


            }
        }

        public int ObtenerIdUsuario()
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                return context.usuario.FirstOrDefault(x => x.AspNetUsers.UserName == User.Identity.Name)?.id_usuario ?? 0;

            }
        }

        public int getIdFincaPorUsuario()
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                return context.usuario.FirstOrDefault(x => x.AspNetUsers.UserName == User.Identity.Name)?.id_finca ?? 0; 
            }
        }



        public int ObtenerSemana()
        {
            decimal cantidadDias = (DateTime.Now - DateTime.Parse("2017-01-02")).Days;
            decimal semana= cantidadDias > 365 ? Convert.ToInt32(Math.Ceiling((cantidadDias % 365) / 7)) : Convert.ToInt32(Math.Ceiling(cantidadDias / 7));
            return Convert.ToInt32(Math.Ceiling(semana%4)+1);
        }

        public int ObtenerPeriodo()
        {
            decimal cantidadDias = (DateTime.Now - DateTime.Parse("2017-01-02")).Days;
            decimal semana= cantidadDias > 365 ? Convert.ToInt32(Math.Ceiling((cantidadDias % 365) / 7)) : Convert.ToInt32(Math.Ceiling(cantidadDias / 7));
            return Convert.ToInt32( Math.Ceiling(semana / 4));
        }

    }
}