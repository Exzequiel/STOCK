using CASMUL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASMUL.Models.UnidadDeMedida;
namespace CASMUL.Controllers
{
    public class UnidadDeMedidaController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var list = contextCm.unidad_medida.ToList().Select(x => new ListUnidadViewModel { Descripcion = x.descripcion, Abreviatura = x.abreviatura, Activo = x.activo, IdUnidad = x.id_unidad_medida });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateUnidadViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.unidad_medida.Add(new unidad_medida { descripcion = model.Descripcion, abreviatura = model.Abreviatura, activo = true });
                    var result = contextCm.SaveChanges() > 0;
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);

                }

            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var model = contextCm.unidad_medida.FirstOrDefault(x => x.id_unidad_medida == id);
                return View(new EditUnidadViewModel { IdUnidad = model.id_unidad_medida, Descripcion = model.descripcion, Abreviatura = model.abreviatura });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditUnidadViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    var modelDb = contextCm.unidad_medida.FirstOrDefault(x => x.id_unidad_medida == model.IdUnidad);
                    modelDb.descripcion = model.Descripcion;
                    modelDb.abreviatura = model.Abreviatura;
                    var result = contextCm.SaveChanges() > 0;
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se ha detectado ningun cambio !!");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);

                }
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var modelDb = contextCm.unidad_medida.FirstOrDefault(x => x.id_unidad_medida == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}