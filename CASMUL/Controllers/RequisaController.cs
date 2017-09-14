using CASMUL.DB;
using CASMUL.Models.Requisa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    public class RequisaController : BaseController
    {
        // GET: Requisa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargarListaRequisas()
        {
            using (var context = new dbcasmulEntities())
            {
                var list = context.requisa.Select(x => new ListaRequisaViewModel {
                    Id = x.id_requisa,
                    nro_requisa = x.nro_requisa,
                    finca = x.finca.descripcion,
                    fecha_transaccion = x.fecha_transaccion,
                    semana = x.semana,
                    periodo = x.periodo
                }).ToList();

                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        [HttpGet]
        public ActionResult Crear()
        {
            using (var context = new dbcasmulEntities())
            {
                ViewBag.ListaItem = context.item.Where(x => x.activo).Select(x=>new SelectListItem { Value = x.id_item.ToString(), Text = x.descripcion}).ToList();
                ViewBag.ListaFinca = context.finca.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_finca.ToString(), Text = x.descripcion }).ToList();
                return View(new CrearRequisaViewModel { fecha_transaccion = DateTime.Now});
            }
        }

        [HttpPost]
        public ActionResult Crear(CrearRequisaViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                context.requisa.Add(new requisa {
                    nro_requisa = model.nro_requisa,
                    id_finca = model.id_finca,
                    fecha_transaccion = model.fecha_transaccion,
                    semana = model.semana,
                    periodo = model.periodo
                });

                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Crear Requisa"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Editar(int Id)
        {
            using(var context = new dbcasmulEntities())
            {
                var model = context.requisa.Find(Id);
                return View(new CrearRequisaViewModel {
                    Id = model.id_requisa,
                    id_finca = model.id_finca,
                    fecha_transaccion = model.fecha_transaccion,
                    nro_requisa = model.nro_requisa,
                    periodo = model.periodo,
                    semana = model.semana
                });
            }
        }



        //public ActionResult Deshabilitar(int Id) {
        //    using (var context = new dbcasmulEntities())
        //    {
        //        var model = context.requisa.Find(Id);
               

        //    }

        //}



    }
}