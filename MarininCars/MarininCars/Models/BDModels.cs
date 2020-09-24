using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace MarininCars.Models
{
    public class BdMarks
    {
        public string Id { get; set; }
        public string Mark { get; set; }
    }
    public class BdModels
    {
        public string Id { get; set;}
        public string Model { get; set;}
        public string IdMark { get; set;}
        public string Picture { get; set; }
    }
    public class BdModifications
    {
        public string Id { get; set; }
        public string Modification { get; set; }
        public string IdMark { get; set; }
        public string IdModel { get; set; }
        public decimal Veng { get; set; }
        public int Peng { get; set; }
        public string Privod { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        
    }
    public class BdOrders
    {
        public int Id { get; set; }
        public string Name_Female { get; set;}
        public string IdMark { get; set; }
        public string IdModel { get; set; }
        public string IdModification { get; set; }
        public string Phone { get; set; }
        public string Secret_Vord { get; set;}
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}