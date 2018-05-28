using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Pelco;
using Dynamixel;
using System.IO.Ports;
using System.Threading;


namespace ConsoleApp
{
    class Program
    {
        #region Settings
        public const byte DYNAMIXEL_PROTOCOL_VERSION = 1;
        public const int DYNAMIXEL_BAUDRATE = 1000000;
        public const string DYNAMIXEL_DEVICENAME = "COM6";

        public const int DYNAMIXEL_SPEED_SCALE = 63;
        public const int DYNAMIXEL_MAX_SPEED = 600;
        public const int DYNAMIXEL_TURBO_SPEED = 1024;
        public const int DYNAMIXEL_BACK_SPEED = 1234;

        public static int DynamixelPortNum;
        public static ushort DynamixelWheelModeData = 0;
        public static int DynamixelCurrentSpeed = 0;
        public static int DynamixelCurrentDirection = 0;
        public static ushort DynamixelSpeed;

        public static string PelcoSerialPort = "COM6";
        public static uint z = 0;
        #endregion

        public static SerialPort serialPort = new SerialPort(PelcoSerialPort);
        public static PacketD packet = new PacketD();
        public static List<PacketD> packets = new List<PacketD>();

        #region Dynamixel

        static void DynamixelInitialization()
        {
            DynamixelPortNum = DynamixelSDK.portHandler(DYNAMIXEL_DEVICENAME);
            DynamixelSDK.packetHandler();
        }

        static void DynamixelOpenPort()
        {
            if (!DynamixelSDK.openPort(DynamixelPortNum))
                throw new Exception("Failed to open the port!");
            else
                Console.WriteLine("Succeeded to open the port!");
        }

        static void DynamixelSetBaudRate()
        {
            if (!DynamixelSDK.setBaudRate(DynamixelPortNum, DYNAMIXEL_BAUDRATE))
                throw new Exception("Failed to change the baudrate!");
            else
                Console.WriteLine("Succeeded to change the baudrate!");
        }

