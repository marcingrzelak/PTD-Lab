using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        public const string DYNAMIXEL_DEVICENAME = "COM8";

        public const int DYNAMIXEL_SPEED_SCALE = 63;
        public const int WHEEL_MAX_SPEED = 600;
        public const int GRASPER_MAX_SPEED = 300;
        public const int DYNAMIXEL_TURBO_SPEED = 1023;
        public const ushort DYNAMIXEL_BACK_SPEED = 1024;
        public const int DYNAMIXEL_STOP_SPEED_BACK = 1024;

        public const ushort DYNAMIXEL_RIGHT_ARM_MIN_POSITION = 350;
        public const ushort DYNAMIXEL_RIGHT_ARM_MAX_POSITION = 673;
        public const ushort DYNAMIXEL_LEFT_ARM_MIN_POSITION = 700;
        public const ushort DYNAMIXEL_LEFT_ARM_MAX_POSITION = 1023;
        public const ushort DYNAMIXEL_TILT_ARM_MIN_POSITION = 220;
        public const ushort DYNAMIXEL_TILT_ARM_MAX_POSITION = 530;

        public static int DynamixelPortNum;
        public static ushort DynamixelWheelModeData = 0;
        public static int DynamixelCurrentSpeed = 0;
        public static int DynamixelCurrentDirection = 0;
        public static ushort DynamixelSpeed, DynamixelSpeed2;

        public static string PelcoSerialPort = "COM6";
        public static uint z = 0;

        public static bool IsWheelSterring = false;
        public static bool IsGrasperSterring = false;
        #endregion

        public static SerialPort serialPort = new SerialPort(PelcoSerialPort);
        public static Packet packet = new Packet();
        public static List<Packet> packets = new List<Packet>();
        public static DateTime dateWheels = DateTime.Now;
        public static DateTime dateGrasper = DateTime.Now;

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
                Console.WriteLine("Dynamixel " + pID + " has been successfully connected");
            }
        }

        static void DynamixelEnableWheelMode(byte pID)
        {
            int Result;
            byte Error;

            DynamixelSDK.write2ByteTxRx(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_CCW_ANGLE_LIMIT_L, DynamixelWheelModeData);

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
                Console.WriteLine("Dynamixel " + pID + " is in a wheel mode");
            }
        }

        static void DynamixelSetMovingSpeed(byte pID, ushort pMovingSpeed)
        {
            int Result;
            byte Error;

            DynamixelSDK.write2ByteTxRx(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_MOVING_SPEED, pMovingSpeed);

            if ((Result = DynamixelSDK.getLastTxRxResult(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION)) != Dynamixel.Constants.COMM_SUCCESS)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.getTxRxResult(DYNAMIXEL_PROTOCOL_VERSION, Result)));
            }
            else if ((Error = DynamixelSDK.getLastRxPacketError(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION)) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.getRxPacketError(DYNAMIXEL_PROTOCOL_VERSION, Error)));
            }
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
                Console.WriteLine("Dynamixel " + pID + " has been successfully disconnected");
            }
        }

        static void DynamixelClosePort()
        {
            DynamixelSDK.closePort(DynamixelPortNum);
        }

        static void DynamixelStopWheels()
        {
            try
            {
                DynamixelSetMovingSpeed(Dynamixel.Constants.FRONT_RIGHT_WHEEL, 0);
                DynamixelSetMovingSpeed(Dynamixel.Constants.FRONT_LEFT_WHEEL, 0);
                DynamixelSetMovingSpeed(Dynamixel.Constants.REAR_RIGHT_WHEEL, 0);
                DynamixelSetMovingSpeed(Dynamixel.Constants.REAR_LEFT_WHEEL, 0);
            }
            catch (Exception)
            {

                throw new Exception("Stopping wheels failed");
            }
        }

        private static void DynamixelStopGrasper()
        {
            DynamixelSetMovingSpeed(Dynamixel.Constants.RIGHT_ARM, 0);
            DynamixelSetMovingSpeed(Dynamixel.Constants.LEFT_ARM, 0);
            DynamixelSetMovingSpeed(Dynamixel.Constants.TILT_ARM, 0);
        }

        private static void DynamixelSetGoalPosition(byte pID, ushort pGoalPosition)
        {
            int Result;
            byte Error;

            DynamixelSDK.write2ByteTxRx(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_GOAL_POSITION_L, pGoalPosition);
            if ((Result = DynamixelSDK.getLastTxRxResult(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION)) != Dynamixel.Constants.COMM_SUCCESS)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.getTxRxResult(DYNAMIXEL_PROTOCOL_VERSION, Result)));
            }
            else if ((Error = DynamixelSDK.getLastRxPacketError(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION)) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(DynamixelSDK.getRxPacketError(DYNAMIXEL_PROTOCOL_VERSION, Error)));
            }
        }

        private static void DynamixelReadPresentPosition(byte pID)
        {
            ushort pos = DynamixelSDK.read2ByteTxRx(DynamixelPortNum, DYNAMIXEL_PROTOCOL_VERSION, pID, Dynamixel.Constants.ADDR_PRESENT_POSITION);
            Console.WriteLine("[ID: {0}] PresPos: {1}", pID, pos);
        }

        #endregion

        #region Pelco

        static void PelcoInitialization()
        {
            serialPort.BaudRate = 57600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
        }

        static void PelcoOpenPort()
        {
            serialPort.Open();
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
                        //Console.WriteLine();
                        break;
                    default:
                        break;
                }
            }
            CarMove();
            Thread.Sleep(50);
        }

        static void CarMove()
        {
            if (packets.Count > 0)
            {
                if (packets[0].Sync == Pelco.Constants.SYNC)
                {
                    if (packets[0].Address == Pelco.Constants.ADDR_1) //sterowanie kolami
                    {
                        if (packets[0].Command2 == Pelco.Constants.DRIVE_AHEAD)
                        {
                            DynamixelSpeed = SpeedByteToNumberAhead(packets[0].Data2);
                            DynamixelSpeed2 = SpeedByteToNumberBack(packets[0].Data2);
                            CarDriveStraight(DynamixelSpeed, DynamixelSpeed2);
                        }
                        else if (packets[0].Command2 == Pelco.Constants.DRIVE_BACK)
                        {
                            DynamixelSpeed2 = SpeedByteToNumberAhead(packets[0].Data2);
                            DynamixelSpeed = SpeedByteToNumberBack(packets[0].Data2);
                            CarDriveStraight(DynamixelSpeed, DynamixelSpeed2);
                        }
                        else if (packets[0].Command2 == Pelco.Constants.TURN_RIGHT)
                        {
                            DynamixelSpeed = SpeedByteToNumberAhead(packets[0].Data1);
                            CarTurn(DynamixelSpeed);
                        }
                        else if (packets[0].Command2 == Pelco.Constants.TURN_LEFT)
                        {
                            DynamixelSpeed = SpeedByteToNumberBack(packets[0].Data1);
                            CarTurn(DynamixelSpeed);
                        }
                    }
                    else if (packets[0].Address == Pelco.Constants.ADDR_2) //sterowanie chwytakiem
                    {
                        if (packets[0].Command2 == Pelco.Constants.GRASPER_UP)
                        {
                            DynamixelSpeed = SpeedByteToNumberGrasper(packets[0].Data2);
                            MoveGrasperUp(DynamixelSpeed);
                        }
                        else if (packets[0].Command2 == Pelco.Constants.GRASPER_DOWN)
                        {
                            DynamixelSpeed = SpeedByteToNumberGrasper(packets[0].Data2);
                            MoveGrasperDown(DynamixelSpeed);
                        }
                        else if (packets[0].Command2 == Pelco.Constants.OPEN_GRASPER)
                        {
                            DynamixelSpeed = SpeedByteToNumberGrasper(packets[0].Data1);
                            GrasperOpen(DynamixelSpeed);
                        }
                        else if (packets[0].Command2 == Pelco.Constants.CLOSE_GRASPER)
                        {
                            DynamixelSpeed = SpeedByteToNumberGrasper(packets[0].Data1);
                            GrasperClose(DynamixelSpeed);
                        }
                    }
                }
                packets.RemoveAt(0);
            }
        }

        static ushort SpeedByteToNumberAhead(byte pSpeed)
        {
            if (pSpeed == Pelco.Constants.TURBO_SPEED)
            {
                return DYNAMIXEL_TURBO_SPEED;
            }
            else if (pSpeed == Pelco.Constants.STOP_SPEED)
            {
                return 0;
            }
            else
            {
                ushort Speed;
                float speed = float.Parse(pSpeed.ToString());
                speed = speed / DYNAMIXEL_SPEED_SCALE * WHEEL_MAX_SPEED;
                Speed = (ushort)speed;

                return Speed;
            }
        }

        static ushort SpeedByteToNumberBack(byte pSpeed)
        {
            if (pSpeed == Pelco.Constants.TURBO_SPEED)
            {
                return DYNAMIXEL_TURBO_SPEED + DYNAMIXEL_BACK_SPEED;
            }
            else if (pSpeed == Pelco.Constants.STOP_SPEED)
            {
                return DYNAMIXEL_STOP_SPEED_BACK;
            }
            else
            {
                ushort Speed;
                float speed = float.Parse(pSpeed.ToString());
                speed = (speed / DYNAMIXEL_SPEED_SCALE * WHEEL_MAX_SPEED) + DYNAMIXEL_BACK_SPEED;
                Speed = (ushort)speed;

                return Speed;
            }
        }

        static ushort SpeedByteToNumberGrasper(byte pSpeed)
        {
            ushort Speed;
            float speed = float.Parse(pSpeed.ToString());
            speed = speed / DYNAMIXEL_SPEED_SCALE * GRASPER_MAX_SPEED;
            Speed = (ushort)speed;

            if (Speed == 0)
            {
                return 1;
            }

            return Speed;
        }

        private static void CarDriveStraight(ushort pSpeedAhead, ushort pSpeedBack)
        {
            IsWheelSterring = true;
            IsGrasperSterring = false;
            DynamixelSetMovingSpeed(Dynamixel.Constants.FRONT_RIGHT_WHEEL, pSpeedAhead);
            DynamixelSetMovingSpeed(Dynamixel.Constants.FRONT_LEFT_WHEEL, pSpeedBack);
            DynamixelSetMovingSpeed(Dynamixel.Constants.REAR_RIGHT_WHEEL, pSpeedAhead);
            DynamixelSetMovingSpeed(Dynamixel.Constants.REAR_LEFT_WHEEL, pSpeedBack);
            dateWheels = DateTime.Now;
        }

        private static void CarTurn(ushort pSpeed)
        {
            IsWheelSterring = true;
            IsGrasperSterring = false;
            DynamixelSetMovingSpeed(Dynamixel.Constants.FRONT_RIGHT_WHEEL, pSpeed);
            DynamixelSetMovingSpeed(Dynamixel.Constants.FRONT_LEFT_WHEEL, pSpeed);
            DynamixelSetMovingSpeed(Dynamixel.Constants.REAR_RIGHT_WHEEL, pSpeed);
            DynamixelSetMovingSpeed(Dynamixel.Constants.REAR_LEFT_WHEEL, pSpeed);
            dateWheels = DateTime.Now;
        }

        private static void MoveGrasperUp(ushort pSpeed)
        {
            IsWheelSterring = false;
            IsGrasperSterring = true;
            DynamixelSetGoalPosition(Dynamixel.Constants.TILT_ARM, DYNAMIXEL_TILT_ARM_MIN_POSITION);
            DynamixelSetMovingSpeed(Dynamixel.Constants.TILT_ARM, pSpeed);
            dateGrasper = DateTime.Now;
        }

        private static void MoveGrasperDown(ushort pSpeed)
        {
            IsWheelSterring = false;
            IsGrasperSterring = true;
            DynamixelSetGoalPosition(Dynamixel.Constants.TILT_ARM, DYNAMIXEL_TILT_ARM_MAX_POSITION);
            DynamixelSetMovingSpeed(Dynamixel.Constants.TILT_ARM, pSpeed);
            dateGrasper = DateTime.Now;
        }

        private static void GrasperOpen(ushort pSpeed)
        {
            IsWheelSterring = false;
            IsGrasperSterring = true;
            DynamixelSetGoalPosition(Dynamixel.Constants.RIGHT_ARM, DYNAMIXEL_RIGHT_ARM_MAX_POSITION);
            DynamixelSetMovingSpeed(Dynamixel.Constants.RIGHT_ARM, pSpeed);
            DynamixelSetGoalPosition(Dynamixel.Constants.LEFT_ARM, DYNAMIXEL_LEFT_ARM_MAX_POSITION);
            DynamixelSetMovingSpeed(Dynamixel.Constants.LEFT_ARM, pSpeed);
            dateGrasper = DateTime.Now;
        }

        private static void GrasperClose(ushort pSpeed)
        {
            IsWheelSterring = false;
            IsGrasperSterring = true;
            DynamixelSetGoalPosition(Dynamixel.Constants.RIGHT_ARM, DYNAMIXEL_RIGHT_ARM_MIN_POSITION);
            DynamixelSetMovingSpeed(Dynamixel.Constants.RIGHT_ARM, pSpeed);
            DynamixelSetGoalPosition(Dynamixel.Constants.LEFT_ARM, DYNAMIXEL_LEFT_ARM_MIN_POSITION);
            DynamixelSetMovingSpeed(Dynamixel.Constants.LEFT_ARM, pSpeed);
            dateGrasper = DateTime.Now;
        }

        #endregion

        static void Main(string[] args)
        {
            int isBytesToRead = 0;
            TimeSpan timeWheels, timeGrasper;

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
                DynamixelSetBaudRate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            try
            {
                DynamixelOpenPort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            try
            {
                DynamixelEnableWheelMode(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
                DynamixelEnableWheelMode(Dynamixel.Constants.FRONT_LEFT_WHEEL);
                DynamixelEnableWheelMode(Dynamixel.Constants.REAR_RIGHT_WHEEL);
                DynamixelEnableWheelMode(Dynamixel.Constants.REAR_LEFT_WHEEL);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            try
            {
                DynamixelEnableTorque(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
                DynamixelEnableTorque(Dynamixel.Constants.FRONT_LEFT_WHEEL);
                DynamixelEnableTorque(Dynamixel.Constants.REAR_RIGHT_WHEEL);
                DynamixelEnableTorque(Dynamixel.Constants.REAR_LEFT_WHEEL);
                DynamixelEnableTorque(Dynamixel.Constants.RIGHT_ARM);
                DynamixelEnableTorque(Dynamixel.Constants.LEFT_ARM);
                DynamixelEnableTorque(Dynamixel.Constants.TILT_ARM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            try
            {
                DynamixelStopWheels();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            try
            {
                PelcoInitialization();
                PelcoOpenPort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                isBytesToRead = serialPort.BytesToRead;
                if (isBytesToRead == 0)
                {
                    timeWheels = DateTime.Now - dateWheels;
                    timeGrasper = DateTime.Now - dateGrasper;

                    if (timeWheels.Milliseconds > 300 && IsWheelSterring)
                    {
                        DynamixelStopWheels();
                    }

                    if (timeGrasper.Milliseconds > 300 && IsGrasperSterring)
                    {
                        DynamixelStopGrasper();
                    }
                }
                else
                {
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(PelcoDataReceived);

                }
            }

            try
            {
                DynamixelDisableTorque(Dynamixel.Constants.FRONT_RIGHT_WHEEL);
                DynamixelDisableTorque(Dynamixel.Constants.FRONT_LEFT_WHEEL);
                DynamixelDisableTorque(Dynamixel.Constants.REAR_RIGHT_WHEEL);
                DynamixelDisableTorque(Dynamixel.Constants.REAR_LEFT_WHEEL);
                DynamixelDisableTorque(Dynamixel.Constants.RIGHT_ARM);
                DynamixelDisableTorque(Dynamixel.Constants.LEFT_ARM);
                DynamixelDisableTorque(Dynamixel.Constants.TILT_ARM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                DynamixelClosePort();
                serialPort.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
