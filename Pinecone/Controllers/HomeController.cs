using Pinecone.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Pinecone.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index(int? id)
        {
            if (id == null || id == 0)
            {
                return View(db.FilmsSet.ToList());
            }
            else
            {
                var films = db.ZanrFilmSet.Where(f => f.Zanr_Id == id).Select(f => f.Film_Id);
                return View(db.FilmsSet.Where(f => films.Contains(f.Id)).ToList());
            }
        }

        [HttpPost]
        public ActionResult Index(string searchString)
        {
            if(searchString == string.Empty)
            {
                return View("Index");
            }

            var filmid = from g in db.GlumacsSet
                         where g.Ime == searchString
                         join gf in db.GlumacFilmSet on g.Id equals gf.Glumac_Id
                         select gf.Film_Id;

            var films = from f in db.FilmsSet
                        where f.Naslov.Contains(searchString) || f.Radnja.Contains(searchString) || filmid.Contains(f.Id)
                        select f;

            return View(films);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.films = db.FilmsSet.Find(id);

            ViewBag.Z = (from zf in db.ZanrFilmSet
                         where zf.Film_Id == id
                         join z in db.ZanrsSet on zf.Zanr_Id equals z.Id
                         select new ZanrDTO { Id = z.Id, Zanr = z.Zanr }).ToList();

            ViewBag.G = (from gf in db.GlumacFilmSet
                         where gf.Film_Id == id
                         join g in db.GlumacsSet on gf.Glumac_Id equals g.Id
                         select new GlumacDTO { Id = g.Id, Ime = g.Ime }).ToList();

            return View();

        }

        public ActionResult Create()
        {
            ViewBag.Zanr_Id = new SelectList(db.ZanrsSet, "Id", "Zanr");
            ViewBag.Glumac_Id = new SelectList(db.GlumacsSet, "Id", "Ime");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naslov,Radnja,Ocjena,Trajanje,Glumac_id,Zanr_id")] CreateFilm films)
        {
            if (ModelState.IsValid)
            {
                var film = new Films { Id = films.Id, Naslov = films.Naslov, Trajanje = films.Trajanje,
                    Ocjena = films.Ocjena, Radnja = films.Radnja };

                db.FilmsSet.Add(film);
                db.SaveChanges();

                
                int i = db.FilmsSet.Where(f => f.Naslov == film.Naslov && f.Radnja == film.Radnja && f.Trajanje == f.Trajanje).FirstOrDefault().Id;

                if (films.Zanr_id != 0)
                {
                    db.ZanrFilmSet.Add(new ZanrFilm { Film_Id = i, Zanr_Id = films.Zanr_id });
                }

                if (films.Glumac_id != 0)
                {
                    db.GlumacFilmSet.Add(new GlumacFilm { Film_Id = i, Glumac_Id = films.Glumac_id });
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(films);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Films films = db.FilmsSet.Find(id);

            if (films == null)
            {
                return HttpNotFound();
            }

            var filmdetails = new CompleteFilmModel
            {
                Id_Film = films.Id,
                Naslov = films.Naslov,
                Radnja = films.Radnja,
                Ocjena = films.Ocjena,
                Trajanje = films.Trajanje,
                Zanr = (from zf in db.ZanrFilmSet
                        where zf.Film_Id == id
                        join z in db.ZanrsSet on zf.Zanr_Id equals z.Id
                        select new ZanrDTO { Id = z.Id, Zanr = z.Zanr }).ToList(),

                Glumci = (from gf in db.GlumacFilmSet
                          where gf.Film_Id == id
                          join g in db.GlumacsSet on gf.Glumac_Id equals g.Id
                          select new GlumacDTO { Id = g.Id, Ime = g.Ime }).ToList()
            };

            return View(filmdetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Film,Naslov,Radnja,Ocjena,Trajanje,Zanr,Glumci")] CompleteFilmModel films)
        {
            if (ModelState.IsValid)
            {
                var f = db.FilmsSet.Where(x => x.Id == films.Id_Film).FirstOrDefault();
                f.Naslov = films.Naslov;
                f.Ocjena = films.Ocjena;
                f.Radnja = films.Radnja;
                f.Trajanje = (short)films.Trajanje;

                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(films);
        }

        public ActionResult EditZanr(int id)
        {
            var zanrs = (from zf in db.ZanrFilmSet
                         where zf.Film_Id == id
                         join z in db.ZanrsSet on zf.Zanr_Id equals z.Id
                         select new ZanrDTO { Id = z.Id, Zanr = z.Zanr }).ToList();

            if (zanrs == null)
            {
                return HttpNotFound();
            }

            var zIds = (from zf in db.ZanrFilmSet
                        where zf.Film_Id == id
                        select zf.Zanr_Id);

            var notInZanrs = db.ZanrsSet.Where(f => !zIds.Contains(f.Id)).ToList();

            ViewBag.Film_Id = new SelectList(db.FilmsSet, "Id", "Naslov", id.ToString());
            ViewBag.Zanr_Id = new SelectList(notInZanrs, "Id", "Zanr");

            return View(new FilmZanrEdit { Id_film = id, Zanr = zanrs });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditZanr([Bind(Include = "Film_Id,Zanr_Id")] ZanrFilm zanrFilm)
        {
            var v = db.ZanrFilmSet.Where(zf => zf.Film_Id == zanrFilm.Film_Id && zf.Zanr_Id == zanrFilm.Zanr_Id).FirstOrDefault();

            if (ModelState.IsValid && v == null && zanrFilm.Film_Id != 0 && zanrFilm.Zanr_Id != 0)
            {
                db.ZanrFilmSet.Add(zanrFilm);
                db.SaveChanges();
                return RedirectToAction("EditZanr");
            }

            return View();
        }

        public ActionResult RemoveEditZanr(int id_film, int id_zanr)
        {
            ZanrFilm zanrFilm = (from zf in db.ZanrFilmSet
                                 where zf.Film_Id == id_film && zf.Zanr_Id == id_zanr
                                 select zf).FirstOrDefault();

            if (zanrFilm== null)
            {
                return HttpNotFound();
            }

            db.ZanrFilmSet.Remove(zanrFilm);
            db.SaveChanges();
            return RedirectToAction("EditZanr", new { id = id_film });
        }


        public ActionResult EditGlumac(int id)
        {
            var glumacs = (from gf in db.GlumacFilmSet
                         where gf.Film_Id == id
                         join g in db.GlumacsSet on gf.Glumac_Id equals g.Id
                         select new GlumacDTO { Id = g.Id, Ime = g.Ime }).ToList();

            if(glumacs == null)
            {
                return HttpNotFound();
            }

            var gIds = (from gf in db.GlumacFilmSet
                        where gf.Film_Id == id
                        select gf.Glumac_Id);

            var notInGlumacs = db.GlumacsSet.Where(f => !gIds.Contains(f.Id)).ToList();

            ViewBag.Film_Id = new SelectList(db.FilmsSet, "Id", "Naslov", id.ToString());
            ViewBag.Glumac_Id = new SelectList(notInGlumacs, "Id", "Ime");

            return View(new GlumacFilmEdit { Id_film = id, Glumac = glumacs });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGlumac([Bind(Include = "Film_Id,Glumac_Id")] GlumacFilm glumacFilm)
        {

            var eg = db.GlumacFilmSet.Where(gf => gf.Film_Id == glumacFilm.Film_Id && gf.Glumac_Id == glumacFilm.Glumac_Id).FirstOrDefault();

            if (ModelState.IsValid && eg == null && glumacFilm.Film_Id != 0 && glumacFilm.Glumac_Id !=0)
            {
                db.GlumacFilmSet.Add(glumacFilm);
                db.SaveChanges();
                return RedirectToAction("EditGlumac");
            }

            return View();
        }

        public ActionResult RemoveEditGlumac(int id_film, int id_glumac)
        {
            GlumacFilm glumacFilm = (from gf in db.GlumacFilmSet
                                 where gf.Film_Id == id_film && gf.Glumac_Id== id_glumac
                                 select gf).FirstOrDefault();

            if (glumacFilm == null)
            {
                return HttpNotFound();
            }

            db.GlumacFilmSet.Remove(glumacFilm);
            db.SaveChanges();
            return RedirectToAction("EditGlumac", new { id = id_film });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Films films = db.FilmsSet.Find(id);

            if (films == null)
            {
                return HttpNotFound();
            }
            return View(films);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Films films = db.FilmsSet.Find(id);
            db.ZanrFilmSet.RemoveRange(db.ZanrFilmSet.Where(f => f.Film_Id == id));
            db.GlumacFilmSet.RemoveRange(db.GlumacFilmSet.Where(f=>f.Film_Id == id));
            db.FilmsSet.Remove(films);
            
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