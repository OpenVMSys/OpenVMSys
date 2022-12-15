using System.Net.Sockets;
using System.Text;

namespace OpenVMSys.Core.FlightServer.FlightServerModule;

public static class ClientHandler
{
    public static void Analyze(List<Client> clients, Client client, NetworkStream stream, string command, OmsConfig config)
    {
        var fields = command.Split(":");
        if (fields.Length == 0)
        {
            return;
        }

        switch (fields[0].ToUpper())
        {
            case "CONNECT":
                Notify(stream,config);
                break;
            case "PING":
                if (fields.Length != 2)
                {
                    SyntaxError(stream);
                    break;
                }
                Ping(stream, fields[1]);
                break;
            case "BROADCAST"://Broadcast a message
                if (fields.Length != 2)
                {
                    SyntaxError(stream);
                    break;
                }
                BroadCast(clients, fields[1]);
                break;
            case "REQMETAR"://Request METAR
                if (fields.Length != 4)
                {
                    SyntaxError(stream);
                    break;
                }
                break;
            case "RMCLIENT"://Remove Client
                if (fields.Length != 1)
                {
                    SyntaxError(stream);
                    break;
                }

                clients.Remove(client);
                break;
            case "ADDCLIENT":
                if (fields.Length != 8)
                {
                    SyntaxError(stream);
                    break;
                }
                break;
            case "PD":
                if (fields.Length != 8)
                {
                    SyntaxError(stream);
                    break;
                }
                client.UpdatePilot(fields);
                break;
            default:
                SyntaxError(stream);
                break;
        }
    }
    private static void Ping(Stream stream, string data)
    {
        stream.WriteAsync(Encoding.UTF8.GetBytes("PONG:" + data));
    }

    private static void Notify(Stream stream, OmsConfig config)
    {
        stream.WriteAsync(Encoding.UTF8.GetBytes("NOTIFY:" + config.ServerIdent + ":" + config.FlightServerName +
                                                 ":" + config.MaintainerEmail + ":" + config.HostName+":"+config.Location));
    }

    private static void SyntaxError(Stream stream)
    {
        stream.WriteAsync(Encoding.UTF8.GetBytes("Syntax Error\n"));
    }

    private static void BroadCast(List<Client>? clients, string message)
    {
        if (clients == null)
        {
            return;
        }

        if (clients.Count == 0)
        {
            return;
        }

        foreach (var client in clients)
        {
            client.TcpClient.GetStream().WriteAsync(Encoding.UTF8.GetBytes(message));
        }
    }
}