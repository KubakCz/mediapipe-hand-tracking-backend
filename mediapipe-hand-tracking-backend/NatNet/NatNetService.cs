using MediaPipeHandTrackingBackend.Models;
using NatNetML;

namespace MediaPipeHandTrackingBackend.NatNet;

public class NatNetService
{
    private readonly NatNetClientML client = new NatNetClientML();
    public NatNetConnectionSettings? ConnectionSettings { get; private set; } = new NatNetConnectionSettings();
    public bool IsRecording { get; private set; } = false;
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

    public NatNetErrorCode StartRecording()
    {
        if (IsRecording)
            throw new InvalidOperationException("Recording already in progress.");

        int nBytes = 0;
        byte[] response = new byte[1000];
        NatNetErrorCode ec = (NatNetErrorCode)client.SendMessageAndWait("StartRecording", 3, 100, out response, out nBytes);

        if (ec is not NatNetErrorCode.OK)
            return ec;

        int opResult = BitConverter.ToInt32(response, 0);
        if (opResult != 0)
            return (NatNetErrorCode)opResult;

        IsRecording = true;
        return NatNetErrorCode.OK;
    }

    public NatNetErrorCode StopRecording()
    {
        if (!IsRecording)
            throw new InvalidOperationException("Recording not in progress.");

        int nBytes = 0;
        byte[] response = new byte[1000];
        NatNetErrorCode ec = (NatNetErrorCode)client.SendMessageAndWait("StopRecording", 3, 100, out response, out nBytes);

        if (ec is not NatNetErrorCode.OK)
            return ec;

        int opResult = BitConverter.ToInt32(response, 0);
        if (opResult != 0)
            return (NatNetErrorCode)opResult;

        IsRecording = false;
        return NatNetErrorCode.OK;
    }
}