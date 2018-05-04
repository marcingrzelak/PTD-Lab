using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dynamixel_sdk;
using System.IO;
using Pelco;
using Dynamixel;

namespace ConsoleApp
{
    class Program
    {
        #region Settings
        public const byte PROTOCOL_VERSION = 1;
        public const int BAUDRATE = 1000000;
        public const string DEVICENAME = "COM1";
        #endregion



        static void OpenPort(int PortNum)
        {

        }

        static void Main(string[] args)
        {
            int port_num = dynamixel.portHandler("COM1");
        }
    }
}
