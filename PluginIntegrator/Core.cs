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
using System.Collections;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using OpenSDK;

namespace OpenVMSys.PluginIntegrator;

[ApiController]
[Route("api/{apiName}")]
public class Core : ControllerBase
{
    private static readonly ArrayList PluginList = OpenPluginCore.LoadAllPlugins();

    [HttpGet("")]
    public ActionResult<object?> Get(string apiName, [FromBody]JsonObject data)
    {
        foreach (var plugin in PluginList)
        {
            if (apiName!=plugin.GetType().Name || !PluginManager.IsEnabled(apiName))
            {
                return BadRequest();
            }

            var method = plugin.GetType().GetMethod("OnGet");
            var returnValue =
                method == null ? null : method.Invoke(plugin, new object?[] { data });
            return returnValue;
        }

        return BadRequest();
    }
    
    [HttpPost("")]
    public ActionResult<object?> Post(string apiName, [FromBody]JsonObject data)
    {
        OpenSDK.Logger<Core>.Info("PluginIntegrator:Post",data.ToString());
        foreach (var plugin in PluginList)
        {
            if (apiName!=plugin.GetType().Name || !PluginManager.IsEnabled(apiName))
            {
                return BadRequest();
            }

            var method = plugin.GetType().GetMethod("OnPost");
            var returnValue =
                method == null ? null : method.Invoke(plugin, new object?[] { data });
            return returnValue;
        }

        return BadRequest();
    }
    
    [HttpPatch("")]
    public ActionResult<object?> Patch(string apiName, [FromBody]JsonObject data)
    {
        foreach (var plugin in PluginList)
        {
            if (apiName!=plugin.GetType().Name || !PluginManager.IsEnabled(apiName))
            {
                return BadRequest();
            }

            var method = plugin.GetType().GetMethod("OnPatch");
            var returnValue =
                method == null ? null : method.Invoke(plugin, new object?[] { data });
            return returnValue;
        }

        return BadRequest();
    }
    
    [HttpDelete("")]
    public ActionResult<object?> Delete(string apiName, [FromBody]JsonObject data)
    {
        foreach (var plugin in PluginList)
        {
            if (apiName!=plugin.GetType().Name || !PluginManager.IsEnabled(apiName))
            {
                return BadRequest();
            }

            var method = plugin.GetType().GetMethod("OnDelete");
            var returnValue =
                method == null ? null : method.Invoke(plugin, new object?[] { data });
            return returnValue;
        }

        return BadRequest();
    }
    
    [HttpPut("")]
    public ActionResult<object?> Put(string apiName, [FromBody]JsonObject data)
    {
        foreach (var plugin in PluginList)
        {
            if (apiName!=plugin.GetType().Name || !PluginManager.IsEnabled(apiName))
            {
                return BadRequest();
            }

            var method = plugin.GetType().GetMethod("OnPut");
            var returnValue =
                method == null ? null : method.Invoke(plugin, new object?[] { data });
            return returnValue;
        }

        return BadRequest();
    }
}