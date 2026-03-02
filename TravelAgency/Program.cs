using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<DataContext>(options =>
{
    var serverVersion = new MySqlServerVersion(new Version(8, 0));
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion);
});


builder.Services.AddAuthentication("MyCookieAuth") 
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyCookieAuth";
        options.LoginPath = "/Account/Login";  
        options.AccessDeniedPath = "/Account/AccessDenied"; 
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
