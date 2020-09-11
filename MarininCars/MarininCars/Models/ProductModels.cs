using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarininCars.Models
{
    public class ProdModel
    {
        public string IdMark { get; set; }
        public string IdModel { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
    }
    public class SelectModification
    {
        public string IdMark { get; set; }
        public string IdModel { get; set; }
        public string Picture { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
    }
    public class BuyCar
    {
        public string Mark { get; set; }
        public string Modification { get; set; }
        public string Model { get; set; }
        public string IdMark { get; set;}
        public string IdModel { get; set; }
        public string IdModification { get; set; }
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