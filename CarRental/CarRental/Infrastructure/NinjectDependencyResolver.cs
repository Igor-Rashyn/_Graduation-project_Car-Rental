using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRental.Models;
using CarRental.Repositories;
using CarRental.WebUI.Models;
using Ninject;
using Ninject.Web.Common;

namespace CarRental.Infrastructure
{
    public class NinjectDependencyResolver: IDependencyResolver
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
            kernel.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));//.InRequestScope();
            //.WithPropertyValue("Context", new BookContext());
            kernel.Bind(typeof(IListViewModel<>)).To(typeof(ListViewModel<>));              
        }


    }
}