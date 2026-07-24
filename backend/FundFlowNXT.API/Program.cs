using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<FundFlowNXT.API.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FundFlowNXT.API.Services.AnomalyDetectionService>();
builder.Services.AddScoped<FundFlowNXT.API.Repositories.IGrantRepository, FundFlowNXT.API.Repositories.GrantRepository>();
builder.Services.AddScoped<FundFlowNXT.API.Services.IGrantService, FundFlowNXT.API.Services.GrantService>();
builder.Services.AddScoped<FundFlowNXT.API.Repositories.ITreasuryRepository, FundFlowNXT.API.Repositories.TreasuryRepository>();
builder.Services.AddScoped<FundFlowNXT.API.Services.ITreasuryService, FundFlowNXT.API.Services.TreasuryService>();
builder.Services.AddScoped<FundFlowNXT.API.Repositories.IAccountsPayableRepository, FundFlowNXT.API.Repositories.AccountsPayableRepository>();
builder.Services.AddScoped<FundFlowNXT.API.Services.IAccountsPayableService, FundFlowNXT.API.Services.AccountsPayableService>();
builder.Services.AddScoped<FundFlowNXT.API.Repositories.IBudgetRepository, FundFlowNXT.API.Repositories.BudgetRepository>();
builder.Services.AddScoped<FundFlowNXT.API.Services.IBudgetService, FundFlowNXT.API.Services.BudgetService>();
builder.Services.AddScoped<FundFlowNXT.API.Repositories.IJournalEntryRepository, FundFlowNXT.API.Repositories.JournalEntryRepository>();
builder.Services.AddScoped<FundFlowNXT.API.Services.IJournalEntryService, FundFlowNXT.API.Services.JournalEntryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FundFlowNXT.API.Data.AppDbContext>();
    FundFlowNXT.API.Data.DbSeeder.Seed(context);
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();