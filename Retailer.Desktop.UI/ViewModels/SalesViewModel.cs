using Caliburn.Micro;
using Retailer.Desktop.UI.Helpers;
using Retailer.Desktop.UI.Models;
using Retailer.Desktop.UI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IConfigHelper _config;
        private IProductService _productService;
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private ProductModel _selectedProduct;
        private int _itemQuantity = 1;

        public SalesViewModel(IConfigHelper config, IProductService productService)
        {
            _config = config;
            _productService = productService;
        }

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public ProductModel SelectedProduct
        { 
            get { return _selectedProduct; }
            set 
            { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string Subtotal
        {
            get 
            {
                return CalculateSubtotal().ToString("C"); // C - currency
            }
        }

        public string Tax
        {
            get
            {                
                return CalculateTax().ToString("C"); // C - currency
            }
        }        

        public string Total
        {
            get
            {
                return CalculateTotal().ToString("C"); // C - currency
            }
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                // Is there a selection and is it in stock?
                if (ItemQuantity > 0 && SelectedProduct?.InStock >= ItemQuantity)
                    return true;

                return false;
            }
        }

        public void AddToCart()
        {
            // If the same item already exists in the cart update qty
            CartItemModel existingItem = Cart.FirstOrDefault(i => i.Product.Id == SelectedProduct.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += ItemQuantity;
                // TODO: Refactor cart refresh
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                // Add selection to cart
                Cart.Add(new CartItemModel
                {
                    Product = SelectedProduct,
                    Quantity = ItemQuantity
                });
            }

            // Prep in-stock qty
            SelectedProduct.InStock -= ItemQuantity;

            // Reset ItemQuantity back to 1
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => Cart);
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                // Is there a selection?
                if (true)
                    return true;

                return false;
            }
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanCheckout
        {
            get
            {
                // Does the cart have anything?
                if (true)
                    return true;

                return false;
            }
        }

        public void Checkout()
        {

        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productsList = (await _productService.GetAllProducts()).ToList();
            Products = new BindingList<ProductModel>(productsList);
        }

        private decimal CalculateSubtotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (decimal)item.Product.RetailPrice * item.Quantity;
            }
            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _config.GetTaxRate()/100; // 8.75/100 = 0.0875

            foreach (var item in Cart)
                if (item.Product.IsTaxable)
                    taxAmount += (decimal)item.Product.RetailPrice * item.Quantity * taxRate;

            return taxAmount;
        }

        private decimal CalculateTotal() => CalculateSubtotal() + CalculateTax(); 
    }
}
