using System.ComponentModel.DataAnnotations;

namespace Own_Service.DTO
{
    public class ProductDTO
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public int Quantity { get; set; }
        [Required]
        public int? CategoryId { get; set; }


    }
}
