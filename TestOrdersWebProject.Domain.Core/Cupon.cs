using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Domain.Core
{
    public class Cupon
    {
        public int Id { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        public double Discount { get; set; }
        [Required]
        [Range(1000, 9999, ErrorMessage = "Unique code must be four values number")]
        [Display(Name = "Unique code")]
        public int UniqCode { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Time end")]
        public DateTime EndTime { get; set; }
        public bool IsUsed { get; set; }

        [Required]
        [Display(Name = "Product category")]
        public int ProductCategoryId { get; set; }
        [Display(Name = "Product category")]
        public ProductCategory ProductCategory { get; set; }
        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        [Display(Name = "Currency")]
        public Currency Currency { get; set; }

    }
}
