using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;

namespace Own_Service.Services
{
    public interface IProductReviewRepository
    {
        //for Admin
        List<ProductReviewWithCustomerNameDTO> GetAll(int ProductId,string suerId); //for specific product
        List<ProductReviewWithProductNameDTO> GetAll(string suerId); //for specific customer(all product reviews)
        ProductReview GetById(int productId,string userId); //review get per product per customer (customer id gets from the loged customer)
        int AddReview(ProductReviewDTO productDto, int productId, string usermerId); 
        int UpdateReview(ProductReviewDTO productDto, int productId,string customerId); 
        int DeleteReview(int reviewId,string userId);
        int DeleteReview(int reviewId);
        int getCustomerId(string id); 
    }
}
