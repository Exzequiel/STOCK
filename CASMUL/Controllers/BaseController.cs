using CASMUL.DB;
using CASMUL.Models.Base;
using System;
using System.Collections.Generic;
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

        public int ObtenerIdFincaPorUsuario()
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                return context.usuario.FirstOrDefault(x => x.AspNetUsers.UserName == User.Identity.Name)?.id_finca ?? 0; 
            }
        }

        public string ObtenerNombreFincaPorUsuario()
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                return context.usuario.FirstOrDefault(x => x.AspNetUsers.UserName == User.Identity.Name)?.finca?.descripcion??"";
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

        public int ObtenerCantidadDisponible(item x)
        {
             return x.cant_disponible - (x.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0) - (x.requisa_detalle.Any(y => y.activo && !y.requisa.movimiento.Any(z => z.activo)) ? x.requisa_detalle.Where(y => y.activo && !y.requisa.movimiento.Any(z => z.activo)).Sum(z => z.cant_enviada) : 0);
        }

        public List<SelectListItem> ObtenerListaItemParaSeleccionar()
        {
            using (var context = new CASMUL.DB.dbcasmulEntities())
            {
                var lista = context.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.cod_item + " - " + x.descripcion + " |Medida: " + x.unidad_medida.descripcion + " |Categoria: " + x.categoria.descripcion + " |Disponible: " + (x.cant_disponible - (x.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0) - (x.requisa_detalle.Any(y => y.activo && !y.requisa.movimiento.Any(z => z.activo)) ? x.requisa_detalle.Where(y => y.activo && !y.requisa.movimiento.Any(z => z.activo)).Sum(z => z.cant_enviada) : 0)) + " |" }).ToList();
                return lista;
            }

        }

     
    }
}