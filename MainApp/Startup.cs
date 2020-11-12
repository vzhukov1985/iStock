using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Services;
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

            services.AddDbContext<MainDbContext>();

            services.AddSingleton<HttpClient>();

            //TODO: Delete after deployment
/*            services.AddLiveReload();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMvc().AddRazorRuntimeCompilation();
*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            //TODO: Delete after deployment and change in _Layout.html all script links to other servers
            /* app.UseFileServer(new FileServerOptions()
             {
                 FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
                 RequestPath = "/node_modules",
                 EnableDirectoryBrowsing = false
             });*/

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            try
            {
                TelegramOperatorBot.StartBot();
            }
            catch
            {
                TelegramOperatorBot.isRunning = false;
            }
        
        }
    }
}
