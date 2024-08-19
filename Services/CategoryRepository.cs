using Microsoft.AspNetCore.Mvc;
using Own_Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Own_Service.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CommerceDbContext _commerceDbContext;

        public CategoryRepository(CommerceDbContext commerceDbContext)
        {
            this._commerceDbContext = commerceDbContext;
        }
        public List<Category> getAll()
        {
            return _commerceDbContext.Categories.ToList();
        }
        public Category getOne(int id)
        {
            return _commerceDbContext.Categories.Find(id);
        }
        public int Creat(Category category)
        {
            _commerceDbContext.Categories.Add(category);
            return _commerceDbContext.SaveChanges();
        }
        public int Update(Category category, int id)
        {
            var oldCategory = _commerceDbContext.Categories.Find(id);
            if (oldCategory == null)
                return 0;
            oldCategory.Name = category.Name;
            return _commerceDbContext.SaveChanges();
        }
        public int Delet(int id)
        {
            var category = _commerceDbContext.Categories.Find(id);
            if (category == null)
                return 0;
            _commerceDbContext.Categories.Remove(category);
            return _commerceDbContext.SaveChanges();
        }
    }
}
