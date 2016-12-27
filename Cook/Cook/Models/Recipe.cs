using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cook.Models
{
    public class Recipe
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Recipe Name")]
        public string name { get; set; }

        [Display(Name = "Image")]
        public string img { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Procedure")]
        public string process { get; set; }

        [Required]
        [Display(Name = "Ingredients List")]
        public List<string> ingredients { get; set; }

    }
}