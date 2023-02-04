using DemoRepositoryPattern.Models;
using DemoRepositoryPattern.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoRepositoryPattern.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly NorthwindContext _context;
        private readonly DbSet<Product> _entities;

        public ProductRepository(NorthwindContext context)
            : base(context)
        {
            _context = context;
            _entities = context.Set<Product>();
        }

        public IEnumerable<Product> GetProductsByPriceRange(string selectedCategoryName, decimal? fromPrice, decimal? toPrice)
        {
            var products = (from p in _context.Products
                            join c in _context.Categories on p.CategoryId equals c.CategoryId
                            join s in _context.Suppliers on p.SupplierId equals s.SupplierId
                            where c.CategoryName == selectedCategoryName
                            select new Product
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                CategoryId = p.CategoryId,
                                CategoryName = c.CategoryName,
                                UnitPrice = p.UnitPrice,
                                SupplierId = p.SupplierId,
                                CompanyName = s.CompanyName
                            });
            if (fromPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice >= fromPrice);
            }
            if (toPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice <= toPrice);
            }

            return products.AsEnumerable();
        }

        public string GetSupplierCompanyName(int supplierId)
        {
            return _context.Suppliers.Where(s => s.SupplierId == supplierId).Select(s => s.CompanyName).FirstOrDefault();
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
            {
                MessageBox.Show("No product selected to update");
                return;
            }
            if (string.IsNullOrEmpty(product.ProductName))
            {
                MessageBox.Show("Product Name cannot be empty");
                return;
            }
            if (string.IsNullOrEmpty(product.CategoryName))
            {
                MessageBox.Show("Category Name cannot be empty");
                return;
            }
            if (string.IsNullOrEmpty(product.CompanyName))
            {
                MessageBox.Show("Supplier Company Name cannot be empty");
                return;
            }
            if (product.UnitPrice <= 0)
            {
                MessageBox.Show("Unit Price must be greater than 0");
                return;
            }

            var existingProduct = _context.Products.Find(product.ProductId);
            if (existingProduct == null)
            {
                MessageBox.Show("Product does not exist in the database");
                return;
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.CategoryName = product.CategoryName;
            existingProduct.CompanyName = product.CompanyName;
            existingProduct.UnitPrice = product.UnitPrice;

            _context.SaveChanges();
        }
        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    }
