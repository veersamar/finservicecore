using Microsoft.EntityFrameworkCore;

using X.Finance.Business.Services;
using X.Finance.Business.Validations;
using X.Finance.Data.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  

builder.Services.AddControllers();
builder.Services.AddScoped<IAccountDocumentService, AccountDocumentService>();
builder.Services.AddScoped<AccountDocumentTaggingService>();
builder.Services.AddScoped<AccountDocumentValidator>();


builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.  

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
