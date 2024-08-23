using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Own_Service.Services
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly CommerceDbContext _commerceDbContext;

        public ProductReviewRepository(CommerceDbContext commerceDbContext)
        {
            this._commerceDbContext = commerceDbContext;
        }
        public int AddReview(ProductReviewDTO productDto, int productId, string userId)
        {
            int customerId = getCustomerId(userId);
            if (_commerceDbContext.Products.Find(productId) == null) //if done, this mean that the product not found
                return 0;
            else if (customerId == 0)
                return -2; //unuthorized user
            ProductReview productReview = GetById(productId, customerId);
            if (productReview != null) //this user added a review for this product before
            {
                UpdateReview(productDto, productId,customerId);
                return -1;
            }
            ProductReview productReviewToSave=new ProductReview();
            productReviewToSave.ProductId=productId;
            productReviewToSave.Notes = productDto.Note;
            productReviewToSave.Rate=productDto.Rate;
            productReviewToSave.CustomerId = customerId;
            _commerceDbContext.productsReviews.Add(productReviewToSave);
            return _commerceDbContext.SaveChanges();
        }

        public int DeleteReview(int productId,string userId)
        {
            int customerId = getCustomerId(userId);
            if (customerId == 0)
                return 0;
            ProductReview productReview = _commerceDbContext.productsReviews.FirstOrDefault(p => p.CustomerId == customerId && p.ProductId == productId);
            if (productReview == null)
                return -1; //product not found
            _commerceDbContext.productsReviews.Remove(productReview);
            return _commerceDbContext.SaveChanges();
        }

        public List<ProductReviewWithCustomerNameDTO> GetAll(int productId,string userId)
        {
            int customerId =getCustomerId(userId);
            List<ProductReview> reviews = _commerceDbContext.productsReviews.Where(p => p.ProductId == productId && p.CustomerId == customerId).ToList();
            if (reviews.Count==0)
                return null;
            List<ProductReviewWithCustomerNameDTO> result = new List<ProductReviewWithCustomerNameDTO>();
            ProductReviewWithCustomerNameDTO productReview;
            foreach (var review in reviews)
            {
                productReview=new ProductReviewWithCustomerNameDTO();
                productReview.CustomerName = review.customer.Name;
                productReview.Note = review.Notes;
                productReview.Rate=review.Rate;
                result.Add(productReview);
            }
            return result;
        }
        public List<ProductReviewWithProductNameDTO> GetAll(string userId)
        {
            int customerId=getCustomerId(userId);
            var reviews = _commerceDbContext.productsReviews.Where(p => p.CustomerId == customerId).Include(p=>p.product).ToList();
            if (reviews.Count == 0)
                return null;
            List<ProductReviewWithProductNameDTO> result = new List<ProductReviewWithProductNameDTO>();
            ProductReviewWithProductNameDTO productReview;
            foreach (var review in reviews)
            {
                productReview = new ProductReviewWithProductNameDTO();
                productReview.ProductId = review.ProductId;
                productReview.ProductName = review.product.Name;
                productReview.Note = review.Notes;
                productReview.Rate = review.Rate;
                result.Add(productReview);
            }
            return result;
        }
        public ProductReview GetById(int productId,string userId)
        {
            int customerId = getCustomerId(userId);
            ProductReview productReview = _commerceDbContext.productsReviews.FirstOrDefault(p => p.ProductId == productId && p.CustomerId == customerId);
            return  productReview;
        }
        public ProductReview GetById(int productId, int customerId)
        {
            ProductReview productReview = _commerceDbContext.productsReviews.FirstOrDefault(p => p.ProductId == productId && p.CustomerId == customerId);
            return productReview;
        }
        //in case the user made a review in a product that reviewed before from the same user, then the old review will be updated with the new one
        public int UpdateReview(ProductReviewDTO productDto, int productId,string userId)
        {
            int customerId=getCustomerId(userId);
            if(customerId==0)
                return -1;//user notfound (unauthorized)
            ProductReview oldproductReview=_commerceDbContext.productsReviews.FirstOrDefault(p => p.ProductId == productId && p.CustomerId==customerId);
            oldproductReview.Notes=productDto.Note;
            oldproductReview.Rate=productDto.Rate;
            oldproductReview.CustomerId=customerId;
            oldproductReview.ProductId=productId;
            return _commerceDbContext.SaveChanges();
        }
        public int UpdateReview(ProductReviewDTO productDto, int productId, int customerId)
        {
            if (customerId == 0)
                return -1;//user notfound (unauthorized)
            ProductReview oldproductReview = _commerceDbContext.productsReviews.FirstOrDefault(p => p.ProductId == productId && p.CustomerId == customerId);
            oldproductReview.Notes = productDto.Note;
            oldproductReview.Rate = productDto.Rate;
            oldproductReview.CustomerId = customerId;
            oldproductReview.ProductId = productId;
            return _commerceDbContext.SaveChanges();
        }
        public int getCustomerId(string userId)
        {
            return _commerceDbContext.Customers.AsNoTracking().FirstOrDefault(c => c.UserId == userId).Id;
        }
    }
}
