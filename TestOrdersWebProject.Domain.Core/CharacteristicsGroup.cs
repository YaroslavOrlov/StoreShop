using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Domain.Core
{
    public class CharacteristicsGroup
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Group name")]
        public string Name { get; set; }

        public virtual ICollection<ProductCharacteristic> ProductCharacteristics { get; set; }
    }
}
