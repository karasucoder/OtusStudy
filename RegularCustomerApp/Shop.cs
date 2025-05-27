using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularCustomerApp
{
    public class Shop
    {
        private ObservableCollection<Item> _items = new ObservableCollection<Item>();

        public ObservableCollection<Item> Items { get; }

        public void Add(Item item) 
        {
     
        }

        public void Remove(Item item) { }
    }
}
