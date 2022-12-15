using OpenSDK;
using Path = OpenSDK.Path;
namespace OpenVMSys.Core;

public class ApplicationServer
{
    public static void StartApplicationServer(string[] args)
    {
        //Declare config variable
        var config = new OmsConfig();

        //Read config from file
        config = ConfReader<OmsConfig>.Read(config, Path.Join(("oms.conf")));

        //Create the application builder
        var builder = WebApplication.CreateBuilder(args);
        //Config the protocol to use
        builder.WebHost.UseUrls(config.SiteUrl+config.Port1+";"+config.SiteUrl+config.Port2);
        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}