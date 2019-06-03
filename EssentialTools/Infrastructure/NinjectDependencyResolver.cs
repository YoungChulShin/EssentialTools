using EssentialTools.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EssentialTools.Infrastructure
{
    // 의존성 해결자(Dependency Resolver)
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
            return kernel.Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // 의존성 체인
            // HomeController는 IValueCalculator에 의존
            // LinqValueCalculator는 IDiscountHelper에 의존
            kernel.Bind<IValueCalculator>().To<LinqValueCalculator>();
            kernel.Bind<IDiscountHelper>()
                  .To<DefaultDiscountHelper>().WithConstructorArgument("discountSizeParam", 50M);
            // 조건적 바인딩 사용
            kernel.Bind<IDiscountHelper>()
                  .To<FlexibleDiscountHelper>().WhenInjectedInto<LinqValueCalculator>();
        }
    }
}