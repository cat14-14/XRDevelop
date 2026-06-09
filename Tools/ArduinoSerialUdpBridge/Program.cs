using System;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal static class Program
{
    private static int Main(string[] args)
    {
        string portName = args.Length > 0 ? args[0] : "COM6";
        int baudRate = args.Length > 1 && int.TryParse(args[1], out int parsedBaud) ? parsedBaud : 9600;
        int udpPort = args.Length > 2 && int.TryParse(args[2], out int parsedUdpPort) ? parsedUdpPort : 5005;

        using var serialPort = new SerialPort(portName, baudRate)
        {
            NewLine = "\n",
            ReadTimeout = 50
        };
        using var udpClient = new UdpClient();

        Console.WriteLine($"Opening serial {portName} @ {baudRate}");
        serialPort.Open();
        Console.WriteLine($"Forwarding serial lines to UDP 127.0.0.1:{udpPort}");

        while (true)
        {
            try
            {
                string line = serialPort.ReadLine().Trim();
                if (line.Length == 0)
                    continue;

                byte[] bytes = Encoding.UTF8.GetBytes(line);
                udpClient.Send(bytes, bytes.Length, "127.0.0.1", udpPort);
                Console.WriteLine(line);
            }
            catch (TimeoutException)
            {
                Thread.Sleep(1);
            }
        }
    }
}
