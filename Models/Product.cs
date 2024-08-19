using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Own_Service.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("category")]
        public int CategoryId { get; set; }
        public  Category category { get; set; }
        
        public   List<ProductReview> productReviews { get; set; }
    }
}
