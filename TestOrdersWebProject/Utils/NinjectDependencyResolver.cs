using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject.Web.Common;
using TestOrdersWebProject.Domain.Core.Context;
using TestOrdersWebProject.Infrastructure.Data.Repositories;
using TestOrdersWebProject.Infrastructure.Interfaces.Repositories;
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Services.Interfaces;
using TestOrdersWebProject.Infrastructure.Bussiness;

namespace TestOrdersWebProject.Utils
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IDbContext>().To<ApplicationDbContext>();
            kernel.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind<IDiscountProduct>().To<MakeProductDiscount>();
            kernel.Bind<IPurchase>().To<MakeProductPurchase>();
        }
    }
}