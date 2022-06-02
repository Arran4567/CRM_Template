using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bicks.Models;

namespace Bicks.Areas.ProductManagement.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}
