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
using System.Diagnostics;
using OpenSDK;
// using OpenVMSys.Core.FlightServer;
using Path = OpenSDK.Path;

namespace OpenVMSys.Core.Console.Module;

public class ServiceController
{
    private static Thread? _appRef;
    private static Thread? _webRef;

    public static void Start(string[] args)
    {
        var config = new OmsConfig();
        // var flightServer = new OpenFlightServer(args);
        //Read config from file
        config = ConfReader<OmsConfig>.Read(config, Path.Join(("oms.conf")));
        _appRef = new Thread(() => ApiServer.StartApiServer(args,config));
        _webRef = new Thread(() => WebServer.StartWebServer(args,config));
        // flightServer.Run();
        _appRef.Start();
        _webRef.Start();
        Thread.Sleep(1000);
    }

    public static void Status()
    {
        if (_appRef!=null && _webRef!=null && _appRef.IsAlive && _webRef.IsAlive)
        {
            var appTread = Process.GetCurrentProcess();
            OpenSDK.Logger<ServiceController>.Info("ServiceController","Service running, press Control/Command+c to stop.");
            OpenSDK.Logger<ServiceController>.Info("ServiceController","Memory Used:",(appTread.WorkingSet64).ToString(),"MB");
        }
        else
        {
            OpenSDK.Logger<ServiceController>.Info("Operator","Service Dead");
        }
    }
}