using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using LibTads.EntityFrameworkCore;
using LibTads.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace LibTads.Web.Tests
{
    [DependsOn(
        typeof(LibTadsWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class LibTadsWebTestModule : AbpModule
    {
        public LibTadsWebTestModule(LibTadsEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibTadsWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(LibTadsWebMvcModule).Assembly);
        }
    }
}