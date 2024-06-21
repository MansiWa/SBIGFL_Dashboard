using Master.API.Repository;
using Context;
using Master.Repository;
using Master.Repository.Interface;
using Master.Entity;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddControllers();

builder.Services.AddScoped<ICurrencyMasterRepository,CurrencyMasterRepository>();
builder.Services.AddScoped<ICurrencyRateMasterRepository, CurrencyRateMasterRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDesignationMasterRepository, DesignationMasterRepository>();
builder.Services.AddScoped<IDepartmentMasterRepository, DepartmentMasterRepository>();
builder.Services.AddScoped<ICompanyDetailsRepository, CompanyDetailsRepository>();
builder.Services.AddScoped<ILoginDetailsRepository, LoginDetailsRepository>();
builder.Services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();
builder.Services.AddScoped<IGetWebMenuRepository, GetWebMenuRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();
builder.Services.AddScoped<IRFRDocRepository, RFRDocRepository>();
builder.Services.AddScoped<IHighlightsRepository, HighlightsRepository>();
builder.Services.AddScoped<IHighlightsNewRepository, HighlightsNewRepository>();
builder.Services.AddScoped<ICreditRatingRepository, CreditRatingRepository>();
builder.Services.AddScoped<ITBillRepository, TBillRepository>();
builder.Services.AddScoped<ICostOfBorrowingRepository, CostOfBorrowingRepository>();
builder.Services.AddScoped<IAccountDashboardRepository, AccountDashboardRepository>();
builder.Services.AddScoped<IDebtRepository, DebtRepository>();
builder.Services.AddScoped<IDebtDashboardRepository, DebtDashboardRepository>();
builder.Services.AddScoped<ICreditDashRepository, CreditDashRepository>();
builder.Services.AddScoped<IAssetsLiabilitiesRepository, AssetsLiabilitiesRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
