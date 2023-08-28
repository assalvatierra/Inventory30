using InvWeb.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PageConfigShared;
using PageObjectShared;
using PageConfigService;
using ObjectConfigService;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Inventory20;

using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.XtraReports.Web.Extensions;

using InvWeb.Services;

namespace InvWeb
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
            //Devexpress 
            services.AddDevExpressControls();
            services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();

            services
                .AddMvc()
                .AddNewtonsoftJson();

            //services.AddDbContext<ReportDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("ReportsDataConnectionString")));
            services.AddDbContext<ReportDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.ConfigureReportingServices(configurator => {
                configurator.ConfigureReportDesigner(designerConfigurator => {
                    designerConfigurator.RegisterDataSourceWizardConnectionStringsProvider<CustomSqlDataSourceWizardConnectionStringsProvider>();
                    designerConfigurator.EnableCustomSql();
                });
                configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
                    viewerConfigurator.UseCachedReportSourceBuilder();
                    viewerConfigurator.RegisterConnectionProviderFactory<CustomSqlDataConnectionProviderFactory>();
                });
                configurator.UseAsyncEngine();
            });

            //add sessions
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //localdb
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //identity db
            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SecurityDbContext>();

            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, UserClaimsPrincipalFactory<IdentityUser, IdentityRole>>();

            //Modules and Repos
            //services.AddScoped<ISearchServices, InvWeb.Data.Services.SearchServices>();
            services.AddScoped<ISearchServices, Inventory20.SearchService20>();
            services.AddScoped<RealSys.CoreLib.Interfaces.Reports.IReportRepo,
                RealSys.Modules.Reports.ReportRepo>();

            //Services
            services.AddScoped<RealSys.CoreLib.Services.ReportServices,
                RealSys.CoreLib.Services.ReportServices>();


            //services.Configure<PageConfigShared.TenantInfo>(Configuration.GetSection("TenantInfo"));
            string tenantcode = Configuration.GetSection("TenantInfo")["TenantCode"];
            string targetVersion = Configuration.GetSection("TenantInfo")["TargetVersion"];

            services.AddScoped<PageConfigShared.Interfaces.IPageConfigServices, PageConfigService.PageConfigServices>(x=>
                new PageConfigService.PageConfigServices(tenantcode, targetVersion)
            );

            services.AddScoped<PageObjectShared.Interfaces.IObjectConfigServices, ObjectConfigService.ObjectConfigServices> (x=>
                new ObjectConfigService.ObjectConfigServices(tenantcode, targetVersion)    
            );

            services.AddRazorPages();

            services.AddControllers(); //Adding API Controllers

            services.AddControllers().AddNewtonsoftJson();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InvApi", Version = "v1" });
            //});
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers(); //Mapping API controllers
                
                endpoints.MapControllerRoute(
                                    name: "default",
                                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
