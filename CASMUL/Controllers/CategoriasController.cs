using CASMUL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASMUL.Models.Categorias;
namespace CASMUL.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CategoriasController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var list = contextCm.categoria.ToList().Select(x => new ListCategoriaViewModel { Descripcion = x.descripcion, Abreviatura = x.abreviatura, Activo = x.activo, IdCategoria = x.id_categoria });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateCategoriaViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.categoria.Add(new categoria { descripcion = model.Descripcion, abreviatura = model.Abreviatura, activo = true });
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
                var model = contextCm.categoria.FirstOrDefault(x => x.id_categoria == id);
                return View(new EditCategoriaViewModel { IdCategoria = model.id_categoria, Descripcion = model.descripcion, Abreviatura = model.abreviatura });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditCategoriaViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    var modelDb = contextCm.categoria.FirstOrDefault(x => x.id_categoria == model.IdCategoria);
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
                var modelDb = contextCm.categoria.FirstOrDefault(x => x.id_categoria == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}