using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;

namespace Own_Service.Services
{
    public interface IProductRepository
    {
        List<ProductDTO> getAll();
        ProductDTO GetById(int id);
        List<ProductDTO> GetByName(string name);
        List<ProductDTO> GetByCategory(int id);
        Product Create(ProductDTO productDto);
        int Update(ProductDTO productDto,int id);
        int Delete(int id);

    }
}