using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularCustomerApp
{
    public class Customer
    {

        public void OnItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is Item newItem)
                    {
                        Console.WriteLine($"Добавлен новый товар {newItem} c идентификатором {newItem.Id}");
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems?[0] is Item oldItem)
                    {
                        Console.WriteLine($"Удален товар {oldItem} c идентификатором {oldItem.Id}");
                    }
                    break;
            }
        }
    }
}
