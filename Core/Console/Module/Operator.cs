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
using OpenSDK;
using OpenVMSys.Core.Console.Module.Security;

namespace OpenVMSys.Core.Console.Module;

public class Operator
{
    public static int Decode(string mainArg, string[] args, string[] environmentArgs)
    {
        switch (mainArg)
        {
            case "service":
            {
                if (args.Contains("start"))
                {
                    // Read all plugins in "plugins" folder
                    var pluginList = OpenPluginCore.LoadAllPlugins();
                    //Verify each plugin
                    foreach (var plugin in pluginList)
                    {
                        var pluginType = plugin.GetType();
                        OpenSDK.Logger<Operator>.Info("PluginSystem","Plugin detected", pluginType.Name);
                        
                        //Invoke "OnServiceStart" Method
                        var onServiceStart = pluginType.GetMethod("OnServiceStart");
                        if (onServiceStart != null)
                        {
                            onServiceStart.Invoke(plugin, null);
                        }
                    }
                    ServiceController.Start(environmentArgs);
                }
                else if (args.Contains("status"))
                {
                    ServiceController.Status();
                }
                break;
            }
            case "exit":
            case "quit":
                return 0;
            case "key":
            {
                if (args.Contains("add") && args.Length == 4)
                {
                    try
                    {
                        Service.Create(args[1], int.Parse(args[2]),args[3]);
                    }
                    catch
                    {
                        OpenSDK.Logger<Operator>.Error("CLI","Argument exception");
                    }
                }
                else if (args.Contains("del") && args.Length > 1)
                {
                    try
                    {
                        Service.Delete(args[1]);
                    }
                    catch
                    {
                        OpenSDK.Logger<Operator>.Error("CLI","Argument exception");
                    }
                }
                else if (args.Contains("gen") && args.Length == 3)
                {
                    try
                    {
                        var value = Service.Gen(int.Parse(args[1]), args[2]);
                        if (value == "false")
                        {
                            OpenSDK.Logger<Operator>.Error("CLI", "Fail: an unknown error occured");
                        }
                    }
                    catch
                    { 
                        OpenSDK.Logger<Operator>.Error("Argument exception");
                    }
                }
                else if (args.Contains("l"))
                {
                    Service.Get();
                }
                break;
            }
            case "plugin":
            {
                if (args.Contains("l"))
                {
                    PluginManager.ListPlugins();
                }
                else if (args.Contains("toggle"))
                {
                    PluginManager.TogglePlugin(args[1]);
                }
                break;
            }
        }
        return 1;
    }
}