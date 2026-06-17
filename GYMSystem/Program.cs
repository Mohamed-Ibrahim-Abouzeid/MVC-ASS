
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystemG01.BLL.Services.Classes;
using GymManagementSystemG01.BLL.Services.Interfaces;
using GymManagementSystemG01.PL;
using GYMSystem.DAL.DBContexts;
using GYMSystem.DAL.Repositories.Classes;
using GYMSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GYMDBContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    );

#region Services

builder.Services.AddScoped(
    typeof(GymManagementSystemG01.DAL.Repositories.Interfaces.IGenericRepository<>),
    typeof(GymManagementSystemG01.DAL.Repositories.Classes.GenericRepository<>)
);
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ITrainerService, TrainerService>();

#endregion
var app = builder.Build();
await app.MigrateAndSeedAsync();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
