using Pinecone.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Pinecone.Controllers
{
    public class GlumacsController : ApplicationController
    {
        public ActionResult Index()
        {
            return View(db.GlumacsSet.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Glumacs glumacs = db.GlumacsSet.Find(id);
            if (glumacs == null)
            {
                return HttpNotFound();
            }
            return View(glumacs);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ime")] Glumacs glumacs)
        {
            if (ModelState.IsValid)
            {
                db.GlumacsSet.Add(glumacs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(glumacs);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Glumacs glumacs = db.GlumacsSet.Find(id);
            if (glumacs == null)
            {
                return HttpNotFound();
            }
            return View(glumacs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ime")] Glumacs glumacs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(glumacs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(glumacs);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Glumacs glumacs = db.GlumacsSet.Find(id);
            if (glumacs == null)
            {
                return HttpNotFound();
            }
            return View(glumacs);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Glumacs glumacs = db.GlumacsSet.Find(id);
            db.GlumacFilmSet.RemoveRange(db.GlumacFilmSet.Where(f=>f.Glumac_Id==id));
            db.GlumacsSet.Remove(glumacs);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
