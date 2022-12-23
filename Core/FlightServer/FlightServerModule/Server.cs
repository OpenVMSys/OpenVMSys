namespace OpenVMSys.Core.FlightServer.FlightServerModule;

public class Server
{
    public int PacketCount { get; set; }
    public int Hops { get; set; }
    public int Check { get; set; }
    public int Lag { get; set; }
    public int Flags { get; set; }
    public int PacketDrops { get; set; }
    public DateTime Alive { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Ident { get; set; }
    public string HostName { get; set; }
    public string Version { get; set; }
    public string Location { get; set; }

    public void ReceivePong(string data)
    {
        
    }

    public void SetAlive()
    {
        Alive=DateTime.UtcNow;
    }

    public void SetPath()
    {
        
    }
}