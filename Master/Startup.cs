using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using common;
using Context;
using Master.API.Repository;
using DapperContext = Context.DapperContext;
using Master.Repository;
using Master.Repository.Interface;


namespace Master.API
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
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<DapperContext>();
            services.AddScoped<ICurrencyMasterRepository, CurrencyMasterRepository>();
            services.AddScoped<ICurrencyRateMasterRepository, CurrencyRateMasterRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IDepartmentMasterRepository, DepartmentMasterRepository>();
            services.AddScoped<IDesignationMasterRepository, DesignationMasterRepository>();
            services.AddScoped<ICompanyDetailsRepository, CompanyDetailsRepository>();
            services.AddScoped<ILoginDetailsRepository, LoginDetailsRepository>();
            services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();
            services.AddScoped<IGetWebMenuRepository, GetWebMenuRepository>();         
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IFileUploadRepository, FileUploadRepository>();
            services.AddScoped<IRFRDocRepository, RFRDocRepository>();
            services.AddScoped<IHighlightsRepository, HighlightsRepository>();
            services.AddScoped<IHighlightsNewRepository, HighlightsNewRepository>();
            services.AddScoped<ICreditRatingRepository, CreditRatingRepository>();
            services.AddScoped<ITBillRepository, TBillRepository>();
            services.AddScoped<ICostOfBorrowingRepository, CostOfBorrowingRepository>();
            services.AddScoped<IAccountDashboardRepository, AccountDashboardRepository>();
            services.AddScoped<IDebtRepository, DebtRepository>();
           services.AddScoped<IDebtDashboardRepository, DebtDashboardRepository>();
           services.AddScoped<ICreditDashRepository, CreditDashRepository>();

            services.AddControllers();
            services.AddMvc(options =>
            {
                //an instant  
                options.Filters.Add<ExampleFilterAttribute>();
                //By the type  
                options.Filters.Add(typeof(ExampleFilterAttribute));
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<ExampleFilterAttribute>();
            });
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins().AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
            services.AddDistributedMemoryCache(); // Add a distributed cache for session data to work
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set your desired session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            //var serviceProvider = services.BuildServiceProvider();
            //var dapperContext = serviceProvider.GetRequiredService<DapperContext>();
            //ServiceProxy.Configure(dapperContext);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }           
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSession();
        }
    }
}
