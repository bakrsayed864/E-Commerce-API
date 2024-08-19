using Own_Service.Models;
using System.Collections.Generic;

namespace Own_Service.Services
{
    public interface ICategoryRepository
    {
        List<Category> getAll();
        Category getOne(int id);
        int Creat(Category category);
        int Delet(int id);
        int Update(Category category, int id);
    }
}