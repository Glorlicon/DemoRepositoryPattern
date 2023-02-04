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
            //Unit of work approach
            //_unitOfWork = new UnitOfWork(new NorthwindContext());
            //var categoriesName = _unitOfWork.CategoryRepository.GetCategoriesName();

            //Repository approach
            _productRepository = new ProductRepository(new NorthwindContext());
            _categoryRepository = new CategoryRepository(new NorthwindContext());
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

            //Unit of work approach
            //var products = _unitOfWork.ProductRepository.GetProductsByPriceRange(selectedCategoryName, fromPrice, toPrice);

            //Repository approach
            var products = _productRepository.GetProductsByPriceRange(selectedCategoryName, fromPrice, toPrice);


            var productsList = products.ToList();
            ProductsListBox.ItemsSource = productsList;
        }

        private void LoadData()
        {
            //Unit of work approach
            //var products = _unitOfWork.Repository.GetAll();

            //Repository approach
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

            //Repository approach
            _productRepository.UpdateProduct(selectedProduct);

            //Unit of work approach
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
                    //Repository approach
                    _productRepository.DeleteProduct(selectedProduct);

                    //Unit of work approach
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
