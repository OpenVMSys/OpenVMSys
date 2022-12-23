/*
 * This file is part of OpenVMSys
 *
 * OpenVMSys
 * Copyright (c) 2015 - 2021 OpenVMSys Team
 *
 * OpenVMSys free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenVMSys is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */
using Microsoft.Extensions.FileProviders;
using Path = OpenSDK.Path;
namespace OpenVMSys.Core;

public static class WebServer
{
    public static void StartWebServer(string[] args, OmsConfig config)
    {
        var webApplicationBuilder = WebApplication.CreateBuilder(args);

        webApplicationBuilder.WebHost.UseUrls(config.HostAddr + ":" + config.WebServerPort);
        
        webApplicationBuilder.Services.AddRazorPages();

        var webServer = webApplicationBuilder.Build();
        
        if (webServer.Environment.IsDevelopment())
        {
            webServer.UseExceptionHandler("/Error");
            webServer.UseHsts();
        }
        webServer.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Join("Resource")),
            RequestPath = ""
        });
        webServer.UseHttpsRedirection();
        webServer.UseStaticFiles();
        webServer.UseAuthentication();
        webServer.UseRouting();
        webServer.MapRazorPages();
        
        webServer.Run();
    }
}