        static void DynamixelEnableTorque(byte pID)
        {
            int Result;
            byte Error;

            DynamixelSDK.write1ByteTxRx(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_TORQUE_ENABLE, Dynamixel.Constants.TORQUE_ENABLE);

            if ((Result = DynamixelSDK.getLastTxRxResult(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION)) != Dynamixel.Constants.COMM_SUCCESS)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.getTxRxResult(DYNAMIXEL_PROTOCOL_VERSION, Result)));
            }
            else if ((Error = DynamixelSDK.getLastRxPacketError(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION)) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.getRxPacketError(DYNAMIXEL_PROTOCOL_VERSION, Error)));
            }
            else
            {
                Console.WriteLine("Dynamixel has been successfully connected");
            }
        }

        static void DynamixelEnableWheelMode(byte pID)
        {
            //todo: rzucanie wyjatkow
            DynamixelSDK.write2ByteTxRx(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_CCW_ANGLE_LIMIT, DynamixelWheelModeData);
        }

        static void DynamixelSetMovingSpeed(byte pID, ushort pMovingSpeed)
        {
            //todo: rzucanie wyjatkow
            DynamixelSDK.write2ByteTxRx(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_MOVING_SPEED, pMovingSpeed);
        }

        static void DynamixelDisableTorque(byte pID)
        {
            int Result;
            byte Error;

            DynamixelSDK.write1ByteTxRx(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_TORQUE_ENABLE, Dynamixel.Constants.TORQUE_DISABLE);

            if ((Result = DynamixelSDK.getLastTxRxResult(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION)) != Dynamixel.Constants.COMM_SUCCESS)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.getTxRxResult(DYNAMIXEL_PROTOCOL_VERSION, Result)));
            }
            else if ((Error = DynamixelSDK.getLastRxPacketError(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION)) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.getRxPacketError(DYNAMIXEL_PROTOCOL_VERSION, Error)));
            }
            else
            {
                Console.WriteLine("Dynamixel has been successfully disconnected");
            }
        }

        static void DynamixelClosePort()
        {
            DynamixelSDK.closePort(DynamixelPortNum);
        }

        #endregion

        #region Pelco

        static void PelcoInitialization()
        {
            serialPort.BaudRate = 2400;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
        }

        static void PelcoOpenPort()
        {
            serialPort.Open();
            //todo: rzucanie wyjatkow
        }

        static void PelcoDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!serialPort.IsOpen) return;
            int bytes = serialPort.BytesToRead;
            byte[] buffer = new byte[bytes];
            serialPort.Read(buffer, 0, bytes);
            for (int i = 0; i < buffer.Length; i++)
            {
                //Console.Write(buffer[i].ToString("X2"));
                //Console.Write(" ");
                switch (z % 7)
                {
                    case 0:
                        packet.Sync = buffer[i];
                        z++;
                        break;
                    case 1:
                        packet.Address = buffer[i];
                        z++;
                        break;
                    case 2:
                        packet.Command1 = buffer[i];
                        z++;
                        break;
                    case 3:
                        packet.Command2 = buffer[i];
                        z++;
                        break;
                    case 4:
                        packet.Data1 = buffer[i];
                        z++;
                        break;
                    case 5:
                        packet.Data2 = buffer[i];
                        z++;
                        break;
                    case 6:
                        packet.Checksum = buffer[i];
                        packets.Add(packet);
                        z = 0;
                        break;
                    default:
                        break;
                }
            }
            CarMove();
            Thread.Sleep(200);
        }

        static void CarMove()
        {
            byte Checksum = CommandsD.ChecksumCalc(packets[0].Sync, packets[0].Address, packets[0].Command1, packets[0].Command2, packets[0].Data1, packets[0].Data2);
            if (packets[0].Checksum == Checksum)
            {
                if (packets[0].Sync == ConstantsD.SYNC)
                {
                    if (packets[0].Address == ConstantsD.ADDR_1)
                    {
                        if (packets[0].Command2 == ConstantsD.DRIVE_AHEAD)
                        {
                            DynamixelSpeed = SpeedByteToNumberAhead(packets[0].Data2);
                            CarDriveStraight(DynamixelSpeed);
                        }
                        else if (packets[0].Command2 == ConstantsD.DRIVE_BACK)
                        {
                            DynamixelSpeed = SpeedByteToNumberBack(packets[0].Data2);
                            CarDriveStraight(DynamixelSpeed);
                        }
                        else if (packets[0].Command2 == ConstantsD.TURN_RIGHT)
                        {
                            //predkosc z data1
                        }
                        else if (packets[0].Command2 == ConstantsD.TURN_LEFT)
                        {
                            //predkosc z data1
                        }
                    }
                }
            }
            else
            {
                //blad obliczania checksumy
            }
        }

        static ushort SpeedByteToNumberAhead(byte pSpeed)
        {
            if (pSpeed == ConstantsD.TURBO_SPEED)
            {
                return DYNAMIXEL_TURBO_SPEED;
            }
            else if (pSpeed == ConstantsD.STOP_SPEED)
            {
                return 0;
            }
            else
            {
                ushort Speed;
                float speed = float.Parse(pSpeed.ToString());
                speed = speed / DYNAMIXEL_SPEED_SCALE * DYNAMIXEL_MAX_SPEED;
                Speed = (ushort)speed;

                return Speed;
            }
        }

        static ushort SpeedByteToNumberBack(byte pSpeed)
        {
            if (pSpeed == ConstantsD.TURBO_SPEED)
            {
                return DYNAMIXEL_TURBO_SPEED + DYNAMIXEL_BACK_SPEED;
            }
            else if (pSpeed == ConstantsD.STOP_SPEED)
            {
                return 0;
            }
            else
            {
                ushort Speed;
                float speed = float.Parse(pSpeed.ToString());
                speed = (speed / DYNAMIXEL_SPEED_SCALE * DYNAMIXEL_MAX_SPEED) + DYNAMIXEL_BACK_SPEED;
                Speed = (ushort)speed;

                return Speed;
            }            
        }

        private static void CarDriveStraight(ushort pSpeed)
        {
            DynamixelSetMovingSpeed(Constants.FRONT_RIGHT_WHEEL, pSpeed);
            DynamixelSetMovingSpeed(Constants.FRONT_LEFT_WHEEL, pSpeed);
            DynamixelSetMovingSpeed(Constants.REAR_RIGHT_WHEEL, pSpeed);
            DynamixelSetMovingSpeed(Constants.REAR_LEFT_WHEEL, pSpeed);
        }

        #endregion

        static void Main(string[] args)
        {
            try
            {
                DynamixelInitialization();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                DynamixelOpenPort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                DynamixelSetBaudRate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DynamixelEnableWheelMode(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
            DynamixelEnableWheelMode(Dynamixel.Constants.FRONT_LEFT_WHEEL);
            DynamixelEnableWheelMode(Dynamixel.Constants.REAR_RIGHT_WHEEL);
            DynamixelEnableWheelMode(Dynamixel.Constants.REAR_LEFT_WHEEL);

            DynamixelSetMovingSpeed(Dynamixel.Constants.FRONT_RIGHT_WHEEL, 300);
            DynamixelSetMovingSpeed(Dynamixel.Constants.FRONT_LEFT_WHEEL, 1324);
            DynamixelSetMovingSpeed(Dynamixel.Constants.REAR_RIGHT_WHEEL, 300);
            DynamixelSetMovingSpeed(Dynamixel.Constants.REAR_LEFT_WHEEL, 1324);

            try
            {
                DynamixelEnableTorque(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
                DynamixelEnableTorque(Dynamixel.Constants.FRONT_LEFT_WHEEL);
                DynamixelEnableTorque(Dynamixel.Constants.REAR_RIGHT_WHEEL);
                DynamixelEnableTorque(Dynamixel.Constants.REAR_LEFT_WHEEL);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                serialPort.DataReceived += new SerialDataReceivedEventHandler(PelcoDataReceived);

                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
            }

            try
            {
                DynamixelDisableTorque(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
                DynamixelDisableTorque(Dynamixel.Constants.FRONT_LEFT_WHEEL);
                DynamixelDisableTorque(Dynamixel.Constants.REAR_RIGHT_WHEEL);
                DynamixelDisableTorque(Dynamixel.Constants.REAR_LEFT_WHEEL);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DynamixelClosePort();

        }
    }
}
