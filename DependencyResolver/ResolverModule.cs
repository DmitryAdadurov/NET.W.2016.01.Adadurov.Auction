using BLL;
using BLL.Interface;
using BLL.Interface.Services;
using BLL.Services;
using DAL.Concrete;
using DAL.Identity.Stores;
using DAL.Interface;
using DAL.Interface.Identity.Entities;
using DAL.Interface.Repository;
using Ninject;
using Ninject.Web.Common;
using ORM.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DependencyResolver
{
    public static class ResolveConfig
    {
        public static void ConfigurateResolverWeb(this IKernel kernel, string connectionString)
        {
            Configure(kernel, true, connectionString);
        }

        public static void ConfigurateResolverConsole(this IKernel kernel, string connectionString)
        {
            Configure(kernel, false, connectionString);
        }

        private static void Configure(IKernel kernel, bool isWeb, string connectionString)
        {
            if (isWeb)
            {
                kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
                kernel.Bind<DbContext>().To<EntityModel>().InRequestScope().WithConstructorArgument<string>(connectionString);
                kernel.Bind<IAuctionService<int>>().To<AuctionsService>().InRequestScope();
                kernel.Bind<IBidService>().To<BidService>().InRequestScope();
                kernel.Bind<ICommentService>().To<CommentService>().InRequestScope();
                kernel.Bind<IRoleService>().To<RoleService>().InRequestScope();
                kernel.Bind<ICategoryAccessor>().To<CategoriesAccessor>().InRequestScope();
            }
            else
            {
                kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();
                kernel.Bind<DbContext>().To<EntityModel>().InSingletonScope();
            }

            kernel.Bind<IUserRepository<DalUser, int>>().To<DalUserStore>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IPasswordsHasher>().To<PasswordsService>();
            kernel.Bind<IEmailSender>().To<GmailMailer>();
        }
    }
}
