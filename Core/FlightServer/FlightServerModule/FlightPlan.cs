namespace OpenVMSys.Core.FlightServer.FlightServerModule;

public class FlightPlan
{
    public string? CallSign { get; set; }
    public int Revision { get; set; }
    public char Type { get; set; }
    public string? Aircraft { get; set; }
    public int TasCruise { get; set; }
    public string? DepAirport { get; set; }
    public int DepTime { get; set; }
    public int ActDepTime { get; set; }
    public string? Altitude { get; set; }
    public string? DestAirport { get; set; }
    public int HrsEnroute { get; set; }
    public int MinEnroute { get; set; }
    public int HrsFuel { get; set; }
    public int MinFuel { get; set; }
    public string? AlternateAirport { get; set; }
    public string? Remarks { get; set; }
    public string? Route { get; set; }
}