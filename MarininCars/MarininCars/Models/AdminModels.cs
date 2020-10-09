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
        public int IdMark { get; set; }
        public string Modification { get; set; }
        public int IdModel { get; set; }
        public decimal Veng { get; set; }
        public int Peng { get; set; }
        public string Privod { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
    }
    public class SelModel
    {
        public int Id { get; set; }
        public int IdMark { get; set; }
        public string Model { get; set; }
        public string Mark { get; set; }
        public string Picture { get; set; }
    }
    public class SelModification
    {
        public int Id { get; set; }
        public int IdModel { get; set; }
        public int IdMark { get; set; }
        public string Model { get; set; }
        public string Mark { get; set; }
        public string Modification { get; set; }
        public decimal Veng { get; set; }
        public int Peng { get; set; }
        public string Privod { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
    public class SelOrder 
    {
        public int Id { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public string Modification { get; set; }
        //public string Phone { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}