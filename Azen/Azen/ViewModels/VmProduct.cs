using Azen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azen.ViewModels
{
    public class VmProduct
    {
        public List<Product> Products { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public int PageCount { get; set; }
        public double ItemCount { get; set; } = 3;
        public int Page { get; set; } = 1;
        public int? prdId { get; set; }
        public string searchData { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
