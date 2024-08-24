using System.ComponentModel.DataAnnotations.Schema;

namespace Own_Service.Models
{
    public class UnConfirmedOrder
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("product")]
        public int PoductId { get; set; }
        public virtual Product product { get; set; }

        [ForeignKey("customer")]
        public int customerId { get; set; }
        public virtual Customer customer { get; set; }
    }
}
