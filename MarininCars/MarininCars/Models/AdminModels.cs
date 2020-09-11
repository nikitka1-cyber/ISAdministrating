using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarininCars.Models
{
    public class InsModel
    {
        public string Mark { get; set; }
        public string Modell { get; set; }
        public string IdMark { get; set; }
        public string Modification { get; set; }
        public string IdModel { get; set; }
        public decimal Veng { get; set; }
        public int Peng { get; set; }
        public string Privod { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
    }
    public class SelModel
    {
        public string Id { get; set; }
        public string IdMark { get; set; }
        public string Model { get; set; }
        public string Mark { get; set; }
    }
    public class SelModification
    {
        public string Id { get; set; }
        public string IdModel { get; set; }
        public string IdMark { get; set; }
        public string Model { get; set; }
        public string Mark { get; set; }
        public string Modification { get; set; }
        public decimal Veng { get; set; }
        public int Peng { get; set; }
        public string Privod { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
    }
    public class SelOrder 
    {
        public int Id { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public string Modification { get; set; }
        public string Name_Female { get; set; }
        public int Phone { get; set; }
        public string Secret_Vord { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}