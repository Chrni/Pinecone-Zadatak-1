using Pinecone.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Pinecone.Controllers
{
    public class ZanrsController : ApplicationController
    {
        public ActionResult Index()
        {
            return View(db.ZanrsSet.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zanrs zanrs = db.ZanrsSet.Find(id);
            if (zanrs == null)
            {
                return HttpNotFound();
            }
            return View(zanrs);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Zanr")] Zanrs zanrs)
        {
            if (ModelState.IsValid)
            {
                db.ZanrsSet.Add(zanrs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zanrs zanrs = db.ZanrsSet.Find(id);
            if (zanrs == null)
            {
                return HttpNotFound();
            }
            return View(zanrs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Zanr")] Zanrs zanrs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zanrs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(zanrs);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zanrs zanrs = db.ZanrsSet.Find(id);
            if (zanrs == null)
            {
                return HttpNotFound();
            }
            return View(zanrs);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Zanrs zanrs = db.ZanrsSet.Find(id);
            db.ZanrFilmSet.RemoveRange(db.ZanrFilmSet.Where(f => f.Zanr_Id == id));
            db.ZanrsSet.Remove(zanrs);
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
