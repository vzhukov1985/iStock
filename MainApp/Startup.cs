using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DbCore;
using DbCore.Models;
using MainApp.Controllers.Pricelists;
using MainApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using Westwind.AspNetCore.LiveReload;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MainApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson();

            services.AddDbContext<MainDbContext>(options => options.UseMySql(SettingsService.GetDbConnectionString()));

            services.AddSingleton<HttpClient>();
            services.AddSingleton<ControllersManagerService>();

            //TODO: Delete after deployment
            //services.AddLiveReload();
            //services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddMvc().AddRazorRuntimeCompilation();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              HttpClient hc,
                              ControllersManagerService cManager,
                              IHostApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            ExchangeRatesService.UpdateRatesAsync(hc);

            appLifetime.ApplicationStarted.Register(() => OnStarted(hc, cManager));

            try
            {
                TelegramOperatorBotService.StartBot();
            }
            catch
            {
                TelegramOperatorBotService.isRunning = false;
            }
        }

        public void OnStarted(HttpClient hc, ControllersManagerService cManager)
        {
            foreach (var controller in cManager.Controllers)
            {
                //hc.PostAsJsonAsync($"https://localhost:{CoreSettings.HttpsPort}/{controller.ControllerName}/setControllerId", controller.Id.ToString()).Wait();
            }
        }
    }
}
