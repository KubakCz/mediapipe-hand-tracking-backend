using NatNetML;

namespace MediaPipe_Hand_Tracking_Backend
{
    class Program
    {
        /*  [NatNet] Network connection configuration    */
        private static NatNetClientML natNet = new NatNetClientML();    // The client instance
        private static string strLocalIP = "127.0.0.1";                 // Local IP address (string)
        private static string strServerIP = "127.0.0.1";                // Server IP address (string)
        private static ushort serverCommandPort = 1510;
        private static ushort serverDataPort = 1511;
        private static ConnectionType connectionType = ConnectionType.Multicast;    // Connection Type

        private static bool isRecording = false;

        public static async Task Main(string[] args)
        {
            await ConnectToServer();
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.S)
                {
                    if (isRecording)
                        Console.WriteLine(StopRecording());
                    else
                        Console.WriteLine(StartRecording());
                }
            }
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

            while (natNet.Connect(connect_options) != 0)
            {
                Console.WriteLine("Unable to connect to the NatNet server. Trying again in 10 seconds.");
                await Task.Delay(10 * 1000);
            }
            Console.WriteLine("Successfully connected to the server.");
        }

        static ErrorCode StartRecording()
        {
            if (isRecording)
                throw new InvalidOperationException("Recording already in progress.");

            int nBytes = 0;
            byte[] response = new byte[1000];
            ErrorCode ec = (ErrorCode)natNet.SendMessageAndWait("StartRecording", 3, 100, out response, out nBytes);

            if (ec is not ErrorCode.OK)
                return ec;
            
            int opResult = BitConverter.ToInt32(response, 0);
            if (opResult != 0)
                throw new Exception("No idea what that means, but the command failed.");
            
            isRecording = true;
            return ErrorCode.OK;
        }

        static ErrorCode StopRecording()
        {
            if (!isRecording)
                throw new InvalidOperationException("Recording not in progress.");

            int nBytes = 0;
            byte[] response = new byte[1000];
            ErrorCode ec = (ErrorCode)natNet.SendMessageAndWait("StopRecording", 3, 100, out response, out nBytes);

            if (ec is not ErrorCode.OK)
                return ec;

            int opResult = BitConverter.ToInt32(response, 0);
            if (opResult != 0)
                throw new Exception("No idea what that means, but the command failed.");

            isRecording = false;
            return ErrorCode.OK;
        }
    }
}
