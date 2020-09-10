using System;
using System.ComponentModel;

namespace Retailer.Desktop.UI.Models
{
    public class ProductDisplayModel : INotifyPropertyChanged
    {
        private int _inStock;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double RetailPrice { get; set; }

        public int InStock
    {
            get { return _inStock; }
            set 
            { 
                _inStock = value;
                CallPropertyChanged(nameof(InStock));
            }
        }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsTaxable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}