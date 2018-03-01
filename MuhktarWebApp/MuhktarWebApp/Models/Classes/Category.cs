using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuhktarWebApp.Models.Classes
{
    public class Category
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MinWeight { get; set; }
        [Required]
        public int MaxWeight { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
