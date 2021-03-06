﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASMUL.DB;
using CASMUL.Models.Grupos;

namespace CASMUL.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class GruposController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                var list = contextCm.grupo.ToList().Select(x => new ListGruposViewModel { IdGrupo =x.id_grupo, Description = x.descripcion, Activo = x.activo, Finca = x.finca.descripcion  }).ToList();
                return View(list);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                ViewBag.SelectFincas = contextCm.finca.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_finca.ToString(), Text = c.descripcion }).ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(CreateGrupoViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                ViewBag.SelectFincas = contextCm.finca.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_finca.ToString(), Text = c.descripcion }).ToList();
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.grupo.Any(x => x.descripcion == model.Description.Trim() && x.id_finca== model.IdFinca))
                    {
                        ModelState.AddModelError("", "Descripcion Grupo ya existente, escriba uno diferente");
                        return View(model);
                    }
                    contextCm.grupo.Add(new grupo { descripcion = model.Description, activo = true, id_finca = model.IdFinca });
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
                ViewBag.SelectFincas = contextCm.finca.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_finca.ToString(), Text = c.descripcion }).ToList();
                var model = contextCm.grupo.FirstOrDefault(x => x.id_grupo == id);
                return View(new EditGrupoViewModel { Description = model.descripcion, IdGrupo = model.id_grupo, IdFinca = model.id_finca });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditGrupoViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {
                    ViewBag.SelectFincas = contextCm.finca.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_finca.ToString(), Text = c.descripcion }).ToList();
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.grupo.Where(x => x.id_grupo != model.IdGrupo).Any(x => x.descripcion == model.Description.Trim() && x.id_finca==model.IdFinca))
                    {
                        ModelState.AddModelError("", "Descripcion Grupo ya existente, escriba uno diferente");
                        return View(model);
                    }
                    var modelDb = contextCm.grupo.FirstOrDefault(x => x.id_grupo == model.IdGrupo);
                    modelDb.descripcion = model.Description;
                    modelDb.id_finca = model.IdFinca;
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
                var modelDb = contextCm.grupo.FirstOrDefault(x => x.id_grupo == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}