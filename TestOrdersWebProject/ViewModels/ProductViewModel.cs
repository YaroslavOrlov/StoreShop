using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestOrdersWebProject.Domain.Core;

namespace TestOrdersWebProject.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int? CharacteristicsGroupId { get; set; }
        public int[] Characteristics { get; set; }
        public int[] Categories { get; set; }

    }
}