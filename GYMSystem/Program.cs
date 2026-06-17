
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystemG01.BLL.Mapping;
using GymManagementSystemG01.BLL.Services.Classes;
using GymManagementSystemG01.BLL.Services.Interfaces;
using GymManagementSystemG01.DAL.Repositories.Classes;
using GymManagementSystemG01.DAL.Repositories.Interfaces;
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
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ISessionService, SessionService>();
// Program.cs

// Register your Session Repository (assuming it's Scoped like your UnitOfWork)
builder.Services.AddScoped<ISessionRepository, SessionRepository>();

builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
#endregion
var app = builder.Build();

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
