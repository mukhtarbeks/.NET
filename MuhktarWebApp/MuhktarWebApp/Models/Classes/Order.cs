using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MuhktarWebApp.Models.Classes
{
    public enum CustomerType
    {
        [Display(Name = "Individual")]
        Individual, 
        [Display(Name = "Entity")]
        Entity
    }
    public enum State
    {
        Pending,
        Completed,
        Canceled,
        Refunded,
        Awaiting
    }
    public class Order
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        public string Customer { get; set; }
        [EnumDataType(typeof(CustomerType))]
        public CustomerType CustomerType { get; set; }
        [Required]
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public virtual Route Route { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public State State { get; set; }
        public double Price { get; set; }
        public double CashBack { get; set; }
    }
}
