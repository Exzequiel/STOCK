using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASMUL.DB;
using CASMUL.Models.Cables;

namespace CASMUL.Controllers
{
    public class CablesController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var list = contextCm.cable.ToList().Select(x => new ListCableViewModel { Finca = x.grupo.finca.descripcion, IdCable = x.id_cable, Hectarias = x.hectaria, Acres = x.acres, Activo = x.activo, Grupo = x.grupo.descripcion }).ToList();
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                ViewBag.SelectGrupos = contextCm.grupo.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_finca.ToString(), Text = c.descripcion }).ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(CreateCableViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                ViewBag.SelectGrupos = contextCm.grupo.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_finca.ToString(), Text = c.descripcion }).ToList();
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.cable.Add(new cable {  hectaria = model.Hectarias, acres=model.Acres, activo = true, id_grupo = model.IdGrupo });
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
                ViewBag.SelectGrupos = contextCm.grupo.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_finca.ToString(), Text = c.descripcion }).ToList();
                var model = contextCm.cable.FirstOrDefault(x => x.id_cable == id);
                return View(new EditCableViewModel { IdCable = model.id_cable, Acres= model.acres, Hectarias= model.hectaria, IdGrupo = model.id_grupo });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditCableViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                ViewBag.SelectGrupos = contextCm.grupo.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_finca.ToString(), Text = c.descripcion }).ToList();
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    var modelDb = contextCm.cable.FirstOrDefault(x => x.id_cable == model.IdCable);
                    modelDb.hectaria = model.Hectarias;
                    modelDb.acres = model.Acres;
                    modelDb.id_grupo = model.IdGrupo;
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
                var modelDb = contextCm.cable.FirstOrDefault(x => x.id_cable == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}