using CASMUL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASMUL.Models.Item;
namespace CASMUL.Controllers
{
    public class ItemController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var list = contextCm.item.ToList().Select(x => new ListaItemViewModel { Unidad = x.unidad_medida.descripcion, Descripcion = x.descripcion, Categoria = x.categoria.descripcion, Activo = x.activo, IdItem = x.id_item}).ToList();
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                ViewBag.SelectUnidad = contextCm.unidad_medida.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_unidad_medida.ToString(), Text = c.descripcion }).ToList();
                ViewBag.SelectCategoria = contextCm.categoria.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_categoria.ToString(), Text = c.descripcion }).ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(CreateItemVIewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                ViewBag.SelectUnidad = contextCm.unidad_medida.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_unidad_medida.ToString(), Text = c.descripcion }).ToList();
                ViewBag.SelectCategoria = contextCm.categoria.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_categoria.ToString(), Text = c.descripcion }).ToList();
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.item.Add(new item { descripcion = model.Descripcion, id_categoria = model.IdCategoria, id_unidad_medida = model.IdUnidad, activo = true});
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
        public ActionResult Delete(int id)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var modelDb = contextCm.item.FirstOrDefault(x => x.id_item == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}