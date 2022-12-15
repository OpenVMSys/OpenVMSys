using System.Net;
using System.Net.Sockets;
using System.Text;
using OpenVMSys.Core.FlightServer.FlightServerModule;

namespace OpenVMSys.Core.FlightServer;

public class ServerInterface
{
    private readonly TcpListener _listener;
    private readonly List<Client> _clients = new();
    private readonly OmsConfig _config = new();

    public ServerInterface()
    {
        _config = OpenSDK.ConfReader<OmsConfig>.Read(_config, "oms.conf");
        if (_config.FlightServerPort==null)
        {
            OpenSDK.Logger<ServerInterface>.Error("FlightServerPort Not Set");
            Environment.Exit(-1);
        }
        var endPoint = new IPEndPoint(IPAddress.Any, int.Parse(_config.FlightServerPort));
        _listener = new TcpListener(endPoint);
    }

    public void Listen()
    {
        _listener.Start();
        _listener.BeginAcceptTcpClient(ClientAcceptance, _listener);
        OpenSDK.Logger<ServerInterface>.Info("FlightServer Started at port:",_config.FlightServerPort);
    }

    private void ClientAcceptance(IAsyncResult asyncResult)
    {
        if (_clients.Count == int.Parse(_config.MaxClient))
        {
            return;
        }
        if (asyncResult.AsyncState == null)
        {
            return;
        }
        var listener = (TcpListener)asyncResult.AsyncState;
        var tcpClient = listener.EndAcceptTcpClient(asyncResult);
        if (!Authentication.ValidateIpAddr(tcpClient))
        {
            _listener.BeginAcceptTcpClient(ClientAcceptance, _listener);
        }
        else
        {
            var client = new Client(null, null, null, null, null, null, tcpClient);
            _clients.Add(client);
            if (client.TcpClient.Client.RemoteEndPoint?.ToString() == null)
            {
                return;
            }
            OpenSDK.Logger<ServerInterface>.Info("Client connected, address:", client.TcpClient.Client.RemoteEndPoint.ToString());
            var t = new Thread(Handle);
            t.Start(client);
            _listener.BeginAcceptTcpClient(ClientAcceptance, _listener);
        }
    }
    private void Handle(object? receiveClient)
    {
        if (receiveClient is not Client client)
        {
            OpenSDK.Logger<ServerInterface>.Error("Client error");
            return;
        }

        while (true)
        {
            try
            {
                if (client.TcpClient == null)
                {
                    _clients.Remove(client);
                }
                var stream = client.TcpClient.GetStream();
                var bytes = new byte[1024];
                var connectionFactor = stream.Read(bytes);
                if (connectionFactor == 0)
                {
                    OpenSDK.Logger<ServerInterface>.Info("Client",client.TcpClient.Client.RemoteEndPoint.ToString(),"closed");
                    _clients.Remove(client);
                    break;
                }

                ServerCommandHandler.Analyze(_clients, client, stream, Encoding.UTF8.GetString(bytes), _config);
            }
            catch (Exception e)
            {
                var clientNow = from c in _clients where c == client select c;
                foreach (var clientD in clientNow)
                {
                    _clients.Remove(clientD);
                }
                OpenSDK.Logger<ServerInterface>.Error(e.Message);
                break;
            }
        }
        OpenSDK.Logger<ServerInterface>.Info("Reset");
    }
}