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
using Microsoft.AspNetCore.Mvc;
using OpenVMSys.Core;

namespace OpenVMSys.Components;

[ApiController]
[Route("plugins")]
public class Plugins : ControllerBase
{
    [HttpGet]
    public string GetAllPlugin()
    {
        var config = new OmsConfig();
        config = OpenSDK.ConfReader<OmsConfig>.Read(config, OpenSDK.Path.Join("oms.conf"));
        var result = "[{\n";
        var list = OpenSDK.OpenPluginCore.LoadAllPlugins();
        foreach (var o in list)
        {
            result += $"\"\"name:\":{o.GetType().Name}\"\n\"url\":\"{config.HostAddr}/api/{o.GetType().Name}\"\n";
        }

        result += "}]";
        return result;
    }
}