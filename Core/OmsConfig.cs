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

public class OmsConfig
{
    public string ApiServerPort { get; set; }
    public string ApiServerPort2 { get; set; }
    public string WebServerPort { get; set; }
    public string FlightServerPort { get; set; }
    public string FlightSystemPort { get; set; }
    public string FlightClientPort { get; set; }
    public string FlightServerName { get; set; }
    public string MaintainerEmail { get; set; }
    public string HostAddr { get; set; }
    public string HostName { get; set; }
    public string ServerIdent { get; set; }
    public string Location { get; set; }
    public string MaxClient { get; set; }
    public string Database { get; set; }
    public string PermissionLow { get; set; }
    public string PermissionMid { get; set; }
    public string PermissionHig { get; set; }
}