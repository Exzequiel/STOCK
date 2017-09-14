using CASMUL.DB;
using CASMUL.Models.Entrega;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    public class EntregaController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargarTablaListaEntrega()
        {
            using (var conexion = new dbcasmulEntities())
            {
                var list = conexion.entrega.Select(x=> new ListaEntregaViewModel {
                    id_entrega = x.id_entrega,
                    nro_entrega = x.nro_entrega,
                    fecha_transaccion = x.fecha_transaccion,
                    nombre_item = x.item.descripcion,
                    nombre_cable = x.cable.descripcion,
                    cant_aentregar = x.cant_aentregar,
                    confirmado = x.confirmado,
                    solicitante = x.solicitante,
                    semana = x.semana,
                    periodo = x.periodo,
                    activo = x.activo
                }).ToList();
                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }      
        }

        [HttpGet]
        public ActionResult VerDetalleEntrega(int IdEntrega)
        {
            ViewBag.IdEntrega = IdEntrega;
            return PartialView();
        }

        [HttpGet]
        public ActionResult CargarTablaDetalleEntrega(int IdEntrega)
        {
            using(var context = new dbcasmulEntities())
            {
                var list = context.entrega_detalle.Where(x => x.id_entrega == IdEntrega).Select(x => new ListaDetalleEntregaViewModel {
                    id_detalle_entrega = x.id_detalle_entrega,
                    nombre_item = x.item.descripcion,
                    cant_aentregar = x.cant_aentregar,
                    activo = x.activo
                }).ToList();
                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }


        [HttpGet]
        public ActionResult CrearEntrega()
        {
            using (var conexion = new dbcasmulEntities())
            {
                ViewBag.ListaFinca = conexion.finca.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_finca.ToString(), Text = x.descripcion }).ToList();
                ViewBag.ListaCables = new List<SelectListItem>();
                ViewBag.ListaItem = conexion.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.descripcion }).ToList();
                return View( new CrearEntregaViewModel {nro_entrega=getConfiguracion("CorrelativoEntrega"), fecha_transaccion=DateTime.Now, semana=1, periodo=1 });
            }
        }

        [HttpPost]
        public ActionResult CrearEntrega(CrearEntregaViewModel model)
        {
            using(var context = new dbcasmulEntities())
            {
                var NuevaEntrega = context.entrega.Add(new entrega {
                    nro_entrega = getConfiguracion("CorrelativoEntrega"),
                    fecha_transaccion = model.fecha_transaccion,
                    id_finca = model.id_finca,
                    solicitante = model.solicitante,
                    semana = 1,
                    periodo = 2,
                    activo =true,
                    confirmado = false,
                    
                });
                foreach (var Idcable in model.id_cable.ToList())
                {
                    context.cable_por_entrega.Add(new cable_por_entrega {
                        id_cable = Idcable,
                        id_entrega = NuevaEntrega.id_entrega
                    });
                }

                foreach(var detalle in model.ListaDetalle??new List<CrearDetalleEntregaViewModel>())
                {
                    context.entrega_detalle.Add(new entrega_detalle {
                        cant_aentregar = detalle.cant_aentregar,
                        id_entrega = NuevaEntrega.id_entrega,
                        id_item = detalle.id_item,
                        activo = true
                    });
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Crear Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerListaCablesPorFinca(int IdFinca)
        {
            using(var context = new dbcasmulEntities())
            {
                var lista = context.cable.Where(x => x.grupo.id_finca == IdFinca).Select(x => new { id = x.id_cable, text = x.descripcion + " - " + x.grupo.descripcion }).ToList();
                return Json(new { list = lista}, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ObtenerInfoItem(int IdItem)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.item.Find(IdItem);
                return Json(new CrearDetalleEntregaViewModel {
                    id_detalle_entrega = 0,
                    cant_aentregar = 0,
                    id_entrega = 0,
                    id_item = IdItem,
                    cant_disponible = model.cant_disponible,
                    categoria = model.categoria.descripcion,
                    unidad_medida = model.unidad_medida.descripcion,
                    descripcion = model.descripcion,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Deshabilitar(int IdEntrega)
        {
            using(var context = new dbcasmulEntities())
            {
                var model = context.entrega.Find(IdEntrega);
                model.activo = false;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Deshabilitar Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ConfirmarEntrega(int IdEntrega)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.entrega.Find(IdEntrega);
                model.confirmado = true;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Confirmar Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditarDetalle(int IdDetalleEntrega, int Cantidad)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.entrega_detalle.Find(IdDetalleEntrega);
                model.cant_aentregar = Cantidad;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Editar Detalle Entrega"), JsonRequestBehavior.AllowGet);
            }          
        }






    }
}