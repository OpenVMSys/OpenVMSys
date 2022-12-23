using OpenSDK;

namespace OpenVMSys.Core.FlightServer;

public class OpenFlightServer
{
    private static string? _configFile;
    private static OfsConfig _config = new OfsConfig();
    //private static int _isDebugging;
    private static ClientInterface? _clientInterface;
    private static ServerInterface? _serverInterface;
    private static SystemInterface? _systemInterface;
    private static readonly List<Thread> Threads = new();
    public OpenFlightServer(IReadOnlyList<string> args)
    {
        ReadArgs(args);
        ConfigureServer();
        CreateInterfaces();
    }

    private static void ReadArgs(IReadOnlyList<string> args)
    {
        for (var i = 0; i < args.Count; i++)
        {
            switch (args[i])
            {
                case "-c":
                    _configFile = args[i++];
                    break;
                case "-d":
                    //_isDebugging++;
                    break;
                default:
                    OpenSDK.Logger<OpenFlightServer>.Warning("unknown parameter", args[i], "ignored");
                    break;
            }
        }
    }

    private static void ConfigureServer()
    {
        OpenSDK.Logger<OpenFlightServer>.Info("Configuring server...");
        if (_configFile==null)
        {
            OpenSDK.Logger<OpenFlightServer>.Error("Configuration file not specified, server shut down.");
            return;
        }
        _config = ConfReader<OfsConfig>.Read(_config, _configFile);
        var config2 = ConfReader.GetGroup("OpenFlightServer",_configFile);
    }

    private static void CreateInterfaces()
    {
        OpenSDK.Logger<OpenFlightServer>.Info("Creating interfaces...");
        _clientInterface = new ClientInterface(_config);
        _serverInterface = new ServerInterface(_config);
        _systemInterface = new SystemInterface(_config);
        Threads.Add(new Thread(() => _clientInterface.Listen()));
        Threads.Add(new Thread(() => _serverInterface.Listen()));
        Threads.Add(new Thread(() => _systemInterface.Listen()));
    }

    public void Run()
    {
        foreach (var thread in Threads)
        {
            thread.Start();
        }
    }
}