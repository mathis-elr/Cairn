using Cairn.Web.Components.ViewsModels.Sources.CompteInvestissement;
using Cairn.Web.Components.ViewsModels.Sources.CompteCourant;
using Cairn.Web.Components.ViewsModels.Authentification;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Infrastructure.APIs.YahooFinance;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.Services.Encrypt;
using Cairn.Infrastructure.Repositories;
using Cairn.Application.Interfaces.APIs;
using Cairn.Infrastructure.APIs.LogoDev;
using Cairn.Infrastructure.APIs.Powens;
using Cairn.Web.Components.ViewsModels;
using Cairn.Infrastructure.Workers;
using Microsoft.AspNetCore.Components.Authorization;
using Cairn.Application.Services;
using Cairn.Web.GestionSession;
using Cairn.Infrastructure;
using Cairn.Web.Components;
using Microsoft.EntityFrameworkCore;
using Blazored.Toast;
using ApexCharts;
using Cairn.Web.Components.ViewsModels.Sources;
using Cairn.Infrastructure.APIs.TradeRepublicLogo;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "fr-FR" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<CairnDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

// options
builder.Services.Configure<CryptOptions>(builder.Configuration.GetSection("Security"));
builder.Services.Configure<PowensApiOptions>(builder.Configuration.GetSection("PowensApi"));
builder.Services.Configure<YahooFinanceApiOptions>(builder.Configuration.GetSection("YahooFinanceApi"));
builder.Services.Configure<LogoDevApiOptions>(builder.Configuration.GetSection("LogoDevApi"));
builder.Services.Configure<TradeRepublicLogoApiOptions>(builder.Configuration.GetSection("TradeRepublicLogoApi"));


// services
builder.Services.AddScoped<IPositionInvestissementService, PositionInvestissementService>();
builder.Services.AddScoped<IHistoriqueSoldeCompteService, HistoriqueSoldeCompteService>();
builder.Services.AddScoped<ISynchronisationPowensService, SynchronisationPowensService>();
builder.Services.AddScoped<IUtilisateurPowensService, UtilisateurPowensService>();
builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();
builder.Services.AddScoped<IMacroCategorieService, MacroCategorieService>();
builder.Services.AddScoped<IMicroCategorieService, MicroCategorieService>();
builder.Services.AddScoped<ICompteBanqueService, CompteBanqueService>();
builder.Services.AddScoped<IFluxBancaireService, FluxBancaireService>();
builder.Services.AddScoped<IBanqueService, BanqueService>();
builder.Services.AddScoped<IActifService, ActifService>();
builder.Services.AddScoped<ICryptService, CryptService>();

builder.Services.AddScoped<ITradeRepublicLogoApiService, TradeRepublicLogoApiService>();
builder.Services.AddScoped<IYahooFinanceApiService, YahooFinanceApiService>();
builder.Services.AddHttpClient<IPowensApiService, PowensApiService>();
builder.Services.AddScoped<ILogoDevApiService, LogoDevApiService>();


// views models
builder.Services.AddScoped<CompteInvestissementViewModel>();
builder.Services.AddScoped<CompteCourantViewModel>();
builder.Services.AddScoped<InscriptionViewModel>();
builder.Services.AddScoped<PatrimoineViewModel>();
builder.Services.AddScoped<ConnexionViewModel>();
builder.Services.AddScoped<DashboardViewModel>();
builder.Services.AddScoped<SourcesViewModel>();


// repositories
builder.Services.AddScoped<IPositionInvestissementRepository, PositionInvestissementRepository>();
builder.Services.AddScoped<IHistoriqueSoldeCompteRepository, HistoriqueSoldeCompteRepository>();
builder.Services.AddScoped<IUtilisateurPowensRepository, UtilisateurPowensRepository>();
builder.Services.AddScoped<IMacroCategorieRepository, MacroCategorieRepository>();
builder.Services.AddScoped<IMicroCategorieRepository, MicroCategorieRepository>();
builder.Services.AddScoped<ICompteBanqueRepository, CompteBanqueRepository>();
builder.Services.AddScoped<IFluxBancaireRepository, FluxBancaireRepository>();
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<IBanqueRepository, BanqueRepository>();
builder.Services.AddScoped<IActifRepository, ActifRepository>();


// workers
builder.Services.AddHostedService<SynchroPowensWorker>();


builder.Services.AddAuthentication("Manual")
    .AddCookie("Manual", options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorizationCore();

// 2. On garde ton Provider et ton LocalStorage
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<SessionService>();


builder.Services.AddApexCharts();
builder.Services.AddBlazoredToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseRequestLocalization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();
