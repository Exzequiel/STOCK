using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASMUL.DB;
using CASMUL.Models.Fincas;
namespace CASMUL.Controllers
{
    public class FincasController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var list = contextCm.finca.ToList().Select(x => new ListFincasVIewModel { Description = x.descripcion, Activo =  x.activo, IdFincas = x.id_finca });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateFincasViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.finca.Add(new finca { descripcion = model.Description, activo = true });
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
                var model = contextCm.finca.FirstOrDefault(x => x.id_finca == id);
                return View(new EditFincasViewModel { IdFincas = model.id_finca, Description = model.descripcion });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditFincasViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    var modelDb = contextCm.finca.FirstOrDefault(x => x.id_finca == model.IdFincas);
                    modelDb.descripcion = model.Description;
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
                var modelDb = contextCm.finca.FirstOrDefault(x => x.id_finca == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}