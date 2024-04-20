using NatNetML;

namespace MediaPipeHandTrackingBackend.Models;

public class ConnectionSettings
{
    public string LocalIP { get; set; } = "127.0.0.1";
    public string ServerIP { get; set; } = "127.0.0.1";
    public ushort CommandPort { get; set; } = 1510;
    public ushort DataPort { get; set; } = 1511;
    public ConnectionType ConnectionType { get; set; } = ConnectionType.Multicast;
}