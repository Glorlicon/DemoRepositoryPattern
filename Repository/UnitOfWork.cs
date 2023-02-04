using DemoRepositoryPattern.Models;
using DemoRepositoryPattern.Repository;
using DemoRepositoryPattern.Repository.Interface;
using System;

public class UnitOfWork : IDisposable
{
    private readonly NorthwindContext _context;
    private IRepository<Product> _genericProductRepository;
    private IRepository<Category> _genericCategoryRepository;
    private ICategoryRepository _categoryRepository;
    private IProductRepository _productRepository;


    public UnitOfWork(NorthwindContext context)
    {
        _context = context;
    }

    public IRepository<Product> GenericProductRepository
    {
        get
        {
            if (_genericProductRepository == null)
            {
                _genericProductRepository = new Repository<Product>(_context);
            }
            return _genericProductRepository;
        }
    }

    public IRepository<Category> GenericCategoryRepository
    {
        get
        {
            if (_genericCategoryRepository == null)
            {
                _genericCategoryRepository = new Repository<Category>(_context);
            }
            return _genericCategoryRepository;
        }
    }

    public IProductRepository ProductRepository
    {
        get
        {
            if (_categoryRepository == null)
            {
                _productRepository = new ProductRepository(new NorthwindContext());
            }
            return _productRepository;
        }
    }

    public ICategoryRepository CategoryRepository
    {
        get
        {
            if (_categoryRepository == null)
            {
                _categoryRepository = new CategoryRepository(new NorthwindContext());
            }
            return _categoryRepository;
        }
    }
    public void Save()
    {
        _context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}