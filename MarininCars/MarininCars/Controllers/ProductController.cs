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
        bool ValidOrder(BdOrders bdorder)
        {
            bool Notnull = bdorder.Name_Female != null &
                bdorder.Secret_Vord != null;
            bool OrderVal = bdorder.Name_Female.Length <= 30 &
                bdorder.Phone.Length==11 &
                bdorder.Secret_Vord.Length == 6;
            if (Notnull && OrderVal)
                return true;
            return false;
        }
        decimal GetPrice(int IdMark, int IdModel)
        {
            BDContext db = new BDContext();         
            var Prices = from m in db.BdModifications
                         where m.IdMark == IdMark && m.IdModel == IdModel
                         select new BdModifications { Price=m.Price};
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
            var PModelQuery = from bdmark in db.BdMarks
                              join bdmodel in db.BdModels on
                              bdmark.IdMark equals bdmodel.IdMark
                              join bdmodification in db.BdModifications on
                              bdmodel.IdModel equals bdmodification.IdModel  
                              select new ProdModel
                              {Mark = bdmark.Mark, Model = bdmodel.Model,
                              IdMark = bdmark.IdMark, IdModel = bdmodel.IdModel,
                              Price = bdmodification.Price,
                              Picture= bdmodel.Picture};
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
       
       
        public ActionResult Sel_Modification(int IdMark, int IdModel, string Picture, string Mark,string Model)
        {
            BDContext db = new BDContext();
            var ModificationLst = (from bdmodification in db.BdModifications
                                   where bdmodification.IdMark == IdMark &&
                                    bdmodification.IdModel == IdModel
                                    select new  
                                    {
                                        Id = bdmodification.IdModification,
                                        Modification = bdmodification.Modification,
                                        IdMark = bdmodification.IdMark,
                                        IdModel = bdmodification.IdModel,
                                        Veng = bdmodification.Veng,
                                        Peng = bdmodification.Peng,
                                        Privod = bdmodification.Privod,
                                        Amount = bdmodification.Amount,
                                        Price = bdmodification.Price,
                                    }).ToList().Select(m=> new BdModifications
                                    {
                                        IdModification = m.Id,
                                        Modification = m.Modification,
                                        IdMark = m.IdMark,
                                        IdModel = m.IdModel,
                                        Veng = m.Veng,
                                        Peng = m.Peng,
                                        Privod = m.Privod,
                                        Amount = m.Amount,
                                        Price = m.Price,
                                    }).ToList();
            foreach (var mod in ModificationLst)
            {
                if (mod.Privod == "full")
                    mod.Privod = "Полный";
                if (mod.Privod == "up")
                    mod.Privod = "Передний";
                if (mod.Privod == "down")
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
        public ActionResult Buy_Car(int IdMark,int IdModel,int IdModification, string Mark, string Model, string Modification, decimal Price)
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
        public ActionResult Buy_Car(BdOrders modell)
        {
            {
                BDContext db = new BDContext();
                modell.Date = DateTime.Today;
                if (ValidOrder(modell))
                {
                    db.BdOrders.Add(modell);
                    var SQLQuery = "Update BdModifications SET Amount = Amount - 1 WHERE Id = '" + modell.IdModification +
                        "' AND IdMark = '" + modell.IdMark + "' AND IdModel = '" + modell.IdModel + "'";
                    db.Database.ExecuteSqlCommand(SQLQuery);
                    db.SaveChanges();
                }
                return RedirectToAction("Products");
            }
        }
    }
}