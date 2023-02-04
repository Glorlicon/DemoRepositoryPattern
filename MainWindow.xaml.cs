using DemoRepositoryPattern.Models;
using DemoRepositoryPattern.Repository;
using DemoRepositoryPattern.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoRepositoryPattern
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UnitOfWork _unitOfWork;
        public MainWindow()
        {
            InitializeComponent();
            //_unitOfWork = new UnitOfWork(new NorthwindContext());
            _productRepository = new ProductRepository(new NorthwindContext());
            _categoryRepository = new CategoryRepository(new NorthwindContext());
            //var categoriesName = _unitOfWork.CategoryRepository.GetCategoriesName();
            var categoriesName = _categoryRepository.GetCategoriesName();
            Products.ItemsSource = categoriesName;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategoryName = Products.SelectedItem.ToString();
            var fromText = From.Text;
            var toText = To.Text;

            decimal? fromPrice = null;
            decimal? toPrice = null;

            if (!string.IsNullOrEmpty(fromText))
            {
                fromPrice = decimal.Parse(fromText);
            }
            if (!string.IsNullOrEmpty(toText))
            {
                toPrice = decimal.Parse(toText);
            }
            //var products = _unitOfWork.ProductRepository.GetProductsByPriceRange(selectedCategoryName, fromPrice, toPrice);
            var products = _productRepository.GetProductsByPriceRange(selectedCategoryName, fromPrice, toPrice);
            var productsList = products.ToList();
            ProductsListBox.ItemsSource = productsList;
        }

        private void LoadData()
        {
            //var products = _unitOfWork.Repository.GetAll();
            var products = _productRepository.GetAll();
            ProductsListBox.ItemsSource = products.ToList();
        }

        private void ProductsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = ProductsListBox.SelectedItem as Product;

            if (selectedProduct != null)
            {
                IDBox.Text = selectedProduct.ProductId.ToString();
                NameBox.Text = selectedProduct.ProductName;
                CategoryBox.Text = selectedProduct.CategoryName;
                SupplierBox.Text = selectedProduct.CompanyName;
                PriceBox.Text = selectedProduct.UnitPrice.ToString();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (Product)ProductsListBox.SelectedItem;
            selectedProduct.ProductName = NameBox.Text;
            selectedProduct.CategoryName = CategoryBox.Text;
            selectedProduct.CompanyName = SupplierBox.Text;
            selectedProduct.UnitPrice = decimal.Parse(PriceBox.Text);


            _productRepository.UpdateProduct(selectedProduct);
            //_unitOfWork.ProductRepository.UpdateProduct(selectedProduct);
            //_unitOfWork.Save();
            MessageBox.Show("Product updated successfully");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this product?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedProduct = ProductsListBox.SelectedItem as Product;
                if (selectedProduct != null)
                {
                    _productRepository.DeleteProduct(selectedProduct);
                    //_unitOfWork.ProductRepository.DeleteProduct(selectedProduct);
                    // _unitOfWork.Save();
                }
                else
                {
                    MessageBox.Show("Please select a product to delete.");
                }
            }
        }

    }
}
