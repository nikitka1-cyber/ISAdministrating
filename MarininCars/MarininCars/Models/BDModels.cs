using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarininCars.Models
{
    public class BdMarks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdMark { get; set; }
        public string Mark { get; set; }
    }
    public class BdModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 1)]
        public int IdModel { get; set;}
        public string Model { get; set;}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 2)]
        public int IdMark { get; set;}
        public string Picture { get; set; }
    }
    public class BdModifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 1)]
        public int IdModification { get; set; }
        public string Modification { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 2)]
        public int IdMark { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 3)]
        public int IdModel { get; set; }
        public decimal Veng { get; set; }
        public int Peng { get; set; }
        public string Privod { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
    public class BdOrders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order =1)]
        public int IdOrder { get; set; }
        public string Name_Female { get; set;}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 2)]
        public int IdMark { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 3)]
        public int IdModel { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 4)]
        public int IdModification { get; set; }
        public string Phone { get; set; }
        public string Secret_Vord { get; set;}
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}