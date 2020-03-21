using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JST.TPLMS.DataBase;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace JST.TPLMS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //运行时将调用此方法。 使用此方法将服务添加到容器。
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                //this lambda determines whether user consent for nonessential cookies is needed for a given request 
                //此lambda决定给定请求是否需要用户同意非必要的cookie
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<TPLMSDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TPLMSDbContext")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            return RegisterAutofac(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //运行时将调用此方法。 使用此方法配置HTTP请求管道
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //实例化Auto发出容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);
            //新模块组件注册
            builder.RegisterModule<AutofacDI>();
            //首先注册options,供DbContext服务初始化使用
            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TPLMSDbContext>();
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("TPLMSDbContext"));
                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();
            //注册DbContext
            builder.RegisterType<TPLMSDbContext>().AsSelf().InstancePerLifetimeScope();
            //创建容器
            var Container = builder.Build();
            //第三方IoC接管Core内置DI容器
            return new AutofacServiceProvider(Container);
        }
    }
}
