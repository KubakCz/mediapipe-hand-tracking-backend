using NatNetML;

class Program
{
    /*  [NatNet] Network connection configuration    */
    private static NatNetClientML natNet = new NatNetClientML();    // The client instance
    private static string strLocalIP = "127.0.0.1";                 // Local IP address (string)
    private static string strServerIP = "127.0.0.1";                // Server IP address (string)
    private static ushort serverCommandPort = 1510;
    private static ushort serverDataPort = 1511;
    private static ConnectionType connectionType = ConnectionType.Multicast;    // Connection Type

    public static async Task Main(string[] args)
    {
        await ConnectToServer();
    }

    static async Task ConnectToServer()
    {
        /*  [NatNet] Checking verions of the NatNet SDK library  */
        int[] verNatNet = new int[4];
        verNatNet = natNet.NatNetVersion();
        Console.WriteLine("NatNet SDK Version: {0}.{1}.{2}.{3}", verNatNet[0], verNatNet[1], verNatNet[2], verNatNet[3]);

        /*  [NatNet] Connecting to the Server    */
        Console.WriteLine($"\nConnecting...\n\tLocal IP address: {strLocalIP}\n\tServer IP Address: {strServerIP}\n\tCommand port: {serverCommandPort}\n\tData port: {serverDataPort}");
        var connect_options = new NatNetClientML.ConnectParams()
        {
            ConnectionType = connectionType,
            ServerAddress = strServerIP,
            LocalAddress = strLocalIP,
            ServerCommandPort = serverCommandPort,
            ServerDataPort = serverDataPort
        };
        
        while(natNet.Connect(connect_options) != 0)
        {
            Console.WriteLine("Unable to connect to the NatNet server. Trying again in 10 seconds.");
            await Task.Delay(10 * 1000);
        }
        Console.WriteLine("Successfully connected to the server.");
    }
}
