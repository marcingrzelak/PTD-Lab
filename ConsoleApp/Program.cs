using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
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
        public static ushort WheelModeData = 0;
        public static int CurrentSpeed = 0;
        public static int CurrentDirection = 0;

        static void Initialization()
        {
            PortNum = DynamixelSDK.PortHandler(DEVICENAME);
            DynamixelSDK.PacketHandler();
        }

        static void OpenPort()
        {
            if (!DynamixelSDK.OpenPort(PortNum))
                throw new Exception("Failed to open the port!");
            else
                Console.WriteLine("Succeeded to open the port!");
        }

        static void SetBaudRate()
        {
            if (!DynamixelSDK.SetBaudRate(PortNum, BAUDRATE))
                throw new Exception("Failed to change the baudrate!");
            else
                Console.WriteLine("Succeeded to change the baudrate!");
        }

        static void EnableDynamixelTorque(byte pID)
        {
            int Result;
            byte Error;

            DynamixelSDK.Write1ByteTxRx(PortNum, PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_TORQUE_ENABLE, Dynamixel.Constants.TORQUE_ENABLE);

            if ((Result = DynamixelSDK.GetLastTxRxResult(PortNum, PROTOCOL_VERSION)) != Dynamixel.Constants.COMM_SUCCESS)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.GetTxRxResult(PROTOCOL_VERSION, Result)));
            }
            else if ((Error = DynamixelSDK.GetLastRxPacketError(PortNum, PROTOCOL_VERSION)) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.GetRxPacketError(PROTOCOL_VERSION, Error)));
            }
            else
            {
                Console.WriteLine("Dynamixel has been successfully connected");
            }
        }

        static void EnableWheelMode(byte pID)
        {
            //todo: rzucanie wyjatkow
            DynamixelSDK.Write2ByteTxRx(PortNum, PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_CCW_ANGLE_LIMIT, WheelModeData);
        }

        static void SetMovingSpeed(byte pID, ushort pMovingSpeed)
        {
            //todo: rzucanie wyjatkow
            DynamixelSDK.Write2ByteTxRx(PortNum, PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_MOVING_SPEED, pMovingSpeed);
        }

        static void DisableDynamixelTorque(byte pID)
        {
            int Result;
            byte Error;

            DynamixelSDK.Write1ByteTxRx(PortNum, PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_TORQUE_ENABLE, Dynamixel.Constants.TORQUE_DISABLE);

            if ((Result = DynamixelSDK.GetLastTxRxResult(PortNum, PROTOCOL_VERSION)) != Dynamixel.Constants.COMM_SUCCESS)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.GetTxRxResult(PROTOCOL_VERSION, Result)));
            }
            else if ((Error = DynamixelSDK.GetLastRxPacketError(PortNum, PROTOCOL_VERSION)) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.GetRxPacketError(PROTOCOL_VERSION, Error)));
            }
            else
            {
                Console.WriteLine("Dynamixel has been successfully disconnected");
            }
        }

        static void ClosePort()
        {
            DynamixelSDK.ClosePort(PortNum);
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
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
                                
                else if (Console.ReadKey().Key == ConsoleKey.W)
                {
                    CurrentSpeed += 1;
                }
                
                else if (Console.ReadKey().Key == ConsoleKey.S)
                {
                    CurrentSpeed -= 1;
                }
                                
                else if (Console.ReadKey().Key == ConsoleKey.A)
                {
                    CurrentDirection += 1;
                }
                                
                else if (Console.ReadKey().Key == ConsoleKey.D)
                {
                    CurrentDirection -= 0;
                }

                Console.Write("Speed: ");
                Console.WriteLine(CurrentSpeed);
                Console.Write("Direction: ");
                Console.WriteLine(CurrentDirection);
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
