using AutoMapper;
using Caliburn.Micro;
using Retailer.Core.Helpers;
using Retailer.Core.Models;
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
        private IMapper _mapper;
        private IProductService _productService;
        private ISaleService _saleService;
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;
        private int _itemQuantity = 1;

        public SalesViewModel(IConfigHelper config, IProductService productService, ISaleService saleService, IMapper mapper)
        {
            _config = config;
            _productService = productService;
            _saleService = saleService;
            _mapper = mapper;
        }

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public ProductDisplayModel SelectedProduct
        { 
            get { return _selectedProduct; }
            set 
            { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(i => i.Product.Id == SelectedProduct.Id);
            if (existingItem != null)
                existingItem.Quantity += ItemQuantity;
            else
            {
                // Add selection to cart
                Cart.Add(new CartItemDisplayModel
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
            NotifyOfPropertyChange(() => CanCheckout);
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                // Is there a selection in the cart?
                if (SelectedCartItem != null && SelectedCartItem?.Quantity > 0)
                    return true;

                return false;
            }
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.InStock += 1;
            if (SelectedCartItem.Quantity > 1)
                SelectedCartItem.Quantity -= 1;
            else
                Cart.Remove(SelectedCartItem);
             
            NotifyOfPropertyChange(() => CanAddToCart);
            NotifyOfPropertyChange(() => CanCheckout);
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanCheckout
        {
            get
            {
                // Does the cart have anything?
                if (Cart.Count > 0)
                    return true;

                return false;
            }
        }

        public async Task Checkout()
        {
            // Create SaleModel
            var sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity
                });
            }

            // POST to API
            var saleId = await _saleService.CreateSale(sale);
            // TODO: What happens next?
            var x = saleId;

            // Reset Cart
            await ResetSalesViewModel();
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productsList = await _productService.GetAllProducts();
            var products = _mapper.Map<IList<ProductDisplayModel>>(productsList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            // TODO: Force selectedCartItem to clear if necessary

            
            // Reload inventory
            await LoadProducts();

            NotifyOfPropertyChange(() => CanAddToCart);
            NotifyOfPropertyChange(() => CanCheckout);
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
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
