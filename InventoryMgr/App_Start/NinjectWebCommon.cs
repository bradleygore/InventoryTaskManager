using System.Web.Http;
using System.Web.Mvc;
using InventoryMgr.Controllers;
using InventoryMgr.Models.Abstract;
using InventoryMgr.Models.Repos;
using InventoryMgr.Models;
using InventoryMgr.Plumbing;



[assembly: WebActivator.PreApplicationStartMethod(typeof(InventoryMgr.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(InventoryMgr.App_Start.NinjectWebCommon), "Stop")]

namespace InventoryMgr.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IQtyMeasurementsRepository>().To<QtyMeasurementsRepository>();
            kernel.Bind<IInventoryRepository>().To<InventoryRepository>();
            kernel.Bind<ICategoriesRepository>().To<CategoriesRepository>();
            kernel.Bind<ITaskListRepository>().To<TaskListRepository>();
            kernel.Bind<ITaskItemRepository>().To<TaskItemRepository>();
            kernel.Bind<IInventoryTaskItemRepository>().To<InventoryTaskItemRepository>();
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
        }        
    }
}
