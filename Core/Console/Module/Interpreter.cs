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

public static class Interpreter
{
    public static void Welcome(Version v)
    {
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine(" _______  _______  _______  __    _  __   __  __   __  _______ ");
        System.Console.WriteLine("|       ||       ||       ||  |  | ||  | |  ||  |_|  ||       |");
        System.Console.WriteLine("|   _   ||    _  ||    ___||   |_| ||  |_|  ||       ||  _____|");
        System.Console.WriteLine("|  | |  ||   |_| ||   |___ |       ||       ||       || |_____ ");
        System.Console.WriteLine("|  |_|  ||    ___||    ___||  _    ||       ||       ||_____  |");
        System.Console.WriteLine("|       ||   |    |   |___ | | |   | |     | | ||_|| | _____| |");
        System.Console.WriteLine("|_______||___|    |_______||_|  |__|  |___|  |_|   |_||_______|{0}\n",v);
        System.Console.ResetColor();
        System.Console.WriteLine("Author: Guo Tingjin\tMail:dev@openvmsys.cn\tGithub:https://github.com/OpenVMSys");
        System.Console.WriteLine("Welcome to OpenVMSys3 Console, application is not started yet, try \"service start\"");
    }

    public static void Ready()
    {
        System.Console.Write("\n[{0}] [OpenVMSys3:Main] => ",DateTime.Now);
    }

    public static int Interpret(string[] environmentArgs)
    {
        System.Console.Write("\n[{0}] [OpenVMSys3:Main] => ",DateTime.Now);
        var commands = System.Console.ReadLine();
        if (commands==null)
        {
            return 1;
        }

        var mainArg = commands.Split(" ")[0];
        var args = commands.Split(" ")[new Range(1, commands.Split(" ").Length)];
        return Operator.Decode(mainArg.ToLower(),args,environmentArgs);
    }
}