using DemoRepositoryPattern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRepositoryPattern.Repository.Interface
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public List<string> GetCategoriesName();
    }
}
