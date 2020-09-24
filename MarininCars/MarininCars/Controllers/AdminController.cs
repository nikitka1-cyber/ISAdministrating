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
                if (db.Marks.Count(m => m.Mark == bdmark.Mark) == 0)
                    return true;
            return false;
        }
        bool ValidModel(BdModels bdmodel)
        {
            bool Notnull = bdmodel.IdMark != null && bdmodel.Model != null;
            bool ModelVal = bdmodel.Model.Length <= 30;
            if (Notnull && ModelVal)
                if (db.Models.Count(m => m.IdMark == bdmodel.IdMark & m.Model == bdmodel.Model) == 0)
                    return true;
            return false;
        }
        bool ValidModification(BdModifications bdmodification)
        {
            bool Notnull = bdmodification.IdMark != null &
                bdmodification.IdModel != null &
                bdmodification.Modification != null &
                bdmodification.Privod != null;
            bool ModifVal = bdmodification.Modification.Length <= 30 &
                 bdmodification.Veng <= (decimal)9.9 & bdmodification.Veng >= (decimal)0 &
                 bdmodification.Peng > 0 &
                 bdmodification.Privod.Length >= 4 &
                 bdmodification.Price <= 999999999 & bdmodification.Price >= 0;
            if (Notnull && ModifVal)
                if (db.Modifications.Count(m => m.IdMark == bdmodification.IdMark &
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
        string IDtostr(int id)
        {
            if (id < 10)
                return "000" + id.ToString();
            if (id < 100)
                return "00" + id.ToString();
            if (id < 100)
                return "0" + id.ToString();
            return id.ToString();
        }
        public ActionResult Insert()
        {
            var MarkLst = new List<BdMarks>();
            var ModelLst = new List<BdModels>();
            var MarkQuery = from d in db.Marks
                            orderby d.Mark
                            select d;
            var ModelQuery = from d in db.Models
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
            var values = Request.Form;
            string Id;
            if (insmodel.Mark != null)
            {
                BdMarks mark = new BdMarks();
                if (db.Marks.Count() > 0)
                {
                    Id = db.Marks.Max(m => m.Id);
                    mark.Id = IDtostr(Int32.Parse(Id) + 1);
                }
                else
                    mark.Id = "0001";
                    mark.Mark = insmodel.Mark;
                if (ValidMark(mark))
                {
                    db.Marks.Add(mark);
                    db.SaveChanges();
                    ViewBag.VMark = "Марка вставленна";
                }
                else
                    ViewBag.VMark = "Неправильный формат данных";
            }
            if (insmodel.Modell != null)
            {
                BdModels _model = new BdModels();
                if (db.Models.Count() > 0)
                {
                    Id = db.Models.Max(m => m.Id);
                    _model.Id = IDtostr(Int32.Parse(Id) + 1);
                }
                else
                    _model.Id = "0001";
                _model.Model = insmodel.Modell;
                _model.IdMark = insmodel.IdMark;
                _model.Picture = insmodel.Picture;    
                if (ValidModel(_model))
                {
                    db.Models.Add(_model);
                    db.SaveChanges();
                    ViewBag.VModel = "Модель вставлена";
                }
                else
                    ViewBag.VModel = "Неправильный формат данных";
            }
            if (insmodel.Modification != null)
            {
                BdModifications modification = new BdModifications();
                if (db.Modifications.Count() > 0)
                {
                    Id = db.Modifications.Max(m => m.Id);
                    modification.Id = IDtostr(Int32.Parse(Id) + 1);
                }
                else
                    modification.Id = "0001"; 
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
                    db.Modifications.Add(modification);
                    db.SaveChanges();
                    ViewBag.VModification = "Модификация вставлена";
                }
                else
                    ViewBag.VModification = "Неправильный формат данных";
            }
            var MarkLst = new List<BdMarks>();
            var ModelLst = new List<BdModels>();
            var MarkQuery = from d in db.Marks
                            orderby d.Mark
                            select d;
            var ModelQuery = from d in db.Models
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
            var MarksQuery = db.Marks.OrderBy(s => s.Mark);
            var ModelsQuery = from BdMark in db.Marks join BdModel in db.Models on
                              BdMark.Id equals BdModel.IdMark
                              select new SelModel { Model = BdModel.Model, Mark = BdMark.Mark,
                              Id = BdModel.Id, IdMark = BdMark.Id,Picture= BdModel.Picture};
            var ModificationsQuery = from BdMark in db.Marks join BdModel in db.Models on
                                     BdMark.Id equals BdModel.IdMark
                                     join Modification in db.Modifications
                                     on BdModel.Id equals Modification.IdModel
                                     select new SelModification { Id = Modification.Id, Mark = BdMark.Mark,
                                     Model = BdModel.Model, Veng = Modification.Veng, Peng = Modification.Peng,
                                     Modification = Modification.Modification, Privod = Modification.Privod,
                                     Amount = Modification.Amount, IdModel = BdModel.Id, IdMark = BdMark.Id,
                                     Price = Modification.Price};
            var OrdersQuery = from bdmark in db.Marks join bdmodel in db.Models on bdmark.Id equals
                              bdmodel.IdMark join modification in db.Modifications on bdmodel.Id
                              equals modification.IdModel join order in db.Orders on modification.Id
                              equals order.IdModification select new SelOrder 
                              {Id=order.Id, Mark= bdmark.Mark,Model= bdmodel.Model,Modification=modification.Modification,
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
        public ActionResult Delete_Mark(string Id)
        {
            BdMarks mark = db.Marks.Find(Id);
            db.Marks.Remove(mark);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Model(string Id, string IdMark)
        {
            BdModels model = db.Models.FirstOrDefault(m => m.Id == Id && m.IdMark == IdMark);
            db.Models.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Modification(string Id, string IdMark, string IdModel)
        {
            BdModifications modification = db.Modifications.FirstOrDefault(m => m.Id == Id && m.IdMark == IdMark && m.IdModel == IdModel);
            db.Modifications.Remove(modification);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Order(int Id)
        {
            BdOrders order = db.Orders.Find(Id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Edit_Mark(string Id)
        {
            BdMarks mark = db.Marks.Find(Id);
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
        public ActionResult Edit_Model(string Id, string IdMark)
        {
            var MarkLst = new List<BdMarks>();
            var MarkQuery = from d in db.Marks
                            orderby d.Mark
                            select d;
            MarkLst.AddRange(MarkQuery.Distinct());
            ViewBag.MarkLst = MarkLst;
            Models.BdModels model = db.Models.FirstOrDefault(m => m.Id == Id && m.IdMark == IdMark);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit_Model(Models.BdModels modell)
        {
            if (ValidModel(modell))
            {
                db.Entry(modell).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Select");
        }
        public ActionResult Edit_Modification(string Id, string IdMark, string IdModel)
        {
            var MarkLst = new List<BdMarks>();
            var ModelLst = new List<BdModels>();
            var MarkQuery = from d in db.Marks
                            orderby d.Mark
                            select d;
            var ModelQuery = from d in db.Models
                             orderby d.Model
                             select d;
            MarkLst.AddRange(MarkQuery.Distinct());
            ModelLst.AddRange(ModelQuery.Distinct());
            ViewBag.MarkLst = MarkLst;
            ViewBag.ModelLst = ModelLst;
            BdModifications modification = db.Modifications.FirstOrDefault(m => m.Id == Id && m.IdMark == IdMark && m.IdModel == IdModel);
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
            BdOrders order = db.Orders.Find(id);
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