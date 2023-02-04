using DemoRepositoryPattern.Models;
using DemoRepositoryPattern.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRepositoryPattern.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly NorthwindContext _context;
        private readonly DbSet<Category> _entities;

        public CategoryRepository(NorthwindContext context) : base(context)
        {
            _context = context;
            _entities = context.Set<Category>();
        }

        public List<string> GetCategoriesName()
        {
            return _entities.Select(c => c.CategoryName).ToList();
        }
    }
}
