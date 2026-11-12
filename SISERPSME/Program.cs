using Blazored.LocalStorage;
using SisData.Data;
using SisData.Dataaccess;
using SisData.Service;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using DevExpress.XtraCharts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DevExpress.Blazor.Reporting;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.Extensions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using DevExpress.Xpo.Logger.Transport;
using Microsoft.JSInterop;
using SisData.Model;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SisCom.Services.Totp;

var builder = WebApplication.CreateBuilder(args);
//Add services to the container.

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<ISqldataacceess, Sqldataacceess>();
builder.Services.AddSingleton<IDbManager, DbManager>();
builder.Services.AddScoped<Statemanagerment>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<ISysVarManager, SysVarManager>();
builder.Services.AddSingleton<IOptionsManager, OptionsManager>();
builder.Services.AddSingleton<ISisTypeManager, SisTypeManager>();
builder.Services.AddSingleton<IDocumentProvider, DocumentProvider>();
builder.Services.AddScoped<IDxWindowService, DxWindowService>();
builder.Services.AddSingleton<ICommandManager, CommandManager>();
builder.Services.AddSingleton<ITotpSettingService, TotpSettingService>();
builder.Services.AddSingleton<IUserTotpService, UserTotpService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddSingleton<SisGlobals>();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<XmlStringLocalizer>();
builder.Services.AddDevExpressBlazor();
builder.Services.AddDevExpressBlazorReporting();
builder.Services.AddDevExpressServerSideBlazorReportViewer();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddLocalization(options => options.ResourcesPath = "Lang");
builder.Services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
builder.Services.Configure<DevExpress.Blazor.Configuration.GlobalOptions>(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
});

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(10);
        options.DisconnectedCircuitMaxRetained = 100;
        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(5);
        options.MaxBufferedUnacknowledgedRenderBatches = 20;
    });

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();
// An thong tin phien ban may chu (ZAP: "Server Leaks Version Information")
builder.WebHost.ConfigureKestrel(o => o.AddServerHeader = false);
var app = builder.Build();

// ===== Security headers (ZAP: CSP, anti-clickjacking, X-Content-Type-Options, Server leak) =====
// Dat o dau pipeline de ap cho moi response (ke ca static files).
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        var headers = context.Response.Headers;

        // Xoa header lo thong tin cong nghe/phien ban
        headers.Remove("Server");
        headers.Remove("X-Powered-By");

        // Content Security Policy. Blazor Server + DevExpress can 'unsafe-inline'/'unsafe-eval';
        // chi cho phep CDN dang dung (jsdelivr cho bootstrap, cdnjs cho font-awesome).
        if (!headers.ContainsKey("Content-Security-Policy"))
        {
            headers["Content-Security-Policy"] =
                "default-src 'self'; " +
                "base-uri 'self'; " +
                "object-src 'none'; " +
                "frame-ancestors 'self'; " +
                "img-src 'self' data: blob:; " +
                "font-src 'self' data: https://cdnjs.cloudflare.com https://cdn.jsdelivr.net; " +
                "style-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com https://cdn.jsdelivr.net; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net; " +
                "worker-src 'self' blob:; " +
                "connect-src 'self' ws: wss:;";
        }

        // Chong clickjacking (bo sung cho frame-ancestors o tren)
        if (!headers.ContainsKey("X-Frame-Options"))
            headers["X-Frame-Options"] = "SAMEORIGIN";

        // Chong MIME sniffing
        if (!headers.ContainsKey("X-Content-Type-Options"))
            headers["X-Content-Type-Options"] = "nosniff";

        // Han che ro ri referrer va quyen trinh duyet
        if (!headers.ContainsKey("Referrer-Policy"))
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        if (!headers.ContainsKey("Permissions-Policy"))
            headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";

        return Task.CompletedTask;
    });

    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");    
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseDevExpressBlazorReporting();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("vi-VN");

app.Run();