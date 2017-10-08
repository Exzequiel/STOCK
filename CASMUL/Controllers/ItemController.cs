using CASMUL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASMUL.Models.Item;
namespace CASMUL.Controllers
{
    [Authorize(Roles = "Administrador,UsuarioFincaNormal")]
    public class ItemController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var list = contextCm.item.ToList().Select(x => new ListaItemViewModel { Unidad = x.unidad_medida.descripcion, Descripcion = x.descripcion, Categoria = x.categoria.descripcion, Activo = x.activo, IdItem = x.id_item, CantidadDisponible = x.cant_disponible, cod_item = x.cod_item }).ToList();
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
                    if(contextCm.item.Any(x=>x.cod_item == model.cod_item.Trim()))
                    {
                        ModelState.AddModelError("", "Cod Item ya existente, escriba uno diferente");
                        return View(model);

                    }

                    contextCm.item.Add(new item { descripcion = model.Descripcion, id_categoria = model.IdCategoria, id_unidad_medida = model.IdUnidad, activo = true, cant_disponible = model.CantidadDisponible, cod_item=model.cod_item});
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
                ViewBag.SelectUnidad = contextCm.unidad_medida.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_unidad_medida.ToString(), Text = c.descripcion }).ToList();
                ViewBag.SelectCategoria = contextCm.categoria.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_categoria.ToString(), Text = c.descripcion }).ToList();

                var model = contextCm.item.Find(id);
                return View(new EditItemViewModel {  IdItem = model.id_item,  Descripcion = model.descripcion, CantidadDisponible = model.cant_disponible, cod_item=model.cod_item, IdCategoria = model.id_categoria, IdUnidad = model.id_unidad_medida });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditItemViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {

                    ViewBag.SelectUnidad = contextCm.unidad_medida.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_unidad_medida.ToString(), Text = c.descripcion }).ToList();
                    ViewBag.SelectCategoria = contextCm.categoria.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_categoria.ToString(), Text = c.descripcion }).ToList();

                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.item.Where(x => x.id_item != model.IdItem).Any(x => x.cod_item == model.cod_item.Trim()))
                    {
                        ModelState.AddModelError("", "Cod item ya existente, escriba uno diferente");
                        return View(model);
                    }
                    var modelDb = contextCm.item.FirstOrDefault(x => x.id_item == model.IdItem);
                    modelDb.descripcion = model.Descripcion;
                    modelDb.id_categoria = model.IdCategoria;
                    modelDb.id_unidad_medida = model.IdUnidad;
                    modelDb.cant_disponible = modelDb.cant_disponible;
                    modelDb.cod_item = model.cod_item;
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
                var modelDb = contextCm.item.FirstOrDefault(x => x.id_item == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}