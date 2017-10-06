using CASMUL.DB;
using CASMUL.Models.Requisa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    [Authorize(Roles = "Administrador,UsuarioFincaPrincipal,UsuarioFincaNormal")]
    public class RequisaController : BaseController
    {
       public RequisaController()
        {
            ViewBag.ControllerName = "Requisa";
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
                var list = context.requisa.Select(x => new ListaRequisaViewModel {
                    id_requisa = x.id_requisa,
                    nro_requisa = x.nro_requisa,
                    finca = x.finca.descripcion,
                    fecha_transaccion = x.fecha_transaccion,
                    semana = x.semana,
                    periodo = x.periodo,
                    activo = x.activo,
                    confirmado = x.movimiento.Any(y=>y.activo),
                    IdFinca = x.id_finca
                }).ToList();

                if (Convert.ToInt32(getConfiguracion("Finca_IdFinca42")) != ObtenerIdFincaPorUsuario())
                {
                    var finca = ObtenerIdFincaPorUsuario();
                    list = list.Where(x => x.IdFinca == finca).ToList();
                }

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
                var list = context.requisa_detalle.Where(x => x.id_requisa == Id && x.activo).Select(x => new CrearDetalleRequisaViewModel
                {
                    id_requisa = x.id_requisa,
                    id_detail_requisa = x.id_detail_requisa,
                    id_item = x.id_item,
                    cant_enviada = x.cant_enviada,
                    cant_disponible = x.item.cant_disponible - (x.item.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.item.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0) - (x.item.requisa_detalle.Any(y => y.activo && !y.requisa.movimiento.Any(z => z.activo) && y.id_requisa!=Id) ? x.item.requisa_detalle.Where(y => y.activo && !y.requisa.movimiento.Any(z => z.activo) && y.id_requisa != Id).Sum(z => z.cant_enviada) : 0),
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
        public ActionResult Crear()
        {
            using (var context = new dbcasmulEntities())
            {
                ViewBag.ListaItem = ObtenerListaItemParaSeleccionar();
                return View(new CrearRequisaViewModel {
                    fecha_transaccion = DateTime.Now,
                    nro_requisa = getConfiguracion("CorrelativoRequisa"),
                    NombreFinca = ObtenerNombreFincaPorUsuario(),
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
                });
            }
        }

        [HttpPost]
        public ActionResult Crear(CrearRequisaViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
               var NuevaRequisa= context.requisa.Add(new requisa {
                    nro_requisa = getConfiguracion("CorrelativoRequisa"),
                    id_finca = ObtenerIdFincaPorUsuario(),
                    fecha_transaccion = DateTime.Now,
                    semana = ObtenerSemana(),
                    periodo = ObtenerPeriodo(),
                    activo = true,
                });

                foreach(var detalle in model.ListaDetalle)
                {
                    context.requisa_detalle.Add(new requisa_detalle {
                        id_requisa = NuevaRequisa.id_requisa,
                        cant_enviada = detalle.cant_enviada,
                        id_item = detalle.id_item,
                        activo = true
                    });
                }

                var resultado = context.SaveChanges() > 0;
                if (resultado) SumarCorrelativo("CorrelativoRequisa");
                return Json(EnviarResultado(resultado, "Requisa creada exitosamente"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Editar(int Id)
        {
            using(var context = new dbcasmulEntities())
            {
                ViewBag.ListaItem = ObtenerListaItemParaSeleccionar();
                var model = context.requisa.Find(Id);
                return View("Crear",new CrearRequisaViewModel {
                    id_requisa = model.id_requisa,
                    id_finca = model.id_finca,
                    fecha_transaccion = model.fecha_transaccion,
                    nro_requisa = model.nro_requisa,
                    periodo = model.periodo,
                    semana = model.semana,
                    NombreFinca = model.finca.descripcion,
                    EsEditar = true
                });
            }
        }

        [HttpPost]
        public ActionResult Editar(CrearRequisaViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var ModelDb = context.requisa.Find(model.id_requisa);
               
                ModelDb.requisa_detalle.ToList().ForEach(x => x.activo = false);
                foreach (var detalle in model.ListaDetalle)
                {
                    if (ModelDb.requisa_detalle.Any(x => x.id_detail_requisa == detalle.id_detail_requisa))
                    {
                        var ModelDetalle = ModelDb.requisa_detalle.FirstOrDefault(x => x.id_detail_requisa == detalle.id_detail_requisa);
                        ModelDetalle.activo = true;
                        ModelDetalle.cant_enviada = detalle.cant_enviada;
                    }
                    else
                    {
                        context.requisa_detalle.Add(new requisa_detalle
                        {
                            id_requisa = ModelDb.id_requisa,
                            cant_enviada = detalle.cant_enviada,
                            id_item = detalle.id_item,
                            activo = true
                        });
                    }
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Requisa editada exitosamente"), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerInfoItem(int IdItem)
        {
            using (var context = new dbcasmulEntities())
            {
                var model = context.item.Find(IdItem);
                return Json(new CrearDetalleRequisaViewModel
                {
                    id_detail_requisa = 0,
                    cant_enviada = 0,
                    id_requisa = 0,
                    id_item = IdItem,
                    cant_disponible = ObtenerCantidadDisponible(model),
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
                var model = context.requisa.Find(Id);
                model.requisa_detalle.ToList().ForEach(x => { x.activo = false; });
                model.activo = false;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Requisa Deshabilitada"), JsonRequestBehavior.AllowGet);
            }
        }




    }
}