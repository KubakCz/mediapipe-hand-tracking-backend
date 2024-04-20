using MediaPipeHandTrackingBackend.Models;
using NatNetML;

namespace MediaPipeHandTrackingBackend.NatNet;

public class NatNetService
{
    private readonly NatNetClientML client = new NatNetClientML();
    public NatNetConnectionSettings? ConnectionSettings { get; private set; } = new NatNetConnectionSettings();
    public bool IsRecording { get; set; } = false;
    public bool IsConnected => ConnectionSettings != null;

    public NatNetService()
    {
        string version = string.Join('.', client.NatNetVersion());
        Console.WriteLine($"Creating new NatNetClient.\nNatNet SDK Version: {version}");
        TryConnectToServer(new NatNetConnectionSettings());  // Try to connect to the server with default settings
    }

    public bool TryConnectToServer(NatNetConnectionSettings connectionSettings)
    {
        if (IsConnected)
        {
            client.Disconnect();
            ConnectionSettings = null;
        }

        Console.WriteLine($"\nConnecting to a NatNet server...\n{ConnectionSettings}");
        var connectOptions = new NatNetClientML.ConnectParams()
        {
            ConnectionType = connectionSettings.ConnectionType,
            ServerAddress = connectionSettings.ServerIP,
            LocalAddress = connectionSettings.LocalIP,
            ServerCommandPort = connectionSettings.CommandPort,
            ServerDataPort = connectionSettings.DataPort
        };
        int result = client.Connect(connectOptions);
        
        if (result != 0)
        {
            Console.WriteLine("Failed to connect to the NatNet server.");
            return false;
        }
        else
        {
            Console.WriteLine("Successfully connected to the NatNet server.");
            ConnectionSettings = connectionSettings;
            return true;
        }
    }
}