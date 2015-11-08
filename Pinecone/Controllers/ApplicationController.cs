using Pinecone.Models;
using System.Linq;
using System.Web.Mvc;

namespace Pinecone.Controllers
{
    public abstract class ApplicationController : Controller
    {
        protected ModelFilmovaContainer db = new ModelFilmovaContainer();

        public ApplicationController()
        {
            ViewBag.Zanrs = (from z in db.ZanrsSet
                      select new ZanrFilmModel
                      {
                          Id = z.Id,
                          Zanr = z.Zanr,
                          NumOfFilms = db.ZanrFilmSet.Where(p => p.Zanr_Id.Equals(z.Id)).Count().ToString()
                      }).ToList();

            ViewBag.All = db.FilmsSet.Count().ToString();
        }
    }
}