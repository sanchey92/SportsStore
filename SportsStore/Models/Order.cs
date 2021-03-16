using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models
{
    public class Order
    {
        [BindNever] 
        public int OrderId { get; set; }
        
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [Required(ErrorMessage = "Please press enter Name")] 
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Please enter the first address line")]

        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        
        [Required(ErrorMessage = "Please enter City name")] 
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter State name")]
        public string State { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage = "Please enter Country name")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
        
        [BindNever]
        public bool Shipped { get; set; }
    }
}