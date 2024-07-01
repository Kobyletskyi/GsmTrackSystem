using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyResolver.Ninject
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IDeviceLogic>().To<DeviceLogic>();
            ninjectKernel.Bind<IUnitOfWorkRepositories>().To<Repositories.EntityFramework.UnitOfWork.Repositories>();
            ninjectKernel.Bind<DbContext>().To<MyContext>();
        }
    }
}
