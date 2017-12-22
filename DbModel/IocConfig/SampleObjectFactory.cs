using DbModel.Context;
using DbModel.ServiceLayer.Interfaces;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Web;
using System;
using System.Threading;

namespace DbModel.IocConfig
{
    public static class SampleObjectFactory
    {
        private static readonly Lazy<Container> ContainerBuilder =
            new Lazy<Container>(DefaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return ContainerBuilder.Value; }
        }

        private static Container DefaultContainer()
        {
            return new Container(x =>
            {
                x.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use(() => new MyDbContext());

                x.Scan(scan =>
                {
                    scan.AssemblyContainingType<IUserService>();
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
            });
        }
    }
}
