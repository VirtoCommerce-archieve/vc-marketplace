using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Models
{
    public class ReviewModel
    {
        public string Id { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public DateTime? Created { get; set; }

        public User Author { get; set; } 
       
    }
}