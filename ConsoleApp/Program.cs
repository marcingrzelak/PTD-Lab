using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
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
        public const string DEVICENAME = "COM3";
        #endregion

        public static int PortNum;
        public static ushort WheelModeData;

        static void Initialization()
        {
            PortNum = dynamixel.portHandler(DEVICENAME);
            dynamixel.packetHandler();
        }

        static void OpenPort()
        {
            if (!dynamixel.openPort(PortNum))
                throw new Exception("Failed to open the port!");
            else
                Console.WriteLine("Succeeded to open the port!");
        }

        static void SetBaudRate()
        {
            if (!dynamixel.setBaudRate(PortNum, BAUDRATE))
                throw new Exception("Failed to change the baudrate!");
            else
                Console.WriteLine("Succeeded to change the baudrate!");
        }

        static void EnableDynamixelTorque(byte pID)
        {
            int Result;
            byte Error;

            dynamixel.write1ByteTxRx(PortNum, PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_TORQUE_ENABLE, Dynamixel.Constants.TORQUE_ENABLE);

            if ((Result = dynamixel.getLastTxRxResult(PortNum, PROTOCOL_VERSION)) != Dynamixel.Constants.COMM_SUCCESS)
            {
                throw new Exception(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, Result)));
            }
            else if ((Error = dynamixel.getLastRxPacketError(PortNum, PROTOCOL_VERSION)) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, Error)));
            }
            else
            {
                Console.WriteLine("Dynamixel has been successfully connected");
            }
        }

        static void EnableWheelMode(byte pID)
        {
            //todo: rzucanie wyjatkow
            dynamixel.write2ByteTxRx(PortNum, PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_CCW_ANGLE_LIMIT, WheelModeData);
        }

        static void SetMovingSpeed(byte pID, ushort pMovingSpeed)
        {
            //todo: rzucanie wyjatkow
            dynamixel.write2ByteTxRx(PortNum, PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_MOVING_SPEED, pMovingSpeed);
        }

        static void DisableDynamixelTorque(byte pID)
        {
            int Result;
            byte Error;

            dynamixel.write1ByteTxRx(PortNum, PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_TORQUE_ENABLE, Dynamixel.Constants.TORQUE_DISABLE);

            if ((Result = dynamixel.getLastTxRxResult(PortNum, PROTOCOL_VERSION)) != Dynamixel.Constants.COMM_SUCCESS)
            {
                throw new Exception(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, Result)));
            }
            else if ((Error = dynamixel.getLastRxPacketError(PortNum, PROTOCOL_VERSION)) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, Error)));
            }
            else
            {
                Console.WriteLine("Dynamixel has been successfully disconnected");
            }
        }

        static void ClosePort()
        {
            dynamixel.closePort(PortNum);
        }

        static void Main(string[] args)
        {
            Initialization();
            try
            {
                OpenPort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                SetBaudRate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            EnableWheelMode(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
            EnableWheelMode(Dynamixel.Constants.FRONT_LEFT_WHEEL);
            EnableWheelMode(Dynamixel.Constants.REAR_RIGHT_WHEEL);
            EnableWheelMode(Dynamixel.Constants.REAR_LEFT_WHEEL);

            SetMovingSpeed(Dynamixel.Constants.FRONT_RIGHT_WHEEL, 300);
            SetMovingSpeed(Dynamixel.Constants.FRONT_LEFT_WHEEL, 1324);
            SetMovingSpeed(Dynamixel.Constants.REAR_RIGHT_WHEEL, 300);
            SetMovingSpeed(Dynamixel.Constants.REAR_LEFT_WHEEL, 1324);

            try
            {
                EnableDynamixelTorque(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
                EnableDynamixelTorque(Dynamixel.Constants.FRONT_LEFT_WHEEL);
                EnableDynamixelTorque(Dynamixel.Constants.REAR_RIGHT_WHEEL);
                EnableDynamixelTorque(Dynamixel.Constants.REAR_LEFT_WHEEL);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                if (Console.ReadKey().KeyChar == 0x1b)
                    break;
            }

            try
            {
                DisableDynamixelTorque(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
                DisableDynamixelTorque(Dynamixel.Constants.FRONT_LEFT_WHEEL);
                DisableDynamixelTorque(Dynamixel.Constants.REAR_RIGHT_WHEEL);
                DisableDynamixelTorque(Dynamixel.Constants.REAR_LEFT_WHEEL);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ClosePort();

        }
    }
}
