using System.ComponentModel.DataAnnotations.Schema;

namespace Own_Service.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Notes { get; set; }
        
        [ForeignKey("product")]
        public int ProductId { get; set; }
        public   Product product { get; set; }

        [ForeignKey("customer")]
        public int CustomerId { get; set; }
        public  Customer customer { get; set; }
    }
}
