using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Domain.Core
{
    public class Currency
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Rate { get; set; }
        public bool IsBase { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }

    }
}
