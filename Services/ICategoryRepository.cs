using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;

namespace Own_Service.Services
{
    public interface ICategoryRepository
    {
        List<Category> getAll();
        CategoryDTO getOne(int id);
        int Creat(CategoryDTO categorydto);
        int Delet(int id);
        int Update(Category category, int id);
    }
}