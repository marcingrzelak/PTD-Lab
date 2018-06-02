using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Pelco
{
    class ConstantsP
    {
        const byte STX = 0xA0;
        const byte ETX = 0xAF;
        const byte CAM_ADDR_1 = 0x00;
        const byte DATA_1 = 0x00;

        const byte STOP_SPEED = 0x00;
        const byte HIGH_SPEED = 0x3f;
        const byte TURBO_SPEED = 0x40;

        const byte TURN_LEFT = 0x04;
        const byte TURN_RIGHT = 0x02;
        const byte DRIVE_AHEAD = 0x08;
        const byte DRIVE_BACK = 0x10;
    }
    class CommandsP
    {

    }
    class PacketP
    {

    }
    class ConstantsD
    {
        public const byte SYNC = 0xff;
        public const byte ADDR_1 = 0x01; //sterowanie kolami
        public const byte ADDR_2 = 0x02; //sterowanie chwytakiem

        public const byte STOP_SPEED = 0x00;
        public const byte HIGH_SPEED = 0x3f;
        public const byte TURBO_SPEED = 0xff;

        public const byte TURN_LEFT = 0x04;
        public const byte TURN_RIGHT = 0x02;
        public const byte DRIVE_AHEAD = 0x08;
        public const byte DRIVE_BACK = 0x10;
        public const byte CLOSE_GRASPER = 0x20;
        public const byte OPEN_GRASPER = 0x40;
        public const byte GRASPER_UP = 0x10;
        public const byte GRASPER_DOWN = 0x08;
    }
    class CommandsD
    {        
        public static byte ChecksumCalc(byte pSync, byte pAddress, byte pCommand1, byte pCommand2, byte pData1, byte pData2)
        {
            byte checksum;
            checksum = (byte)(pSync ^ pAddress ^ pCommand1 ^ pCommand2 ^ pData1 ^ pData2);
            return checksum;
        }

        
    }
    class PacketD
    {
        public byte Sync { get; set; }
        public byte Address { get; set; }
        public byte Command1 { get; set; }
        public byte Command2 { get; set; }
        public byte Data1 { get; set; }
        public byte Data2 { get; set; }
        public byte Checksum { get; set; }
    }



}
