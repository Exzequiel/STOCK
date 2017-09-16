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
        #region Listar

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
                    confirmado = x.confirmado,
                    solicitante = x.solicitante,
                    semana = x.semana,
                    periodo = x.periodo,
                    activo = x.activo,
                    finca = x.id_finca.ToString()
                }).ToList();
                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }      
        }

        [HttpGet]
        public ActionResult VerDetalleEntrega(int Id)
        {
            ViewBag.Id = Id;
            return PartialView();      
        }

        [HttpGet]
        public ActionResult CargarTablaDetalleEntrega(int IdEntrega)
        {
            using (var context = new dbcasmulEntities())
            {
                var list = context.entrega_detalle.Where(x => x.id_entrega == IdEntrega && x.activo).Select(x => new CrearDetalleEntregaViewModel
                {
                    id_entrega = x.id_entrega,
                    id_item = x.id_item,
                    id_detalle_entrega = x.id_detalle_entrega,
                    cant_aentregar = x.cant_aentregar,
                    cant_disponible = x.item.cant_disponible - (x.item.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.item.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0),
                    categoria = x.item.categoria.descripcion,
                    descripcion =x.item.cod_item+" - "+ x.item.descripcion,
                    unidad_medida = x.item.unidad_medida.descripcion,
                    activo = x.activo
                }).ToList();
                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        #endregion

        #region Crear
        [HttpGet]
        public ActionResult CrearEntrega()
        {
            using (var conexion = new dbcasmulEntities())
            {
                ViewBag.ListaFinca = conexion.finca.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_finca.ToString(), Text = x.descripcion }).ToList();
                ViewBag.ListaCables = new List<SelectListItem>();
                ViewBag.ListaItem = conexion.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.cod_item + " - " + x.descripcion }).ToList();
                return View( new CrearEntregaViewModel {nro_entrega=getConfiguracion("CorrelativoEntrega"), fecha_transaccion=DateTime.Now, semana=ObtenerSemana(), periodo=ObtenerPeriodo() });
            }
        }

        [HttpPost]
        public ActionResult CrearEntrega(CrearEntregaViewModel model)
        {
            using(var context = new dbcasmulEntities())
            {
                var NuevaEntrega = context.entrega.Add(new entrega {
                    nro_entrega = getConfiguracion("CorrelativoEntrega"),
                    fecha_transaccion = DateTime.Now,
                    id_finca = model.id_finca,
                    solicitante = model.solicitante,
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
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
                if (resultado) SumarCorrelativo("CorrelativoEntrega");
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
                    cant_disponible = model.cant_disponible - (model.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? model.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0),
                    categoria = model.categoria.descripcion,
                    unidad_medida = model.unidad_medida.descripcion,
                    descripcion =model.cod_item+" - "+ model.descripcion,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Editar
        [HttpGet]
        public ActionResult EditarEntrega(int IdEntrega)
        {
            using(var context = new dbcasmulEntities())
            {
                var ModelEntrega = context.entrega.Find(IdEntrega);
                ViewBag.ListaFinca = context.finca.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_finca.ToString(), Text = x.descripcion }).ToList();
                ViewBag.ListaCables = context.cable.Where(x => x.activo && x.grupo.id_finca==ModelEntrega.id_finca).Select(x => new SelectListItem { Value = x.id_cable.ToString(), Text = x.descripcion +" - "+x.grupo.descripcion }).ToList();
                ViewBag.ListaItem = context.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text =x.cod_item+" - "+ x.descripcion }).ToList();

                return View("CrearEntrega", new CrearEntregaViewModel
                {
                    id_entrega =ModelEntrega.id_entrega,
                    id_cable = ModelEntrega.cable_por_entrega.Select(x=>x.id_cable).ToArray(),
                    fecha_transaccion = ModelEntrega.fecha_transaccion,
                    id_finca = ModelEntrega.id_finca,
                    nro_entrega = ModelEntrega.nro_entrega,
                    semana = ModelEntrega.semana,
                    periodo = ModelEntrega.periodo,
                    solicitante = ModelEntrega.solicitante,
                    EsEditar = true,
                });
            }
        }

        [HttpPost]
        public ActionResult EditarEntrega(CrearEntregaViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var ModelEntrega = context.entrega.Find(model.id_entrega);
                ModelEntrega.id_finca = model.id_finca;
                ModelEntrega.solicitante = model.solicitante;
                context.cable_por_entrega.RemoveRange(ModelEntrega.cable_por_entrega);
                foreach(var idcable in model.id_cable) { context.cable_por_entrega.Add(new cable_por_entrega { id_cable = idcable, id_entrega = ModelEntrega.id_entrega }); }
                ModelEntrega.entrega_detalle.ToList().ForEach(x => x.activo=false);
                foreach(var detalle in model.ListaDetalle)
                {
                    if (ModelEntrega.entrega_detalle.Any(x => x.id_detalle_entrega == detalle.id_detalle_entrega))
                    {
                        var ModelDetalle= ModelEntrega.entrega_detalle.FirstOrDefault(x => x.id_detalle_entrega == detalle.id_detalle_entrega);
                        ModelDetalle.activo = true;
                        ModelDetalle.cant_aentregar = detalle.cant_aentregar;
                    }else
                    {
                        context.entrega_detalle.Add(new entrega_detalle {
                            id_entrega = ModelEntrega.id_entrega,
                            id_item = detalle.id_item,
                            cant_aentregar = detalle.cant_aentregar,
                            activo = true,
                        });
                    }
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Editar Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditarFilaDetalle(int IdDetalleEntrega, int Cantidad)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.entrega_detalle.Find(IdDetalleEntrega);
                model.cant_aentregar = Cantidad;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Editar Detalle Entrega"), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [HttpGet]
        public ActionResult Deshabilitar(int Id)
        {
            using(var context = new dbcasmulEntities())
            {
                var model = context.entrega.Find(Id);
                model.entrega_detalle.ToList().ForEach(x => { x.activo = false; });
                model.activo = false;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Deshabilitar Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ConfirmarEntrega(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.entrega.Find(Id);
                model.confirmado = true;
                foreach(var detalle in model.entrega_detalle.Where(x=>x.activo).ToList())
                {
                    detalle.item.cant_disponible = detalle.item.cant_disponible - detalle.cant_aentregar;
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Confirmar Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

    }
}