using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagmentSystem.Models
{
    public class OrderList
    {
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        public decimal Quanity { get; set; }
        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }
        [DisplayName("Total Price")]
        public decimal TotalPrice { get; set; } 
    }
}
