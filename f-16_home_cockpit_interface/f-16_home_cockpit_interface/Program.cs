using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace f_16_home_cockpit_interface
{
    class Program
    {
        static SerialPort _serialPort = new SerialPort();
        static String[] _dedLines = new string[5] {"", "", "", "", ""};
        static UInt16 _count = 0;
        static void Main(string[] args)
        {
            _serialPort.PortName = "COM6";
            _serialPort.BaudRate = 115200;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;

            try
            {
                _serialPort.Open();
            }
            catch (Exception Ex)
            {
                Console.Write(Ex);
            }

            while (true)
            {
                String dedText = "";

                _dedLines[0] = _count.ToString();
                _count++;

                for (UInt16 i = 0; i < 5; i++)
                {
                    dedText += _dedLines[i].PadRight(24);
                }

                _serialPort.Write(dedText);

                System.Threading.Thread.Sleep(1000);
            }


        }
    }
}
