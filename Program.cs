using BlazorGoldenZebra.Components;
using BlazorGoldenZebra.Components.Account;
using BlazorGoldenZebra.Data;
using BlazorGoldenZebra.Models;
using BlazorGoldenZebra.Utills;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var usePostgres = builder.Configuration.GetValue<bool>("UsePostgres");
var useMySql = builder.Configuration.GetValue<bool>("UseMySql");

ConfigSettings.UsePostgres = usePostgres;
ConfigSettings.UseMySql = useMySql;

var connectionString = string.Empty;

Console.WriteLine($"{DateTime.Now} Application is started");

if (usePostgres)
{
    var privateConnectionString = "Host=192.168.0.4;Password=$ga8sr533S;Username=gen_user;Database=default_db";

    Console.WriteLine($"{DateTime.Now} In connection Start");

    connectionString = builder.Configuration.GetConnectionString("GoldenZebraSecurityContextPostgres")
        ?? throw new InvalidOperationException("Connection string 'GoldenZebraSecurityContextPostgres' not found.");
    builder.Services.AddDbContextFactory<GoldenZebraSecurityContext>(options =>
        options.UseNpgsql(privateConnectionString), ServiceLifetime.Transient);

    Console.WriteLine($"{DateTime.Now} In connection Middle");

    connectionString = builder.Configuration.GetConnectionString("DefaultConnectionPostgres")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnectionPostgres' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(privateConnectionString), ServiceLifetime.Transient);

    Console.WriteLine($"{DateTime.Now} In connection End");
}
else if (useMySql)
{
    connectionString = builder.Configuration.GetConnectionString("GoldenZebraSecurityContextMySql")
    ?? throw new InvalidOperationException("Connection string 'GoldenZebraSecurityContextMySql' not found.");
    builder.Services.AddDbContextFactory<GoldenZebraSecurityContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)), ServiceLifetime.Transient);


    connectionString = builder.Configuration.GetConnectionString("DefaultConnectionMySql")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnectionMySql' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)), ServiceLifetime.Transient);
}
else
{
    connectionString = builder.Configuration.GetConnectionString("GoldenZebraSecurityContext")
    ?? throw new InvalidOperationException("Connection string 'GoldenZebraSecurityContext' not found.");
    builder.Services.AddDbContextFactory<GoldenZebraSecurityContext>(options =>
        options.UseSqlServer(connectionString), ServiceLifetime.Transient);


    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString), ServiceLifetime.Transient);
}


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
    options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("manager"));
});

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddBlazorBootstrap();

var app = builder.Build();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    Console.WriteLine($"{DateTime.Now} Database.EnsureCreated Start");

    var context = serviceScope.ServiceProvider.GetRequiredService<GoldenZebraSecurityContext>();
    context.Database.EnsureCreated();

    Console.WriteLine($"{DateTime.Now} Database.EnsureCreated End");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

var iuc = new InitialUserCreation();
await iuc.CreateRoles(app.Services);

app.Run();