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
namespace OpenVMSys.Core;
using OpenSDK;

public class ApiServer
{
    public static void StartApiServer(string[] args, OmsConfig config)
    {
        var pluginList = OpenPluginCore.LoadAllPlugins();
        foreach (var plugin in pluginList)
        {
            var pluginType = plugin.GetType();
            var onServiceStart = pluginType.GetMethod("OnServiceStart");
            if (onServiceStart==null)
            {
                Logger<ApiServer>.Error("Unexpected method declaration");
            }
            else
            {
                var returnValue = onServiceStart.Invoke(plugin, null);
                Logger<ApiServer>.Info(returnValue.ToString());
            }
        }
        //Create the application builder
        var builder = WebApplication.CreateBuilder(args);
        //Config the protocol to use
        builder.WebHost.UseUrls(config.HostAddr + ":" + config.ApiServerPort + ";" + config.HostAddr + ":" + config.ApiServerPort2);
        
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