using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarininCars.Models;

namespace MarininCars.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BDContext db = new BDContext();
            var SearchLst = new List<SearchModel>();
            var SearchQuery = from mark in db.Marks join modification in db.Modifications
                              on mark.Id equals modification.IdMark select new SearchModel{Mark=mark.Mark};
            var PModelLst = new List<ProdModel>();
            var PModelQuery = from mark in db.Marks
                              join model in db.Models on
                              mark.Id equals model.IdMark
                              join modification in db.Modifications on
                              model.Id equals modification.IdModel  
                              select new ProdModel
                              {Mark = mark.Mark, Model = model.Model,
                              IdMark = mark.Id, IdModel = model.Id,
                              Price = modification.Price};
            var PriceQuery = from m in PModelQuery 
                             group m by new { m.IdModel, m.IdMark, m.Mark, m.Model, m.Picture }
                             into g select new ProdModel
                             {
                                 Mark = g.Key.Mark,
                                 Model = g.Key.Model,
                                 IdMark = g.Key.IdMark,
                                 IdModel = g.Key.IdModel,
                                 Price = g.Min(m => m.Price),
                                 Picture = g.Key.Picture
                             };
            PriceQuery.ToList();
            PModelLst.AddRange(PriceQuery.Distinct());
            ViewBag.PModelLst = PModelLst;
            SearchLst.AddRange(SearchQuery.Distinct());
            ViewBag.SearchLst = SearchLst;
            return View();
        }
    }
}