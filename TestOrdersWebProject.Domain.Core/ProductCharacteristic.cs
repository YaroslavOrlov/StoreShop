using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Domain.Core
{
    public class ProductCharacteristic
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }

        [Display(Name = "Product characteristics group")]
        public int? CharacteristicsGroupId { get; set; }
        [Display(Name = "Product characteristics group")]
        public virtual CharacteristicsGroup CharacteristicsGroup { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
