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
        bool ValidMark(Marks mark)
        {
            if (mark.Mark != null & mark.Mark.Length <= 30)
                if (db.Marks.Count(m => m.Mark == mark.Mark) == 0)
                    return true;
            return false;
        }
        bool ValidModel(Models.Models model)
        {
            bool Notnull = model.IdMark != null && model.Model != null;
            bool ModelVal = model.Model.Length <= 30;
            if (Notnull && ModelVal)
                if (db.Models.Count(m => m.IdMark == model.IdMark & m.Model == model.Model) == 0)
                    return true;
            return false;
        }
        bool ValidModification(Modifications modification)
        {
            bool Notnull = modification.IdMark != null &
                modification.IdModel != null &
                modification.Modification != null &
                modification.Privod != null;
            bool ModifVal = modification.Modification.Length <= 30 &
                 modification.Veng <= (decimal)9.9 & modification.Veng >= (decimal)0 &
                 modification.Peng > 0 &
                 modification.Privod.Length >= 4 &
                 modification.Price <= 999999999 & modification.Price >= 0;
            if (Notnull && ModifVal)
                if (db.Modifications.Count(m => m.IdMark == modification.IdMark &
                 m.IdModel == modification.IdModel & m.Modification == modification.Modification) == 0)
                    return true;
            return false;
        }
        bool ValidOrder(Orders order)
        {
            bool Notnull = order.Name_Female != null &
                order.Secret_Vord != null;
            bool OrderVal = order.Name_Female.Length <= 30 &
                order.Phone.Length==11 &
                order.Secret_Vord.Length == 6;
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
            var MarkLst = new List<Marks>();
            var ModelLst = new List<Models.Models>();
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
        public ActionResult Insert(InsModel model)
        {
            var values = Request.Form;
            string Id;
            if (model.Mark != null)
            {
                Marks mark = new Marks();
                if (db.Marks.Count() > 0)
                {
                    Id = db.Marks.Max(m => m.Id);
                    mark.Id = IDtostr(Int32.Parse(Id) + 1);
                }
                else
                    mark.Id = "0001";
                    mark.Mark = model.Mark;
                if (ValidMark(mark))
                {
                    db.Marks.Add(mark);
                    db.SaveChanges();
                    ViewBag.VMark = "Марка вставленна";
                }
                else
                    ViewBag.VMark = "Неправильный формат данных";
            }
            if (model.Modell != null)
            {
                Models.Models _model = new Models.Models();
                if (db.Models.Count() > 0)
                {
                    Id = db.Models.Max(m => m.Id);
                    _model.Id = IDtostr(Int32.Parse(Id) + 1);
                }
                else
                    _model.Id = "0001";
                _model.Model = model.Modell;
                _model.IdMark = model.IdMark;
                _model.Picture = model.Picture;    
                if (ValidModel(_model))
                {
                    db.Models.Add(_model);
                    db.SaveChanges();
                    ViewBag.VModel = "Модель вставлена";
                }
                else
                    ViewBag.VModel = "Неправильный формат данных";
            }
            if (model.Modification != null)
            {
                Modifications modification = new Modifications();
                if (db.Modifications.Count() > 0)
                {
                    Id = db.Modifications.Max(m => m.Id);
                    modification.Id = IDtostr(Int32.Parse(Id) + 1);
                }
                else
                    modification.Id = "0001"; 
                    modification.IdMark = model.IdMark;
                    modification.IdModel = model.IdModel;
                    modification.Modification = model.Modification;
                    modification.Peng = model.Peng;
                    modification.Price = model.Price;
                    modification.Privod = model.Privod;
                    modification.Veng = model.Veng;
                    modification.Amount = model.Amount;
                if (ModelState.IsValid)
                {
                    db.Modifications.Add(modification);
                    db.SaveChanges();
                    ViewBag.VModification = "Модификация вставлена";
                }
                else
                    ViewBag.VModification = "Неправильный формат данных";
            }
            var MarkLst = new List<Marks>();
            var ModelLst = new List<Models.Models>();
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
            var ModelsQuery = from Mark in db.Marks join Model in db.Models on
                              Mark.Id equals Model.IdMark
                              select new SelModel { Model = Model.Model, Mark = Mark.Mark,
                              Id = Model.Id, IdMark = Mark.Id,Picture=Model.Picture};
            var ModificationsQuery = from Mark in db.Marks join Model in db.Models on
                                     Mark.Id equals Model.IdMark
                                     join Modification in db.Modifications
                                     on Model.Id equals Modification.IdModel
                                     select new SelModification { Id = Modification.Id, Mark = Mark.Mark,
                                     Model = Model.Model, Veng = Modification.Veng, Peng = Modification.Peng,
                                     Modification = Modification.Modification, Privod = Modification.Privod,
                                     Amount = Modification.Amount, IdModel = Model.Id, IdMark = Mark.Id,
                                     Price = Modification.Price};
            var OrdersQuery = from mark in db.Marks join model in db.Models on mark.Id equals
                              model.IdMark join modification in db.Modifications on model.Id
                              equals modification.IdModel join order in db.Orders on modification.Id
                              equals order.IdModification select new SelOrder 
                              {Id=order.Id, Mark=mark.Mark,Model=model.Model,Modification=modification.Modification,
                              Date=order.Date,Name_Female=order.Name_Female,Price=order.Price,
                              Phone=order.Phone,Secret_Vord=order.Secret_Vord};
            var MarksLst = new List<Marks>();
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
            Marks mark = db.Marks.Find(Id);
            db.Marks.Remove(mark);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Model(string Id, string IdMark)
        {
            Models.Models model = db.Models.FirstOrDefault(m => m.Id == Id && m.IdMark == IdMark);
            db.Models.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Modification(string Id, string IdMark, string IdModel)
        {
            Modifications modification = db.Modifications.FirstOrDefault(m => m.Id == Id && m.IdMark == IdMark && m.IdModel == IdModel);
            db.Modifications.Remove(modification);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Delete_Order(int Id)
        {
            Orders order = db.Orders.Find(Id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Edit_Mark(string Id)
        {
            Marks mark = db.Marks.Find(Id);
            ViewBag.mark = mark;
            return View(mark);
        }
        [HttpPost]
        public ActionResult Edit_Mark(Marks model)
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
            var MarkLst = new List<Marks>();
            var MarkQuery = from d in db.Marks
                            orderby d.Mark
                            select d;
            MarkLst.AddRange(MarkQuery.Distinct());
            ViewBag.MarkLst = MarkLst;
            Models.Models model = db.Models.FirstOrDefault(m => m.Id == Id && m.IdMark == IdMark);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit_Model(Models.Models modell)
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
            var MarkLst = new List<Marks>();
            var ModelLst = new List<Models.Models>();
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
            Modifications modification = db.Modifications.FirstOrDefault(m => m.Id == Id && m.IdMark == IdMark && m.IdModel == IdModel);
            return View(modification);
        }
        [HttpPost]
        public ActionResult Edit_Modification(Modifications model)
        {
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Select");
        }
        public ActionResult Edit_Order(int id)
        {
            Orders order = db.Orders.Find(id);
            ViewBag.order = order;
            return View(order);
        }
        [HttpPost]
        public ActionResult Edit_Order(Orders order)
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