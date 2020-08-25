using Caliburn.Micro;
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
        private IProductService _productService;
        private BindingList<ProductModel> _products;
        private BindingList<ProductModel> _cart;
        private int _itemQuantity;

        public SalesViewModel(IProductService productService)
        {
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

        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public string Subtotal
        {
            get 
            {
                // TODO: Replace with calculation
                return "0.00";
            }
        }

        public string Tax
        {
            get
            {
                // TODO: Replace with calculation
                return "0.00";
            }
        }

        public string Total
        {
            get
            {
                // TODO: Replace with calculation
                return "0.00";
            }
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                // Is there a selection?
                if (true)
                    return true;

                return false;
            }
        }

        public void AddToCart()
        {

        }

        public bool CanRemoveToCart
        {
            get
            {
                // Is there a selection?
                if (true)
                    return true;

                return false;
            }
        }

        public void RemoveToCart()
        {

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
    }
}
