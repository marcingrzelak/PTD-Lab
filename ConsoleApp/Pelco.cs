using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pelco
{
    class Constants
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
    class Commands
    {
        byte checksum;
        void ChecksumCalc(byte pSTX, byte pCamAddr, byte pData1, byte pData2, byte pData3, byte pData4, byte pETX)
        {
            checksum = (byte)(pSTX ^ pCamAddr ^ pData1 ^ pData2 ^ pData3 ^ pData4 ^ pETX);
        }
    }
}
