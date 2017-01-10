using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cook.Models
{
    public class Recipe
    {

        public Recipe()
        {
            setType();
        }


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

        //User related properties
        public int creatorId { get; set; }

        //User related Recipe properties for User
        public bool isFavourited = false;
        public bool isEditable = false;




        public string type { get; set; }



        /*Methods*/
        public virtual void setType() { }

    }
}