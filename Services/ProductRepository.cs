﻿using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Own_Service.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly CommerceDbContext _commerceDbContext;

        public ProductRepository(CommerceDbContext commerceDbContext)
        {
            this._commerceDbContext = commerceDbContext;
        }

        public Product Create(ProductDTO productDto)
        {
            Product product = mapToProduct(productDto);
            _commerceDbContext.Products.Add(product);
            if (_commerceDbContext.SaveChanges() == 0)
                return null;
            return product;
        }

        public int Delete(int id)
        {
            var product = _commerceDbContext.Products.Find(id);
            if (product == null)
                return 0;
            _commerceDbContext.Products.Remove(product);
            return _commerceDbContext.SaveChanges();
        }

        public List<ProductDTO> getAll()
        {
            var products = _commerceDbContext.Products.ToList();
            return populateResult(products);
        }

        public List<ProductDTO> GetByCategory(int id)
        {
            var products = _commerceDbContext.Products.Where(p => p.CategoryId == id).ToList();
            return populateResult(products);
        }

        public ProductDTO GetById(int id)
        {
            var product = _commerceDbContext.Products.Find(id);
            //if (product == null)
            //    return null;
            ProductDTO productDTO=mapToProductDTO(product);
            return productDTO;
        }

        public List<ProductDTO> GetByName(string name)
        {
            var products= _commerceDbContext.Products.Where(x => x.Name.Contains(name)).ToList();
            return populateResult(products);
        }

        public int Update(ProductDTO productDto, int id)
        {
            var Oldproduct = _commerceDbContext.Products.Find(id);
            if (Oldproduct == null)
                return 0;
            Oldproduct.CategoryId = (int)productDto.CategoryId;
            Oldproduct.Price = productDto.Price;
            Oldproduct.Quantity = productDto.Quantity;
            Oldproduct.Name = productDto.Name;
            return _commerceDbContext.SaveChanges();
        }
        private List<ProductDTO> populateResult(List<Product> products)
        {
            List<ProductDTO> result = new List<ProductDTO>();
            ProductDTO productDTO;
            foreach (var product in products)
            {
                productDTO = mapToProductDTO(product);
                result.Add(productDTO);
            }
            return result;
        }
        #region mapping DTO
        private Product mapToProduct(ProductDTO productDto)
        {
            Product product = new Product();
            product.CategoryId = (int)productDto.CategoryId;
            product.Id = productDto.Id;
            product.Price = productDto.Price;
            product.Quantity = productDto.Quantity;
            product.Name = productDto.Name;
            return product;
        }
        private ProductDTO mapToProductDTO(Product product)
        {
            ProductDTO productDto = new ProductDTO();
            productDto.CategoryId = product.CategoryId;
            productDto.Id = product.Id;
            productDto.Price = product.Price;
            productDto.Quantity = product.Quantity;
            productDto.Name = product.Name;
            return productDto;
        }
    }
        #endregion
}
