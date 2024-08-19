using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Own_Service.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public  Customer Customer { get; set; }

    }
}
