using CASMUL.DB;
using CASMUL.Models.Movimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    [Authorize(Roles = "Administrador,UsuarioFincaPrincipal")]
    public class MovimientoController : BaseController
    {
        public MovimientoController()
        {
            ViewBag.ControllerName = "Movimiento";
        }

        // GET: Requisa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargarTablaIndex()
        {
            using (var context = new dbcasmulEntities())
            {
                var list = context.movimiento.Select(x => new CrearMovimientoViewModel
                {
                    id = x.id_movimiento,
                    nro_movimiento = x.nro_movimiento,
                    NombreFinca = x.requisa.finca.descripcion,
                    id_requisa = x.id_requisa,
                    nro_requisa = x.requisa.nro_requisa,
                    fecha_transaccion = x.fecha_transaccion,
                    semana = x.semana,
                    periodo = x.periodo,
                    activo = x.activo,
                    
                }).ToList();

                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        [HttpGet]
        public ActionResult VerDetalleIndex(int Id)
        {
            ViewBag.Id = Id;
            return PartialView();
        }

        [HttpGet]
        public ActionResult CargarTablaDetalle(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var list = context.movimiento_detalle.Where(x => x.id_movimiento == Id && x.activo).Select(x => new CrearDetalleMovimientoViewModel
                {
                    id_detalle_movimiento = x.id_detalle_movimiento,
                    id_movimiento = x.id_movimiento,
                    activo = x.activo,
                    id_item = x.id_item,
                    cant_enviada = x.cant_enviada,
                    cant_disponible = x.item.cant_disponible - (x.item.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.item.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0),
                    categoria = x.item.categoria.descripcion,
                    descripcion = x.item.cod_item + " - " + x.item.descripcion,
                    unidad_medida = x.item.unidad_medida.descripcion,
                }).ToList();
                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        [HttpGet]
        public ActionResult CargarTablaDetalleRequisa(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var list = context.requisa_detalle.Where(x => x.id_requisa == Id && x.activo).Select(x => new CrearDetalleMovimientoViewModel
                {
                    id_item = x.id_item,
                    cant_enviada = x.cant_enviada,
                    cant_disponible = x.item.cant_disponible - (x.item.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.item.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0),
                    categoria = x.item.categoria.descripcion,
                    descripcion = x.item.cod_item + " - " + x.item.descripcion,
                    unidad_medida = x.item.unidad_medida.descripcion,
                }).ToList();
                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        [HttpGet]
        public ActionResult Crear(int IdRequisa)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.requisa.Find(IdRequisa);
                return PartialView(new CrearMovimientoViewModel
                {
                    fecha_transaccion = DateTime.Now,
                    nro_movimiento = getConfiguracion("CorrelativoMovimiento"),
                    NombreFinca = ObtenerNombreFincaPorUsuario(),
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
                    nro_requisa = model.nro_requisa,
                    id_requisa = model.id_requisa
                });
            }
        }

        [HttpPost]
        public ActionResult Crear(CrearMovimientoViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var NuevaRequisa = context.movimiento.Add(new movimiento
                {
                    nro_movimiento = getConfiguracion("CorrelativoMovimiento"),
                    fecha_transaccion = DateTime.Now,
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
                    activo = true,
                    id_requisa = model.id_requisa,
                });

                foreach (var detalle in model.ListaDetalle)
                {
                    context.movimiento_detalle.Add(new movimiento_detalle
                    {
                        id_movimiento = NuevaRequisa.id_movimiento,
                        cant_enviada = detalle.cant_enviada,
                        id_item = detalle.id_item,
                        activo = true
                    });
                    var item = context.item.Find(detalle.id_item);
                    item.cant_disponible = item.cant_disponible - detalle.cant_enviada;
                }

                var resultado = context.SaveChanges() > 0;
                if (resultado) SumarCorrelativo("CorrelativoMovimiento");
                return Json(EnviarResultado(resultado, "Movimiento creado"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Editar(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.movimiento.Find(Id);
                return PartialView("Crear", new CrearMovimientoViewModel
                {
                    id_requisa = model.id_requisa,
                    id = model.id_movimiento,
                    nro_movimiento = model.nro_movimiento,
                    fecha_transaccion = model.fecha_transaccion,
                    nro_requisa = model.requisa.nro_requisa,
                    periodo = model.periodo,
                    semana = model.semana,
                    NombreFinca = model.requisa.finca.descripcion,
                    EsEditar = true
                });
            }
        }

        [HttpPost]
        public ActionResult Editar(CrearMovimientoViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var ModelDb = context.movimiento.Find(model.id);
                foreach (var detalle in model.ListaDetalle)
                {
                    if (ModelDb.movimiento_detalle.Any(x => x.id_detalle_movimiento == detalle.id_detalle_movimiento))
                    {
                        var ModelDetalle = ModelDb.movimiento_detalle.FirstOrDefault(x => x.id_detalle_movimiento == detalle.id_detalle_movimiento);
                        ModelDetalle.activo = true;
                        ModelDetalle.item.cant_disponible = ModelDetalle.item.cant_disponible + ModelDetalle.cant_enviada - detalle.cant_enviada;
                        ModelDetalle.cant_enviada = detalle.cant_enviada; 
                    }
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Movimiento editado"), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerInfoItem(int IdItem)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.item.Find(IdItem);
                return Json(new CrearDetalleMovimientoViewModel
                {
                    id_movimiento = 0,
                    cant_enviada = 0,
                    id_detalle_movimiento = 0,
                    id_item = IdItem,
                    cant_disponible = model.cant_disponible - (model.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? model.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0),
                    categoria = model.categoria.descripcion,
                    unidad_medida = model.unidad_medida.descripcion,
                    descripcion = model.cod_item + " - " + model.descripcion,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Deshabilitar(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.movimiento.Find(Id);
                model.movimiento_detalle.ToList().ForEach(x => { x.activo = false; x.item.cant_disponible = x.item.cant_disponible + x.cant_enviada; });
                model.activo = false;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Movimiento Deshabilitada"), JsonRequestBehavior.AllowGet);
            }
        }



    }
}