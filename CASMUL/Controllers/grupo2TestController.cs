using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CASMUL.DB;

namespace CASMUL.Controllers
{
    public class grupo2TestController : Controller
    {
        private dbcasmulEntities db = new dbcasmulEntities();

        // GET: grupo2Test
        public ActionResult Index()
        {
            var grupo = db.grupo.Include(g => g.finca);
            return View(grupo.ToList());
        }

        // GET: grupo2Test/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupo grupo = db.grupo.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // GET: grupo2Test/Create
        public ActionResult Create()
        {
            ViewBag.id_finca = new SelectList(db.finca, "id_finca", "descripcion");
            return View();
        }

        // POST: grupo2Test/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_grupo,id_finca,descripcion,activo")] grupo grupo)
        {
            if (ModelState.IsValid)
            {
                db.grupo.Add(grupo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_finca = new SelectList(db.finca, "id_finca", "descripcion", grupo.id_finca);
            return View(grupo);
        }

        // GET: grupo2Test/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupo grupo = db.grupo.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_finca = new SelectList(db.finca, "id_finca", "descripcion", grupo.id_finca);
            return View(grupo);
        }

        // POST: grupo2Test/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_grupo,id_finca,descripcion,activo")] grupo grupo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_finca = new SelectList(db.finca, "id_finca", "descripcion", grupo.id_finca);
            return View(grupo);
        }

        // GET: grupo2Test/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupo grupo = db.grupo.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // POST: grupo2Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            grupo grupo = db.grupo.Find(id);
            db.grupo.Remove(grupo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
