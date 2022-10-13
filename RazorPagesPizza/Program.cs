using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RazorPagesPizza.Areas.Identity.Data;
using RazorPagesPizza.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("RazorPagesPizzaAuthConnection");
builder.Services.AddDbContext<RazorPagesPizzaAuth>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<RazorPagesPizzaUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<RazorPagesPizzaAuth>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailSender, EmailSender>();

var configuration = builder.Configuration;

// For setup secrets
// https://learn.microsoft.com/es-mx/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#enable-secret-storage 


// Authentication with Google
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});


// Authentication with Facebook
builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
});

// Authentication with Microsoft
builder.Services.AddAuthentication().AddMicrosoftAccount(microsoftOptions => 
{
    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapRazorPages();

app.Run();
