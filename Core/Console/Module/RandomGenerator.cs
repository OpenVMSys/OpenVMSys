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
namespace OpenVMSys.Core.Console.Module;

public static class RandomGenerator
{
    private static int _rep;
    public static string Gen()
    {
        var str = string.Empty;
        var num2 = DateTime.Now.Ticks + _rep;
        _rep++;
        Random random = new(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> _rep)));
        for (var i = 0; i < 20; i++)
        {
            char ch;
            var num = random.Next();
            if ((num % 2) == 0)
            {
                ch = (char)(0x30 + ((ushort)(num % 10)));
            }
            else
            {
                ch = (char)(0x41 + ((ushort)(num % 0x1a)));
            }
            str += ch.ToString();
        }

        return str;
    }
}