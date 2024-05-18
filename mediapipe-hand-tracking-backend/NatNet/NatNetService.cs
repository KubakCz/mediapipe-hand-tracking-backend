using MediaPipeHandTrackingBackend.Models;
using NatNetML;

namespace MediaPipeHandTrackingBackend.NatNet;

/// <summary>
/// Service for connecting to a NatNet server and sending commands to it.
/// </summary>
public class NatNetService
{
    private readonly NatNetClientML client = new NatNetClientML();
    public NatNetConnectionSettings? ConnectionSettings { get; private set; }
    public bool IsRecording { get; private set; } = false;
    public bool IsConnected => ConnectionSettings != null;

    public NatNetService()
    {
        string version = string.Join('.', client.NatNetVersion());
        Console.WriteLine($"Creating new NatNetClient.\nNatNet SDK Version: {version}");
        TryConnectToServer(new NatNetConnectionSettings());  // Try to connect to the server with default settings
    }

    /// <summary>
    /// Tries to connect to a NatNet server with the given connection settings.
    /// </summary>
    /// <param name="connectionSettings">Connection settings for the server.</param>
    /// <returns>True if the connection was successful, false otherwise.</returns>
    public bool TryConnectToServer(NatNetConnectionSettings connectionSettings)
    {
        // Disconnect from the current server if connected
        if (IsConnected)
        {
            client.Disconnect();
            ConnectionSettings = null;
        }

        // Try to connect to the new server
        Console.WriteLine($"\nConnecting to a NatNet server...\n{connectionSettings}");
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

    /// <summary>
    /// Starts recording on the connected NatNet server.
    /// </summary>
    /// <returns>Result code of the operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when not connected to a server or recording is already in progress.</exception>
    public NatNetErrorCode StartRecording()
    {
        if (!IsConnected)
            throw new InvalidOperationException("Not connected to a server.");
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

    /// <summary>
    /// Stops recording on the connected NatNet server.
    /// </summary>
    /// <returns>Result code of the operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when not connected to a server or recording is not in progress.</exception>
    public NatNetErrorCode StopRecording()
    {
        if (!IsRecording)
            throw new InvalidOperationException("Recording not in progress.");
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