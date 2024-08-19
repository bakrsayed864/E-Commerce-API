using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;

namespace Own_Service.Services
{
    public interface IProductRepository
    {
        List<Product> getAll();
        ProductDTO GetById(int id);
        List<Product> GetByName(string name);
        List<Product> GetByCategory(int id);
        Product Create(ProductDTO productDto);
        int Update(ProductDTO productDto,int id);
        int Delete(int id);

    }
}