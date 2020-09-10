using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        private int _quantity;

        public ProductDisplayModel Product { get; set; }
              
        public int Quantity
        {
            get { return _quantity; }
            set 
            { 
                _quantity = value;
                CallPropertyChanged(nameof(Quantity));
                CallPropertyChanged(nameof(DisplayText));
            }
        }

        public string DisplayText
        {
            get
            {
                return $"{Product.Name} ({Quantity})";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
