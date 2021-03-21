using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO.Ports;
using F4SharedMem;
using F4SharedMem.Headers;

// Consider switching to https://www.sparxeng.com/blog/software/reading-lines-serial-port

namespace f_16_home_cockpit_interface
{
    class Program
    {
        private static Reader _sharedMemReader = new Reader();
        private static FlightData _lastFlightData;
        static Timer _timer = new Timer();

        static SerialPort _serialPort = new SerialPort();
        static String[] _dedLines = new string[5] {"", "", "", "", ""};
        //static UInt16 _count = 0;

        static void Main(string[] args)
        {
            _serialPort.PortName = "COM6";
            _serialPort.BaudRate = 115200;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Encoding = Encoding.Unicode;

            try
            {
                _serialPort.Open();

                _timer.Elapsed += _timer_Tick;
                _timer.Interval = 20;
                _timer.Start();
            }
            catch (Exception Ex)
            {
                Console.Write(Ex);
            }

            while (true)
            {
#if false
                String dedText = "";

                _dedLines[0] = _count.ToString();
                _count++;

                for (UInt16 i = 0; i < 5; i++)
                {
                    dedText += _dedLines[i].PadRight(24);
                }

                _serialPort.Write(dedText);

                System.Threading.Thread.Sleep(1000);
#endif
            }
        }

        private static void _timer_Tick(object sender, ElapsedEventArgs e)
        {
            if (ReadSharedMem() != null)
            {
                String dedText = "";

                //_dedLines[0] = _count.ToString();
                //_count++;

                for (UInt16 i = 0; i < _lastFlightData.DEDLines.Length; i++)
                {
                    var thisLineBytes = Encoding.Default.GetBytes(_lastFlightData.DEDLines[i] ?? string.Empty);
                    var thisLineInvertBytes = Encoding.Default.GetBytes(_lastFlightData.Invert[i] ?? string.Empty);

                    for (UInt16 j = 0; j < _lastFlightData.DEDLines[i].Length; j++)
                    {
                        var thisByte = thisLineBytes[j];
                        var thisChar = (char)thisByte;

                        var thisByteInvert = _lastFlightData.Invert[i].Length > j ? (byte)thisLineInvertBytes[j] : (byte)0;
                        var inverted = thisByteInvert > 0 && thisByteInvert != 32;

                        if (inverted)
                        {
                            switch (thisByte)
                            {
                                case  1:  { thisChar = '\u8482';        } break;    // <Arrow up/down>
                                case  2:  { thisChar = (char)(215);     } break;    // *
                                case 32:  { thisChar = (char)(160);     } break;    // <Space>
                                case 33:  { thisChar = (char)(161);     } break;    // !
                                case 34:  { thisChar = '\u8222';        } break;    // "
                                case 35:  { thisChar = '\u8364';        } break;    // #
                                case 36:  { thisChar = (char)(162);     } break;    // $
                                case 37:  { thisChar = (char)(247);     } break;    // %
                                case 38:  { thisChar = (char)(163);     } break;    // &
                                case 39:  { thisChar = '\u8219';        } break;    // '
                                case 40:  { thisChar = '\u8249';        } break;    // (
                                case 41:  { thisChar = '\u8250';        } break;    // )
                                case 42:  { thisChar = (char)(215);     } break;    // *
                                case 43:  { thisChar = (char)(164);     } break;    // +
                                case 44:  { thisChar = '\u8218';        } break;    // ,
                                case 45:  { thisChar = '\u8212';        } break;    // -
                                case 46:  { thisChar = '\u8226';        } break;    // .
                                case 47:  { thisChar = (char)(182);     } break;    // /
                                case 48:  { thisChar = (char)(192);     } break;    // 0
                                case 49:  { thisChar = (char)(193);     } break;    // 1
                                case 50:  { thisChar = (char)(194);     } break;    // 2
                                case 51:  { thisChar = (char)(195);     } break;    // 3
                                case 52:  { thisChar = (char)(196);     } break;    // 4
                                case 53:  { thisChar = (char)(197);     } break;    // 5
                                case 54:  { thisChar = (char)(198);     } break;    // 6
                                case 55:  { thisChar = (char)(199);     } break;    // 7
                                case 56:  { thisChar = (char)(200);     } break;    // 8
                                case 57:  { thisChar = (char)(201);     } break;    // 9
                                case 58:  { thisChar = (char)(168);     } break;    // :
                                case 59:  { thisChar = (char)(170);     } break;    // ;
                                case 60:  { thisChar = (char)(185);     } break;    // ( <small parenthesis left>
                                case 61:  { thisChar = (char)(177);     } break;    // : (large colon)
                                case 62:  { thisChar = (char)(186);     } break;    // ) <small parenthesis right>
                                case 63:  { thisChar = (char)(191);     } break;    // ?
                                case 64:  { thisChar = '\u8482';        } break;    // <Arrow up/down>
                                case 65:  { thisChar = (char)(97);      } break;    // A
                                case 66:  { thisChar = (char)(98);      } break;    // B
                                case 67:  { thisChar = (char)(99);      } break;    // C
                                case 68:  { thisChar = (char)(100);     } break;    // D
                                case 69:  { thisChar = (char)(101);     } break;    // E
                                case 70:  { thisChar = (char)(102);     } break;    // F
                                case 71:  { thisChar = (char)(103);     } break;    // G
                                case 72:  { thisChar = (char)(104);     } break;    // H
                                case 73:  { thisChar = (char)(105);     } break;    // I
                                case 74:  { thisChar = (char)(106);     } break;    // J
                                case 75:  { thisChar = (char)(107);     } break;    // K
                                case 76:  { thisChar = (char)(108);     } break;    // L
                                case 77:  { thisChar = (char)(109);     } break;    // M
                                case 78:  { thisChar = (char)(110);     } break;    // N
                                case 79:  { thisChar = (char)(111);     } break;    // O
                                case 80:  { thisChar = (char)(112);     } break;    // P
                                case 81:  { thisChar = (char)(113);     } break;    // Q
                                case 82:  { thisChar = (char)(114);     } break;    // R
                                case 83:  { thisChar = (char)(115);     } break;    // S
                                case 84:  { thisChar = (char)(116);     } break;    // T
                                case 85:  { thisChar = (char)(117);     } break;    // U
                                case 86:  { thisChar = (char)(118);     } break;    // V
                                case 87:  { thisChar = (char)(119);     } break;    // W
                                case 88:  { thisChar = (char)(120);     } break;    // X
                                case 89:  { thisChar = (char)(121);     } break;    // Y
                                case 90:  { thisChar = (char)(122);     } break;    // Z
                                case 91:  { thisChar = (char)(171);     } break;    // [
                                case 92:  { thisChar = (char)(172);     } break;    // \
                                case 93:  { thisChar = (char)(187);     } break;    // ]
                                case 94:  { thisChar = (char)(176);     } break;    // °
                                case 95:  { thisChar = (char)(175);     } break;    // _
                                default:  { thisChar = (char)thisByte;  } break;
                            }
                        }
                        else
                        {
                            switch (thisByte)
                            {
                                case 1:   { thisChar = (char)64;        } break;    // @
                                case 2:   { thisChar = (char)42;        } break;    // *
                                default:  { thisChar = (char)thisByte;  } break;
                            }
                        }

                        dedText += thisChar;
                    }
                }
                
                _serialPort.Write(dedText);
                //BindSharedMemoryDataToFormElements();
            }
            else
            {
                //DisableControlsAllTabs();
            }
        }

        private static FlightData ReadSharedMem()
        {
            return _lastFlightData = _sharedMemReader.GetCurrentData();
        }
    }
}
