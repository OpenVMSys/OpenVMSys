using OpenSDK;
using Path = OpenSDK.Path;
namespace OpenVMSys.Core;

public static class OAuthServer
{
    public static void StartOAuthServer(string[] args)
    {
        //Declare config variable
        var config = new OmsConfig();

//Read config from file
        config = ConfReader<OmsConfig>.Read(config, Path.Join(("oms.conf")));
        var authServerBuilder = WebApplication.CreateBuilder(args);
        
        authServerBuilder.WebHost.UseUrls(config.SiteUrl + "31701");
        
        authServerBuilder.Services.AddRazorPages();
        var authApp = authServerBuilder.Build();
        
        if (authApp.Environment.IsDevelopment())
        {
            authApp.UseExceptionHandler("/Error");
            authApp.UseHsts();
        }
        
        authApp.UseHttpsRedirection();
        authApp.UseStaticFiles();
        authApp.UseAuthentication();
        authApp.UseRouting();
        authApp.MapRazorPages();
        
        authApp.Run();
    }
}