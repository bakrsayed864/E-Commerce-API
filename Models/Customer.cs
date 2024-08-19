using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Own_Service.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Job { get; set; }
        public DateTime BirthDate { get; set; }

        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser IdentityUser { get; set; }

        public virtual List<ProductReview> productReviews { get; set; }
    }
}
