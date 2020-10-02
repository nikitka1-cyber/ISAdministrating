using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarininCars.Models
{
    public class ProdModel
    {
        public int IdMark { get; set; }
        public int IdModel { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
    }
    public class SelectModification
    {
        public int IdMark { get; set; }
        public int IdModel { get; set; }
        public string Picture { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
    }
    public class BuyCar
    {
        public string Mark { get; set; }
        public string Modification { get; set; }
        public string Model { get; set; }
        public int IdMark { get; set;}
        public int IdModel { get; set; }
        public int IdModification { get; set; }
        public decimal Price { get; set; }
    }

    public class SearchModel
    {
        public string Mark { get; set;}
        public string Model { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}