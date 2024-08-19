using System.ComponentModel.DataAnnotations;

namespace Own_Service.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
