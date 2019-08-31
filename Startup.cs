using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EF.Core.Demo.Data;
using EF.Core.Demo.Repository.Repository;
using EF.Core.Demo.Repository.UnitOfWork;
using EF.Core.Demo.Service.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EF.Core.Demo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private const string _defaultCorsPolicyName = "EF.Core.Demo.Api.Cors";

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            #region EF DbContext配置
            string dbConnstr = "server=127.0.0.1;port=3306;user id=root;password=66666666;database=userDb;persistsecurityinfo=True;SslMode=None";
            services.AddDbContextPool<UserDbContext>(options => options.UseMySQL(dbConnstr));

            string testDbConnstr = "server=127.0.0.1;port=3306;user id=root;password=6666666;database=testDb;persistsecurityinfo=True;SslMode=None";
            services.AddDbContextPool<TestDbContext>(options => options.UseMySQL(testDbConnstr));
            #endregion

            #region Api相关配置
            services.AddMvc(options =>
            {
                //options.Filters.Add 添加过滤器
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            {
                #region Api返回的json数据格式化
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";    // 默认Datetime类型的格式json返回会加一个T，如：2018-11-07T14:17:19  
                #endregion
            });
            #endregion

            #region 跨域配置
            services.AddCors(options =>
                {
                    options.AddPolicy(_defaultCorsPolicyName, builder =>
                    {
                        //builder.WithOrigins(允许跨域的地址，可读取配置 Configuration["Application:CorsOrigins"]，*也表示允许所有)

                        builder.AllowAnyOrigin() //允许任何来源的主机访问
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();//指定处理cookie
                    });
                });
            #endregion

            #region 依赖注入

            var asm = this.GetType().Assembly;

            var types = new List<Type>();

            var testsm = this.GetType().Assembly.GetReferencedAssemblies().ToList();

            var smblist = Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList();
            var smblist2 = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
            var allAssemblys = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load).Where(c => c.FullName.StartsWith("EF.Core.Demo")).ToList();
            foreach (var assembly in allAssemblys)
            {
                foreach (var assemblyDefineType in assembly.DefinedTypes)
                {
                    if (assemblyDefineType.FullName.EndsWith("AppService"))
                    {
                        types.Add(assemblyDefineType.AsType());
                    }
                }
            }
            var implementTypes = types.Where(t => t.IsClass).ToList();
            foreach (var implementType in implementTypes)
            {
                //var isGeneric = implementType.IsConstructedGenericType;
                //接口和实现的命名规则为："UserAppService"类实现了"IUserAppService"接口,你也可以自定义规则
                var interfaceType = implementType.GetInterface("I" + implementType.Name);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementType);
                }
            }
            // 泛型注入
            services.AddScoped(typeof(IBaseAppService<>), typeof(BaseAppService<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<UserDbContext>));
            services.AddScoped(typeof(IUnitOfWork<UserDbContext>), typeof(UnitOfWork<UserDbContext>));
            services.AddScoped(typeof(IUnitOfWork<TestDbContext>), typeof(UnitOfWork<TestDbContext>));
            services.AddScoped(typeof(DbContext), typeof(UserDbContext));   // DbContext默认实现为UserDbContext
            services.AddScoped(typeof(IRepository<>), typeof(EFRepositoryBase<>));

            //var containerBuilder = new ContainerBuilder();
            //containerBuilder.Populate(services);

            //containerBuilder.RegisterAssemblyTypes(Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToArray())
            //    .Where(t => t.Name.EndsWith("AppService") && !t.IsAbstract)
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();
            //containerBuilder.RegisterGeneric(typeof(BaseAppService<>)).As(typeof(IBaseAppService<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            //var container = containerBuilder.Build();
            //return new AutofacServiceProvider(container);

            //services.BuildServiceProvider(); 
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 允许跨域
            app.UseCors(_defaultCorsPolicyName);

            app.UseMvc();
        }
    }
}
