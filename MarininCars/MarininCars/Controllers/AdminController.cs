using MarininCars.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarininCars.Controllers
{
    public class AdminController : Controller
    {
        BDContext db = new BDContext();
        bool ValidMark(BdMarks bdmark)
        {
            if (bdmark.Mark != null & bdmark.Mark.Length <= 30)
                if (db.BdMarks.Count(m => m.Mark == bdmark.Mark) == 0)
                    return true;
            return false;
        }
        bool ValidModel(BdModels bdmodel)
        {
            bool Notnull = bdmodel.IdMark != 0 && bdmodel.Model != null;
            bool ModelVal = bdmodel.Model.Length <= 30;
            if (Notnull && ModelVal)
                if (db.BdModels.Count(m => m.IdMark == bdmodel.IdMark & m.Model == bdmodel.Model) == 0)
                    return true;
            return false;
        }
        bool ValidModification(BdModifications bdmodification)
        {
            bool Notnull = bdmodification.IdMark != 0 &
                bdmodification.IdModel != 0 &
                bdmodification.Modification != null &
                bdmodification.Privod != null;
            bool ModifVal = bdmodification.Modification.Length <= 30 &
                 bdmodification.Veng <= (decimal)9.9 & bdmodification.Veng >= (decimal)0 &
                 bdmodification.Peng > 0 &
                 bdmodification.Privod.Length >= 4 &
                 bdmodification.Price <= 999999999 & bdmodification.Price >= 0;
            if (Notnull && ModifVal)
                if (db.BdModifications.Count(m => m.IdMark == bdmodification.IdMark &
                 m.IdModel == bdmodification.IdModel & m.Modification == bdmodification.Modification) == 0)
                    return true;
            return false;
        }
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
        
        public ActionResult Insert()
        {
            var MarkLst = new List<BdMarks>();
            var ModelLst = new List<BdModels>();
            var MarkQuery = from d in db.BdMarks
                            orderby d.Mark
                            select d;
            var ModelQuery = from d in db.BdModels
                             orderby d.Model
                             select d;
            MarkLst.AddRange(MarkQuery.Distinct());
            ModelLst.AddRange(ModelQuery.Distinct());
            ViewBag.MarkLst = MarkLst;
            ViewBag.ModelLst = ModelLst;
            ViewBag.VMark = "";
            ViewBag.VModel = "";
            ViewBag.VModification = "";
            return View();
        }
        [HttpPost]
        public ActionResult Insert(InsModel insmodel)
        {
            db.SaveChanges();
            int Id;
            if (insmodel.Mark != null)
            {
                
                BdMarks mark = new BdMarks();
                if (db.BdMarks.Count() > 0)
                {
                    mark.IdMark = db.BdMarks.Max(m => m.IdMark) +1;
                }
                else
                    mark.IdMark = 1;
                    mark.Mark = insmodel.Mark;
                if (ValidMark(mark))
                {
                    db.BdMarks.Add(mark);
                    db.SaveChanges();
                    ViewBag.VMark = "Марка вставленна";
                }
                else
                    ViewBag.VMark = "Неправильный формат данных";
            }
            if (insmodel.Modell != null)
            {
                BdModels _model = new BdModels();
                if (db.BdModels.Count() > 0)
                {
                    _model.IdModel = db.BdModels.Max(m => m.IdModel);
                }
                else
                    _model.IdModel = 1;
                _model.Model = insmodel.Modell;
                _model.IdMark = insmodel.IdMark;
                _model.Picture = insmodel.Picture;    
                if (ValidModel(_model))
                {
                    db.BdModels.Add(_model);
                    db.SaveChanges();
                    ViewBag.VModel = "Модель вставлена";
                }
                else
                    ViewBag.VModel = "Неправильный формат данных";
            }
            if (insmodel.Modification != null)
            {
                BdModifications modification = new BdModifications();
                if (db.BdModifications.Count() > 0)
                {
                    Id = db.BdModifications.Max(m => m.IdModification);
                }
                else
                    modification.IdModification = 1; 
                    modification.IdMark = insmodel.IdMark;
                    modification.IdModel = insmodel.IdModel;
                    modification.Modification = insmodel.Modification;
                    modification.Peng = insmodel.Peng;
                    modification.Price = insmodel.Price;
                    modification.Privod = insmodel.Privod;
                    modification.Veng = insmodel.Veng;
                    modification.Amount = insmodel.Amount;
                if (ModelState.IsValid)
                {
                    db.BdModifications.Add(modification);
                    db.SaveChanges();
                    ViewBag.VModification = "Модификация вставлена";
                }
                else
                    ViewBag.VModification = "Неправильный формат данных";
            }
            var MarkLst = new List<BdMarks>();
            var ModelLst = new List<BdModels>();
            var MarkQuery = from d in db.BdMarks
                            orderby d.Mark
                            select d;
            var ModelQuery = from d in db.BdModels
                             orderby d.Model
                             select d;
            MarkLst.AddRange(MarkQuery.Distinct());
            ModelLst.AddRange(ModelQuery.Distinct());
            ViewBag.MarkLst = MarkLst;
            ViewBag.ModelLst = ModelLst;
            return View();
        }

        public ActionResult Select()
        {
            var MarksQuery = db.BdMarks.OrderBy(s => s.Mark);
            var ModelsQuery = from BdMark in db.BdMarks
                              join BdModel in db.BdModels on
                              BdMark.IdMark equals BdModel.IdMark
                              select new SelModel { Model = BdModel.Model, Mark = BdMark.Mark,
                              Id = BdModel.IdModel, IdMark = BdMark.IdMark,
                                  Picture= BdModel.Picture};
            var ModificationsQuery = from BdMark in db.BdMarks
                                     join BdModel in db.BdModels on
                                     BdMark.IdMark equals BdModel.IdMark
                                     join Modification in db.BdModifications
                                     on BdModel.IdModel equals Modification.IdModel
                                     select new SelModification { Id = Modification.IdModification, Mark = BdMark.Mark,
                                     Model = BdModel.Model, Veng = Modification.Veng, Peng = Modification.Peng,
                                     Modification = Modification.Modification, Privod = Modification.Privod,
                                     Amount = Modification.Amount, IdModel = BdModel.IdModel, IdMark = BdMark.IdMark,
                                     Price = Modification.Price};
            var OrdersQuery = from bdmark in db.BdMarks
                              join bdmodel in db.BdModels on bdmark.IdMark equals
                              bdmodel.IdMark join modification in db.BdModifications on bdmodel.IdModel
                              equals modification.IdModel join order in db.BdOrders on modification.IdModification
                              equals order.IdModification select new SelOrder 
                              {Id=order.IdOrder, Mark= bdmark.Mark,Model= bdmodel.Model,Modification=modification.Modification,
                              Date=order.Date,Name_Female=order.Name_Female,Price=order.Price,
                              Phone=order.Phone,Secret_Vord=order.Secret_Vord};
            var MarksLst = new List<BdMarks>();
            var ModelsLst = new List<SelModel>();
            var ModificationsLst = new List<SelModification>();
            var OrderLst = new List<SelOrder>();
            MarksLst.AddRange(MarksQuery.Distinct());
            ModelsLst.AddRange(ModelsQuery.Distinct());
            ModificationsLst.AddRange(ModificationsQuery.Distinct());
            OrderLst.AddRange(OrdersQuery.Distinct());
            ViewBag.MarksLst = MarksLst;
            ViewBag.ModelsLst = ModelsLst;
            ViewBag.ModificationsLst = ModificationsLst;
            ViewBag.OrderLst = OrderLst;
            return View();
        }
        public ActionResult Delete_Mark(int Id)
        {
            BdMarks mark = db.BdMarks.Find(Id);
            db.BdMarks.Remove(mark);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Model(int Id, int IdMark)
        {
            BdModels model = db.BdModels.FirstOrDefault(m => m.IdModel == Id && m.IdMark == IdMark);
            db.BdModels.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Modification(int Id, int IdMark, int IdModel)
        {
            BdModifications modification = db.BdModifications.FirstOrDefault(m => m.IdModification == Id && m.IdMark == IdMark && m.IdModel == IdModel);
            db.BdModifications.Remove(modification);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Order(int Id)
        {
            BdOrders order = db.BdOrders.Find(Id);
            db.BdOrders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Edit_Mark(int Id)
        {
            BdMarks mark = db.BdMarks.Find(Id);
            ViewBag.mark = mark;
            return View(mark);
        }
        [HttpPost]
        public ActionResult Edit_Mark(BdMarks model)
        {
            if (ValidMark(model))
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Select");
        }
        public ActionResult Edit_Model(int Id, int IdMark)
        {
            var MarkLst = new List<BdMarks>();
            var MarkQuery = from d in db.BdMarks
                            orderby d.Mark
                            select d;
            MarkLst.AddRange(MarkQuery.Distinct());
            ViewBag.MarkLst = MarkLst;
            BdModels model = db.BdModels.FirstOrDefault(m => m.IdModel == Id && m.IdMark == IdMark);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit_Model(BdModels modell)
        {
            if (ValidModel(modell))
            {
                db.Entry(modell).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Select");
        }
        public ActionResult Edit_Modification(int Id, int IdMark, int IdModel)
        {
            var MarkLst = new List<BdMarks>();
            var ModelLst = new List<BdModels>();
            var MarkQuery = from d in db.BdMarks
                            orderby d.Mark
                            select d;
            var ModelQuery = from d in db.BdModels
                             orderby d.Model
                             select d;
            MarkLst.AddRange(MarkQuery.Distinct());
            ModelLst.AddRange(ModelQuery.Distinct());
            ViewBag.MarkLst = MarkLst;
            ViewBag.ModelLst = ModelLst;
            BdModifications modification = db.BdModifications.FirstOrDefault(m => m.IdModification == Id && m.IdMark == IdMark && m.IdModel == IdModel);
            return View(modification);
        }
        [HttpPost]
        public ActionResult Edit_Modification(BdModifications model)
        {
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Edit_Order(int id)
        {
            BdOrders order = db.BdOrders.Find(id);
            ViewBag.order = order;
            return View(order);
        }
        [HttpPost]
        public ActionResult Edit_Order(BdOrders order)
        {
            if (ValidOrder(order))
            {
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Select");
        }
    }
}