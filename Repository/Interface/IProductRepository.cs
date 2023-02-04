using DemoRepositoryPattern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRepositoryPattern.Repository.Interface
{
    public interface IProductRepository: IRepository<Product>
    {
        IEnumerable<Product> GetProductsByPriceRange(string selectedCategoryName, decimal? fromPrice, decimal? toPrice);

        string GetSupplierCompanyName(int supplierId);

        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
