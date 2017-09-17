using CASMUL.DB;
using CASMUL.Models.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    [Authorize(Roles = "Administrador,UsuarioFincaPrincipal")]
    public class PedidoController : BaseController
    {
        public PedidoController()
        {
            ViewBag.ControllerName = "Pedido";
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
                var list = context.pedido.Select(x => new CrearPedidoViewModel
                {
                    id_pedido = x.id_pedido,
                    nro_pedido = x.nro_pedido,
                    NombreFinca = x.finca.descripcion,
                    NombreProveedor = x.proveedor.cod_proveedor+" - "+ x.proveedor.nombre_proveedor,
                    fecha_transaccion = x.fecha_transaccion,
                    semana = x.semana,
                    periodo = x.periodo,
                    activo = x.activo,
                    confirmado = x.mercaderia.Any(y => y.activo)
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
                var list = context.pedido_detalle.Where(x => x.id_pedido == Id && x.activo).Select(x => new CrearDetallePedidoViewModel
                {
                    id_pedido = x.id_pedido,
                    id_detalle_pedido = x.id_detalle_pedido,
                    id_item = x.id_item,
                    cant_solicitada = x.cant_solicitada,
                    cant_disponible = x.item.cant_disponible - (x.item.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.item.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0),
                    categoria = x.item.categoria.descripcion,
                    descripcion = x.item.cod_item + " - " + x.item.descripcion,
                    unidad_medida = x.item.unidad_medida.descripcion
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
                ViewBag.ListaProveedor = context.proveedor.Where(x => x.activo ?? false).Select(x => new SelectListItem { Value = x.id_proveedor.ToString(), Text = x.cod_proveedor + " - " + x.nombre_proveedor }).ToList();
                ViewBag.ListaItem = context.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.cod_item + " - " + x.descripcion }).ToList();
                return View(new CrearPedidoViewModel
                {
                    fecha_transaccion = DateTime.Now,
                    nro_pedido = getConfiguracion("CorrelativoPedido"),
                    NombreFinca = ObtenerNombreFincaPorUsuario(),
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
                });
            }
        }

        [HttpPost]
        public ActionResult Crear(CrearPedidoViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var NuevaModelo = context.pedido.Add(new pedido
                {
                    nro_pedido = getConfiguracion("CorrelativoPedido"),
                    id_finca = ObtenerIdFincaPorUsuario(),
                    id_proveedor = model.id_proveedor,
                    fecha_transaccion = DateTime.Now,
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
                    activo = true,
                });

                foreach (var detalle in model.ListaDetalle)
                {
                    context.pedido_detalle.Add(new pedido_detalle
                    {
                        id_pedido = NuevaModelo.id_pedido,
                        cant_solicitada = detalle.cant_solicitada,
                        id_item = detalle.id_item,
                        activo = true
                    });
                }

                var resultado = context.SaveChanges() > 0;
                if (resultado) SumarCorrelativo("CorrelativoPedido");
                return Json(EnviarResultado(resultado, "Pedido creado exitosamente"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Editar(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                ViewBag.ListaProveedor = context.proveedor.Where(x=>x.activo??false).Select(x => new SelectListItem { Value = x.id_proveedor.ToString(), Text = x.cod_proveedor + " - " + x.nombre_proveedor }).ToList();
                ViewBag.ListaItem = context.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.cod_item + " - " + x.descripcion }).ToList();
                var model = context.pedido.Find(Id);
                return View("Crear", new CrearPedidoViewModel
                {
                    id_pedido = model.id_pedido,
                    id_finca = model.id_finca,
                    id_proveedor = model.id_proveedor,
                    fecha_transaccion = model.fecha_transaccion,
                    nro_pedido = model.nro_pedido,
                    periodo = model.periodo,
                    semana = model.semana,
                    NombreFinca = model.finca.descripcion,
                    EsEditar = true
                });
            }
        }

        [HttpPost]
        public ActionResult Editar(CrearPedidoViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var ModelDb = context.pedido.Find(model.id_pedido);
                ModelDb.id_proveedor = model.id_proveedor;

                ModelDb.pedido_detalle.ToList().ForEach(x => x.activo = false);
                foreach (var detalle in model.ListaDetalle)
                {
                    if (ModelDb.pedido_detalle.Any(x => x.id_detalle_pedido == detalle.id_detalle_pedido))
                    {
                        var ModelDetalle = ModelDb.pedido_detalle.FirstOrDefault(x => x.id_detalle_pedido == detalle.id_detalle_pedido);
                        ModelDetalle.activo = true;
                        ModelDetalle.cant_solicitada = detalle.cant_solicitada;
                    }
                    else
                    {
                        context.pedido_detalle.Add(new pedido_detalle
                        {
                            id_pedido = ModelDb.id_pedido,
                            cant_solicitada = detalle.cant_solicitada,
                            id_item = detalle.id_item,
                            activo = true,
                        });
                    }
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Pedido editado exitosamente"), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerInfoItem(int IdItem)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.item.Find(IdItem);
                return Json(new CrearDetallePedidoViewModel
                {
                    id_detalle_pedido = 0,
                    cant_solicitada = 0,
                    id_pedido = 0,
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
                var model = context.pedido.Find(Id);
                model.pedido_detalle.ToList().ForEach(x => { x.activo = false; });
                model.activo = false;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Pedido Deshabilitada"), JsonRequestBehavior.AllowGet);
            }
        }

    }
}