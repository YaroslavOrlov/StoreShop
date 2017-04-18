using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOrdersWebProject.Domain.Core.Context;
using TestOrdersWebProject.Services.DTO;

namespace TestOrdersWebProject.Services.Interfaces
{
    public interface IPurchase
    {
        bool MakePurchase(PurchaseDTO purchase);
    }
}
