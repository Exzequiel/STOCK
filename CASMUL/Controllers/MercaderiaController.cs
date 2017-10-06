using CASMUL.DB;
using CASMUL.Models.Mercaderia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    [Authorize(Roles = "Administrador,UsuarioFincaPrincipal")]
    public class MercaderiaController : BaseController
    {
        public MercaderiaController()
        {
            ViewBag.ControllerName = "Mercaderia";
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
                var list = context.mercaderia.Select(x => new CrearMercaderiaViewModel
                {
                    id = x.id_mercaderia,
                    nro_mercaderia = x.nro_mercaderia,
                    NombreFinca = x.pedido.finca.descripcion,
                    id_pedido = x.id_pedido,
                    nro_pedido = x.pedido.nro_pedido,
                    fecha_transaccion = x.fecha_transaccion,
                    semana = x.semana,
                    periodo = x.periodo,
                    activo = x.activo,
                    NombreProveedor = x.pedido.proveedor.nombre_proveedor

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
                var list = context.mercaderia_detalle.Where(x => x.id_mercaderia == Id && x.activo).Select(x => new CrearDetalleMercaderiaViewModel
                {
                    id_detalle_mercaderia = x.id_detalle_mercaderia,
                    id_mercaderia = x.id_mercaderia,
                    activo = x.activo,
                    id_item = x.id_item,
                    cant_recibida = x.cant_recibida,
                    cant_disponible = x.item.cant_disponible - (x.item.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.item.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0) - (x.item.requisa_detalle.Any(y => y.activo && !y.requisa.movimiento.Any(z => z.activo)) ? x.item.requisa_detalle.Where(y => y.activo && !y.requisa.movimiento.Any(z => z.activo)).Sum(z => z.cant_enviada) : 0),
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
        public ActionResult CargarTablaDetallePedido(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var list = context.pedido_detalle.Where(x => x.id_pedido == Id && x.activo).Select(x => new CrearDetalleMercaderiaViewModel
                {
                    id_item = x.id_item,
                    cant_recibida = x.cant_solicitada,
                    cant_disponible = x.item.cant_disponible - (x.item.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.item.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0) - (x.item.requisa_detalle.Any(y => y.activo && !y.requisa.movimiento.Any(z => z.activo)) ? x.item.requisa_detalle.Where(y => y.activo && !y.requisa.movimiento.Any(z => z.activo)).Sum(z => z.cant_enviada) : 0),
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
        public ActionResult Crear(int IdPedido)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.pedido.Find(IdPedido);
                return PartialView(new CrearMercaderiaViewModel
                {
                    fecha_transaccion = DateTime.Now,
                    nro_mercaderia = getConfiguracion("CorrelativoMercaderia"),
                    NombreFinca = ObtenerNombreFincaPorUsuario(),
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
                    nro_pedido = model.nro_pedido,
                    id_pedido = model.id_pedido,
                    NombreProveedor = model.proveedor.cod_proveedor+" - "+ model.proveedor.nombre_proveedor                   
                });
            }
        }

        [HttpPost]
        public ActionResult Crear(CrearMercaderiaViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var NuevoMercadera = context.mercaderia.Add(new mercaderia
                {
                    nro_mercaderia = getConfiguracion("CorrelativoMercaderia"),
                    fecha_transaccion = DateTime.Now,
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
                    activo = true,
                    id_pedido = model.id_pedido,
                });

                foreach (var detalle in model.ListaDetalle)
                {
                    context.mercaderia_detalle.Add(new mercaderia_detalle
                    {
                        id_mercaderia = NuevoMercadera.id_mercaderia,
                        cant_recibida = detalle.cant_recibida,
                        id_item = detalle.id_item,
                        activo = true
                    });
                    var item = context.item.Find(detalle.id_item);
                    item.cant_disponible = item.cant_disponible + detalle.cant_recibida;
                }

                var resultado = context.SaveChanges() > 0;
                if (resultado) SumarCorrelativo("CorrelativoMercaderia");
                return Json(EnviarResultado(resultado, "Mercaderia creada"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Editar(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.mercaderia.Find(Id);
                return PartialView("Crear", new CrearMercaderiaViewModel
                {
                    id_pedido = model.id_pedido,
                    id = model.id_mercaderia,
                    nro_mercaderia = model.nro_mercaderia,
                    fecha_transaccion = model.fecha_transaccion,
                    nro_pedido = model.pedido.nro_pedido,
                    periodo = model.periodo,
                    semana = model.semana,
                    NombreFinca = model.pedido.finca.descripcion,
                    NombreProveedor = model.pedido.proveedor.cod_proveedor +" - "+model.pedido.proveedor.nombre_proveedor,
                    EsEditar = true
                });
            }
        }

        [HttpPost]
        public ActionResult Editar(CrearMercaderiaViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var ModelDb = context.mercaderia.Find(model.id);
                foreach (var detalle in model.ListaDetalle)
                {
                    if (ModelDb.mercaderia_detalle.Any(x => x.id_detalle_mercaderia == detalle.id_detalle_mercaderia))
                    {
                        var ModelDetalle = ModelDb.mercaderia_detalle.FirstOrDefault(x => x.id_detalle_mercaderia == detalle.id_detalle_mercaderia);
                        ModelDetalle.activo = true;
                        ModelDetalle.item.cant_disponible = ModelDetalle.item.cant_disponible - ModelDetalle.cant_recibida + detalle.cant_recibida;
                        ModelDetalle.cant_recibida = detalle.cant_recibida;
                    }
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Mercaderia editada"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Deshabilitar(int Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.mercaderia.Find(Id);
                model.mercaderia_detalle.ToList().ForEach(x => { x.activo = false; x.item.cant_disponible = x.item.cant_disponible - x.cant_recibida; });
                model.activo = false;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Mercaderia Deshabilitada"), JsonRequestBehavior.AllowGet);
            }
        }

    }
}