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
            var SearchQuery = from mark in db.BdMarks
                              join modification in db.BdModifications
                              on mark.IdMark equals modification.IdMark select new SearchModel{Mark=mark.Mark};
            var PModelLst = new List<ProdModel>();
            var PModelQuery = from mark in db.BdMarks
                              join model in db.BdModels on
                              mark.IdMark equals model.IdMark
                              join modification in db.BdModifications on
                              model.IdModel equals modification.IdModel  
                              select new ProdModel
                              {Mark = mark.Mark, Model = model.Model,
                              IdMark = mark.IdMark, IdModel = model.IdModel,
                              Price = modification.Price,Picture=model.Picture};
            PModelQuery.ToList();
            var PriceQuery = from m in PModelQuery 
                             group m by new { m.IdModel, m.IdMark, m.Mark, m.Model, m.Picture}
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