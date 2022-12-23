using System.Net.Sockets;

namespace OpenVMSys.Core.FlightServer.FlightServerModule;

public class Client
{
    public TcpClient? TcpClient { get; set; }
    public string? Cid { get; set; }
    public string? CallSign { get; set; }
    public string? RealName { get; set; }
    public double? Lat { get; set; }
    public double? Lon { get; set; }
    public int? Transponder { get; set; }
    public double? Altitude { get; set; }
    public double? GroundSpeed { get; set; }
    public int? Frequency { get; set; }
    public int? FacilityType { get; set; }
    public int? Type { get; set; }
    public int PositionOk { get; set; }
    public int? Rating { get; set; }
    public int? SimType { get; set; }
    public int? VisualRange { get; set; }
    public FlightPlan? FlightPlan { get; set; }

    public Client(string? cid, string? callSign, int? reqRating, string? realName, int? simType, int? type, TcpClient client)
    {
        Cid = cid;
        CallSign = callSign;
        Rating = reqRating;
        RealName = realName;
        SimType = simType;
        Type = type;
        FacilityType = 0;
        VisualRange = 40;
        FlightPlan = null;
        Altitude = 0;
        Frequency = 0;
        Transponder = 0;
        GroundSpeed = 0;
        Lat = 0;
        Lon = 0;
        TcpClient = client;
        PositionOk = 0;
    }

    public void HandleFp(string[] fpArray)
    {
        FlightPlan ??= new FlightPlan();
        try
        {
            FlightPlan.Revision += 1;
            FlightPlan.CallSign = fpArray[0];
            FlightPlan.Type = char.Parse(fpArray[1]);
            FlightPlan.Aircraft = fpArray[2];
            FlightPlan.TasCruise = int.Parse(fpArray[3]);
            FlightPlan.DepAirport = fpArray[4];
            FlightPlan.DepTime = int.Parse(fpArray[5]);
            FlightPlan.ActDepTime = int.Parse(fpArray[6]);
            FlightPlan.Altitude = fpArray[7];
            FlightPlan.DestAirport = fpArray[8];
            FlightPlan.HrsEnroute = int.Parse(fpArray[9]);
            FlightPlan.MinEnroute = int.Parse(fpArray[10]);
            FlightPlan.HrsFuel = int.Parse(fpArray[11]);
            FlightPlan.MinFuel = int.Parse(fpArray[12]);
            FlightPlan.AlternateAirport = fpArray[13];
            FlightPlan.Remarks = fpArray[14];
            FlightPlan.Route = fpArray[15];
        }
        catch (Exception e)
        {
            OpenSDK.Logger<Client>.Error(e.Message);
        }
    }

    public void UpdatePilot(string[] ptArray)
    {
        try
        {
            CallSign = ptArray[1];
            Transponder = int.Parse(ptArray[2]);
            Rating = int.Parse(ptArray[3]);
            Lat = double.Parse(ptArray[4]);
            Lon = double.Parse(ptArray[5]);
            Altitude = double.Parse(ptArray[6]);
            GroundSpeed = double.Parse(ptArray[7]);
            PositionOk = 1;
        }
        catch (Exception e)
        {
            OpenSDK.Logger<Client>.Error(e.Message);
        }
    }

    public void UpdateAtc(string[] atArray)
    {
        try
        {
            CallSign = atArray[1];
            Frequency = int.Parse(atArray[2]);
            FacilityType = int.Parse(atArray[3]);
            VisualRange = int.Parse(atArray[4]);
            Rating = int.Parse(atArray[5]);
            Lat = double.Parse(atArray[6]);
            Lon = double.Parse(atArray[7]);
            Altitude = double.Parse(atArray[8]);
            PositionOk = 1;
        }
        catch (Exception e)
        {
            OpenSDK.Logger<Client>.Error(e.Message);
        }
    }

    public double Distance(Client other)
    {
        return Support.Dist(Lat, Lon, other.Lat, other.Lon);
    }

    public int GetRange()
    {
        if (Type==1)
        {
            if (Altitude<0 || Altitude == null)
            {
                Altitude = 0;
            }
            return (int)(10 + 1.414 * Math.Sqrt(Altitude.Value));
        }

        switch (FacilityType)
        {
            case 0://Unknown
                return 40;
            case 1://FSS
                return 1500;
            case 2://CLR_DEL
            case 3://GROUND
                return 5;
            case 4://TOWER
                return 30;
            case 5://APP/DEP
                return 100;
            case 6://CENTER
                return 400;
            case 7://MONITOR
                return 1500;
            default:
                return 40;
        }
    }
}