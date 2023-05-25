using System;
using System.IO.Ports;
using System.Reflection;
using System.Text;





public class CalculatorReceiver
{
    private SerialPort serialPort;

    public CalculatorReceiver(string portName, int baudRate)
    {
        // Set up the serial port
        serialPort = new SerialPort(portName, baudRate);
        serialPort.DataReceived += SerialPort_DataReceived;
        serialPort.Open();
    }

    public void Close()
    {
        serialPort.Close();
    }
    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        int dataLength = sp.BytesToRead;
        byte[] data = new byte[dataLength];
        sp.Read(data, 0, dataLength);

        if (dataLength == 3)
        {
            byte operand1 = data[0];
            byte operand2 = data[1];
            byte operation = data[2];

            double result = PerformCalculation(operand1, operand2, operation);

            Console.WriteLine($"Received: Operand1={operand1}, Operand2={operand2}, Operation={operation}");
            Console.WriteLine($"Result: {result}");
        }
        else
        {
            Console.WriteLine("Invalid data length");
        }
    }

    private double PerformCalculation(byte operand1, byte operand2, byte operation)
    {
        double num1 = operand1;
        double num2 = operand2;
        double result = 0;

        switch (operation)
        {
            case 0x2B: // Addition operation
                result = num1 + num2;
                break;
            case 0x2D: // Subtraction operation
                result = num1 - num2;
                break;
            case 0x2A: // Multiplication operation
                result = num1 * num2;
                break;
            case 0x2F: // Division operation
                result = num1 / num2;
                break;
            default:
                Console.WriteLine("Invalid operation");
                break;
        }

        return result;
    }

    public class ConsoleGraphics
    {

        private const int WindowWidth = 80;
        private const int WindowHeight = 20;

        private SerialPort serialPort;

        public ConsoleGraphics(string portName, int baudRate)
        {
            // Set up the serial port
            serialPort = new SerialPort(portName, baudRate);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
        }

        private static void DrawResult(string result)
        {
            Console.Clear();

            int startX = (WindowWidth - result.Length) / 2;
            int startY = WindowHeight / 2;

            Console.SetCursorPosition(startX, startY);
            Console.Write(result);
        }
        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int dataLength = sp.BytesToRead;
            byte[] data = new byte[dataLength];
            sp.Read(data, 0, dataLength);

            if (dataLength == 1)
            {
                byte resultByte = data[0];
                string result = Encoding.ASCII.GetString(new byte[] { resultByte });

                DrawResult(result);
            }
            else
            {
                Console.WriteLine("Invalid data length");
            }
        }

        

        public void Close()
        {
            serialPort.Close();
        }

        public static void Main()
        {
            CalculatorReceiver receiver = new CalculatorReceiver("COM3", 115200); // Replace "COMX" with the appropriate COM port name and baud rate
            Console.WriteLine("Calculator Receiver started. Press any key to exit.");
            Console.ReadKey();
            receiver.Close();
            ConsoleGraphics consoleGraphics = new ConsoleGraphics("COM3", 115200); // Replace "COMX" with the appropriate COM port name and baud rate
            Console.WriteLine("Console Graphics started. Press any key to exit.");
            Console.ReadKey();
            SerialPort serialPort = new SerialPort("COM3", 9600); // Replace "COM1" with the appropriate COM port name and baud rate
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();

            Console.WriteLine("Serial port is open. Press any key to exit.");
            Console.ReadKey();

            serialPort.Close();
            
            consoleGraphics.Close();

        }
    }
}

