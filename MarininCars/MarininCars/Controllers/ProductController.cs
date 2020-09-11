using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarininCars.Models;
using Microsoft.SqlServer.Server;

namespace MarininCars.Controllers
{
    public class ProductController : Controller
    {
        bool ValidOrder(Orders order)
        {
            bool Notnull = order.Name_Female != null &
                order.Secret_Vord != null;
            bool OrderVal = order.Name_Female.Length <= 30 &
                order.Phone <= 999999999 & order.Phone >= 100000000 &
                order.Secret_Vord.Length == 6;
            if (Notnull && OrderVal)
                return true;
            return false;
        }
        string GetPicture(string IdMark, string IdModel)
        {
            BDContext db = new BDContext();
            var stringVar = from m in db.Modifications
                         where m.IdMark == IdMark && m.IdModel == IdModel
                         select new Modifications {Picture = m.Picture};
            string picture = stringVar.Min(m=>m.Picture);
            return picture;
        }
        decimal GetPrice(string IdMark, string IdModel)
        {
            BDContext db = new BDContext();         
            var Prices = from m in db.Modifications
                         where m.IdMark == IdMark && m.IdModel == IdModel
                         select new Modifications {Price=m.Price};
            decimal price = Prices.Min(m=>m.Price);
            return price;
        }
        public ActionResult Products(SearchModel search)
        {
            BDContext db = new BDContext();
            ProdModel PModel = new ProdModel();
            if (search.Mark == null)
                search.Mark = "";
            if (search.Model == null)
                search.Model = "";
            if (search.MaxPrice == 0)
                search.MaxPrice = 999999999999999;
            var PModelLst = new List<ProdModel>();
            var PModelQuery = from mark in db.Marks
                              join model in db.Models on
                              mark.Id equals model.IdMark
                              join modification in db.Modifications on
                              model.Id equals modification.IdModel  
                              select new ProdModel
                              {Mark = mark.Mark, Model = model.Model,
                              IdMark = mark.Id, IdModel = model.Id,
                              Price = modification.Price,
                              Picture=modification.Picture};
            var PriceQuery = from m in PModelQuery where m.Mark.Contains(search.Mark) &&
                             m.Model.Contains(search.Model) && m.Price<=search.MaxPrice && m.Price>=search.MinPrice
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
            return View();
        }
       
       
        public ActionResult Sel_Modification(string IdMark, string IdModel, string Picture, string Mark,string Model)
        {
            BDContext db = new BDContext();
            var ModificationLst = (from modification in db.Modifications
                                    where modification.IdMark == IdMark &&
                                    modification.IdModel == IdModel
                                    select new  
                                    {
                                        Id = modification.Id,
                                        Modification = modification.Modification,
                                        IdMark = modification.IdMark,
                                        IdModel = modification.IdModel,
                                        Veng = modification.Veng,
                                        Peng = modification.Peng,
                                        Privod = modification.Privod,
                                        Amount = modification.Amount,
                                        Picture = modification.Picture,
                                        Price = modification.Price,
                                    }).ToList().Select(m=> new Modifications {
                                        Id = m.Id,
                                        Modification = m.Modification,
                                        IdMark = m.IdMark,
                                        IdModel = m.IdModel,
                                        Veng = m.Veng,
                                        Peng = m.Peng,
                                        Privod = m.Privod,
                                        Amount = m.Amount,
                                        Picture = m.Picture,
                                        Price = m.Price,
                                    }).ToList();
            foreach (var mod in ModificationLst)
            {
                if (mod.Privod == "full")
                    mod.Privod = "Полный";
                if (mod.Privod == "up")
                    mod.Privod = "Передний";
                if (mod.Privod == "full")
                    mod.Privod = "Задний";
            }
            SelectModification Selmodel = new SelectModification();
            Selmodel.IdMark = IdMark;
            Selmodel.IdModel = IdModel;
            Selmodel.Picture = Picture;
            Selmodel.Mark = Mark;
            Selmodel.Model = Model;
            ViewBag.ModificationLst = ModificationLst;
            ViewBag.Selmodel = Selmodel;
            return View();
        }
        public ActionResult Buy_Car(string IdMark,string IdModel,string IdModification, string Mark, string Model, string Modification, decimal Price)
        {
            {
                BuyCar buycar = new BuyCar();
                buycar.Mark = Mark;
                buycar.Model = Model;
                buycar.Modification = Modification;
                buycar.IdMark = IdMark;
                buycar.IdModel = IdModel;
                buycar.IdModification = IdModification;
                buycar.Price = Price;
                ViewBag.buycar = buycar;
                return View();
            }        
        }
        [HttpPost]
        public ActionResult Buy_Car(Orders modell)
        {
            {
                BDContext db = new BDContext();
                modell.Date = DateTime.Today;
                if (ValidOrder(modell))
                {
                    db.Orders.Add(modell);
                    var SQLQuery = "Update Modifications SET Amount = Amount - 1 WHERE Id = '" + modell.IdModification +
                        "' AND IdMark = '" + modell.IdMark + "' AND IdModel = '" + modell.IdModel + "'";
                    db.Database.ExecuteSqlCommand(SQLQuery);
                    db.SaveChanges();
                }
                return RedirectToAction("Products");
            }
        }
    }
}