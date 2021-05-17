using System.IO.Abstractions;
using Autofac;
using Blazored.LocalStorage;
using BlazorPro.BlazorSize;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Recyclarr.Code.Radarr;
using Recyclarr.Code.Settings;
using Recyclarr.Code.Settings.Persisters;
using Serilog;
using TrashLib.Config;
using TrashLib.Radarr;

namespace Recyclarr
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMediaQueryService();
            services.AddMudServices();
            services.AddBlazoredLocalStorage();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<RadarrAutofacModule>();

            builder.Register(_ => new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .CreateLogger())
                .As<ILogger>()
                .SingleInstance();

            builder.RegisterType<CustomFormatRepository>();
            builder.RegisterType<ResizeListener>().As<IResizeListener>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<ResourcePaths>().As<IResourcePaths>();

            // Persisters
            builder.RegisterType<SettingsPersister>().As<ISettingsPersister>();
            builder.RegisterType<AppSettingsPersister>().As<IAppSettingsPersister>();
            builder.RegisterType<RadarrConfigPersister>().As<IRadarrConfigPersister>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
