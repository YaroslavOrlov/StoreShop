using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Domain.Core
{
    public class ProductCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Display(Name = "Parent category name")]
        public int? ParentCategoryId { get; set; }
        [Display(Name = "Parent category name")]
        public ProductCategory ParentCategory { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<ProductCategory> SubCategories { get; set; }

    }
}
