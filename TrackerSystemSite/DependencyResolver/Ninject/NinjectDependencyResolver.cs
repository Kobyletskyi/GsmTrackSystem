using BusinessLayer;
using Ninject;
using Ninject.Syntax;
using RepositoriesLayer.DataBase;
using RepositoriesLayer.Repositories;
using RepositoriesLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace DependencyResolver.Ninject
{
    //// Provides a Ninject implementation of IDependencyScope
    //// which resolves services using the Ninject container.
    //public class NinjectDependencyScope : IDependencyScope
    //{
    //    IResolutionRoot resolver;

    //    public NinjectDependencyScope(IResolutionRoot resolver)
    //    {
    //        this.resolver = resolver;
    //    }

    //    public object GetService(Type serviceType)
    //    {
    //        if (resolver == null)
    //            throw new ObjectDisposedException("this", "This scope has been disposed");

    //        return resolver.TryGet(serviceType);
    //    }

    //    public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
    //    {
    //        if (resolver == null)
    //            throw new ObjectDisposedException("this", "This scope has been disposed");

    //        return resolver.GetAll(serviceType);
    //    }

    //    public void Dispose()
    //    {
    //        IDisposable disposable = resolver as IDisposable;
    //        if (disposable != null)
    //            disposable.Dispose();

    //        resolver = null;
    //    }
    //}

    //// This class is the resolver, but it is also the global scope
    //// so we derive from NinjectScope.
    //public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    //{
    //    IKernel kernel;

    //    public NinjectDependencyResolver(IKernel kernel)
    //        : base(kernel)
    //    {
    //        this.kernel = kernel;
    //    }

    //    public IDependencyScope BeginScope()
    //    {
    //        return new NinjectDependencyScope(kernel.BeginBlock());
    //    }

    //}

    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolver;

        internal NinjectDependencyScope(IResolutionRoot resolver)
        {
            Contract.Assert(resolver != null);

            this.resolver = resolver;
        }

        public void Dispose()
        {
            IDisposable disposable = resolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            resolver = null;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.GetAll(serviceType);
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }
    }

    public static class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            try
            {
                //kernel.Bind<IAuthService>().To<AuthService>();
                kernel.Bind<IUnitOfWorkRepositories>().To<Repositories>();
                kernel.Bind<IBusinessLogic>().To<BusinessLogic>();
                kernel.Bind<DbContext>().To<DbScheme>();
                return kernel;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
