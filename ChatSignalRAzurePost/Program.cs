using ChatSignalRAzurePost.Data;
using ChatSignalRAzurePost.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string sqlCnn = builder.Configuration.GetConnectionString("Sql");
string signalrCnn = builder.Configuration.GetConnectionString("SignalR");

builder.Services.AddSignalR().AddAzureSignalR(signalrCnn);

builder.Services.AddDbContext<ChatContext>(options =>
{
    options.UseSqlServer(sqlCnn);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
