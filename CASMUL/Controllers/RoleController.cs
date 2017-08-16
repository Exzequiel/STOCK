using CASMUL.DB;
using CASMUL.Models;
using CASMUL.Models.Base;
using CASMUL.Models.Roles;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Listar()
        {
            using (var context = new dbcasmulEntities())
            {
                var jsonResult = Json(context.AspNetRoles.Select(x => new CrearRolViewModel { Id = x.Id, Nombre = x.Name, Estado = x.activo??false }).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        [HttpGet]
        public ActionResult Crear()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Crear(CrearRolViewModel model)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (roleManager.RoleExists(model.Nombre.Trim())) return Json(new MensajeRespuestaViewModel { Titulo = "Crear Rol", Mensaje = "El Rol ya Existe", Estado = false }, JsonRequestBehavior.AllowGet);
            var result = roleManager.Create(new IdentityRole { Name = model.Nombre.Trim() });
            if(result.Succeeded)
            {
                using (var context = new dbcasmulEntities())
                {
                    context.AspNetRoles.FirstOrDefault(x => x.Name == model.Nombre.Trim()).activo = true;
                    context.SaveChanges();
                }

            }
            return Json(new MensajeRespuestaViewModel { Titulo = "Crear Rol", Mensaje = result.Succeeded ? "Se creo Satisfactoriamente" : "Error al crear el Rol: " + result.Errors.FirstOrDefault(), Estado = result.Succeeded }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Editar(string Id)
        {
            using (var context = new dbcasmulEntities())
            {
                var role = context.AspNetRoles.Find(Id);
                return PartialView(new CrearRolViewModel { Id = role.Id, Nombre = role.Name, EsEditar = true, Estado= role.activo??false });
            }
        }

        [HttpPost]
        public ActionResult Editar(CrearRolViewModel model)
        {
            using (var context = new dbcasmulEntities())
            {
                var rol = context.AspNetRoles.Find(model.Id);
                rol.Name = model.Nombre;
                rol.activo = model.Estado;
                context.Entry(rol).State = System.Data.Entity.EntityState.Modified;
                var result = context.SaveChanges() > 0;
                return Json(new MensajeRespuestaViewModel { Titulo = "Editar Rol", Mensaje = result ? "Se edito Satisfactoriamente" : "Error al editar ", Estado = result }, JsonRequestBehavior.AllowGet);
            }
        }
 
    }
}