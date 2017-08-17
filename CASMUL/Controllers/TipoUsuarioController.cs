using CASMUL.Models.TipoUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASMUL.DB;
namespace CASMUL.Controllers
{
    public class TipoUsuarioController : Controller
    {
        // GET: TipoUsuario
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetListTipoUsuario()
        {
            using (var contextCm = new dbcasmulEntities())
            {
                return Json(contextCm.tipo_usuario.ToList().Select(x => new ListTipoUsuarioViewModel { Description = x.descripcion, Activo = x.activo, IdTipoUsuario = x.id_tipo_usuario }));
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateTipoUsuarioViewModel model)
        {
            using (var contextCm = new dbcasmulEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.tipo_usuario.Add(new tipo_usuario { descripcion = model.Description, activo = true });
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
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);

                }

            }
        }
    }
}