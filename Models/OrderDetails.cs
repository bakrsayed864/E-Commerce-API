using System.ComponentModel.DataAnnotations.Schema;

namespace Own_Service.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("order")]
        public int OrderId { get; set; }
        public virtual  Order order { get; set; }

        [ForeignKey("product")]
        public int ProductId { get; set; }
        public virtual Product product { get; set; }
    }
}
