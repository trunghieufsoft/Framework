using Autofac;
using DataAccess.DbContext;
using BusinessAccess.Repository;
using BusinessAccess.UnitOfWork;
using BusinessAccess.Service.Interface;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessAccess
{
    public class ModuleInit : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbContextFactory>().As<IDesignTimeDbContextFactory<DataDbContext>>().InstancePerLifetimeScope();

            // There are 2 types of generic repository: IRepository<TEntity> and IRepository<TEntity, in TPrimaryKey>
            // That's why we need to register both generic types here
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();


            // implemented interface + instace time scope for services
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(type => typeof(BaseService).IsAssignableFrom(type))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
