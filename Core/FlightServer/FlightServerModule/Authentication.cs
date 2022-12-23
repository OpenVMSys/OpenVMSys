using System.Net.Sockets;
using System.Text;
using Path = OpenSDK.Path;

namespace OpenVMSys.Core.FlightServer.FlightServerModule;

public class Authentication
{
    private static string[]? _banList;
    public static bool ValidateIpAddr(TcpClient? client)
    {
        if (client == null)
        {
            return false;
        }
        var addr = client.Client.RemoteEndPoint?.ToString()?.Split(":")[0];
        if (_banList == null || Refresh() != _banList)
        {
            _banList = Refresh();
        }
        OpenSDK.Logger<Authentication>.Info("Validating",client.Client.RemoteEndPoint?.ToString());
        
        return _banList.All(ban => ban != addr);
    }

    private static string[] Refresh()
    {
        var fileStream = new FileStream(Path.Join("banList.txt"), FileMode.OpenOrCreate);
        var readStream = new StreamReader(fileStream, Encoding.UTF8);
        var nBanList = readStream.ReadToEnd().Split("\n");
        readStream.Close();
        fileStream.Close();
        return nBanList;
    }
}