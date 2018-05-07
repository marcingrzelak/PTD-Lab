/*******************************************************************************
* Copyright 2017 ROBOTIS CO., LTD.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*******************************************************************************/

/* Author: Ryu Woon Jung (Leon) */

using System;
using System.Runtime.InteropServices;

namespace Dynamixel
{
    class DynamixelSDK
    {
        const string DllPath = @"..\..\..\dxl_x64_c.dll";
        //const string DllPath = @"..\..\..\dxl_x86_c.dll";

        #region PortHandler
        [DllImport(DllPath)]
        public static extern int PortHandler(string port_name);

        [DllImport(DllPath)]
        public static extern bool OpenPort(int port_num);
        [DllImport(DllPath)]
        public static extern void ClosePort(int port_num);
        [DllImport(DllPath)]
        public static extern void ClearPort(int port_num);

        [DllImport(DllPath)]
        public static extern void SetPortName(int port_num, string port_name);
        [DllImport(DllPath)]
        public static extern string GetPortName(int port_num);

        [DllImport(DllPath)]
        public static extern bool SetBaudRate(int port_num, int baudrate);
        [DllImport(DllPath)]
        public static extern int GetBaudRate(int port_num);

        [DllImport(DllPath)]
        public static extern int ReadPort(int port_num, byte[] packet, int length);
        [DllImport(DllPath)]
        public static extern int WritePort(int port_num, byte[] packet, int length);

        [DllImport(DllPath)]
        public static extern void SetPacketTimeout(int port_num, UInt16 packet_length);
        [DllImport(DllPath)]
        public static extern void SetPacketTimeoutMSec(int port_num, double msec);
        [DllImport(DllPath)]
        public static extern bool IsPacketTimeout(int port_num);
        #endregion

        #region PacketHandler
        [DllImport(DllPath)]
        public static extern void PacketHandler();

        [DllImport(DllPath)]
        public static extern void PrintTxRxResult(int protocol_version, int result);
        [DllImport(DllPath)]
        public static extern IntPtr GetTxRxResult(int protocol_version, int result);
        [DllImport(DllPath)]
        public static extern void PrintRxPacketError(int protocol_version, byte error);
        [DllImport(DllPath)]
        public static extern IntPtr GetRxPacketError(int protocol_version, byte error);

        [DllImport(DllPath)]
        public static extern int GetLastTxRxResult(int port_num, int protocol_version);
        [DllImport(DllPath)]
        public static extern byte GetLastRxPacketError(int port_num, int protocol_version);

        [DllImport(DllPath)]
        public static extern void SetDataWrite(int port_num, int protocol_version, UInt16 data_length, UInt16 data_pos, UInt32 data);
        [DllImport(DllPath)]
        public static extern UInt32 GetDataRead(int port_num, int protocol_version, UInt16 data_length, UInt16 data_pos);

        [DllImport(DllPath)]
        public static extern void TxPacket(int port_num, int protocol_version);

        [DllImport(DllPath)]
        public static extern void RxPacket(int port_num, int protocol_version);

        [DllImport(DllPath)]
        public static extern void TxRxPacket(int port_num, int protocol_version);

        [DllImport(DllPath)]
        public static extern void Ping(int port_num, int protocol_version, byte id);

        [DllImport(DllPath)]
        public static extern UInt16 PingGetModelNum(int port_num, int protocol_version, byte id);

        [DllImport(DllPath)]
        public static extern void BroadcastPing(int port_num, int protocol_version);
        [DllImport(DllPath)]
        public static extern bool GetBroadcastPingResult(int port_num, int protocol_version, int id);

        [DllImport(DllPath)]
        public static extern void Reboot(int port_num, int protocol_version, byte id);

        [DllImport(DllPath)]
        public static extern void FactoryReset(int port_num, int protocol_version, byte id, byte option);

        [DllImport(DllPath)]
        public static extern void ReadTx(int port_num, int protocol_version, byte id, UInt16 address, UInt16 length);
        [DllImport(DllPath)]
        public static extern void ReadRx(int port_num, int protocol_version, UInt16 length);
        [DllImport(DllPath)]
        public static extern void ReadTxRx(int port_num, int protocol_version, byte id, UInt16 address, UInt16 length);

        [DllImport(DllPath)]
        public static extern void Read1ByteTx(int port_num, int protocol_version, byte id, UInt16 address);
        [DllImport(DllPath)]
        public static extern byte Read1ByteRx(int port_num, int protocol_version);
        [DllImport(DllPath)]
        public static extern byte Read1ByteTxRx(int port_num, int protocol_version, byte id, UInt16 address);

        [DllImport(DllPath)]
        public static extern void Read2ByteTx(int port_num, int protocol_version, byte id, UInt16 address);
        [DllImport(DllPath)]
        public static extern UInt16 Read2ByteRx(int port_num, int protocol_version);
        [DllImport(DllPath)]
        public static extern UInt16 Read2ByteTxRx(int port_num, int protocol_version, byte id, UInt16 address);

        [DllImport(DllPath)]
        public static extern void Read4ByteTx(int port_num, int protocol_version, byte id, UInt16 address);
        [DllImport(DllPath)]
        public static extern UInt32 Read4ByteRx(int port_num, int protocol_version);
        [DllImport(DllPath)]
        public static extern UInt32 Read4ByteTxRx(int port_num, int protocol_version, byte id, UInt16 address);

        [DllImport(DllPath)]
        public static extern void WriteTxOnly(int port_num, int protocol_version, byte id, UInt16 address, UInt16 length);
        [DllImport(DllPath)]
        public static extern void WriteTxRx(int port_num, int protocol_version, byte id, UInt16 address, UInt16 length);

        [DllImport(DllPath)]
        public static extern void Write1ByteTxOnly(int port_num, int protocol_version, byte id, UInt16 address, byte data);
        [DllImport(DllPath)]
        public static extern void Write1ByteTxRx(int port_num, int protocol_version, byte id, UInt16 address, byte data);

        [DllImport(DllPath)]
        public static extern void Write2ByteTxOnly(int port_num, int protocol_version, byte id, UInt16 address, UInt16 data);
        [DllImport(DllPath)]
        public static extern void Write2ByteTxRx(int port_num, int protocol_version, byte id, UInt16 address, UInt16 data);

        [DllImport(DllPath)]
        public static extern void Write4ByteTxOnly(int port_num, int protocol_version, byte id, UInt16 address, UInt32 data);
        [DllImport(DllPath)]
        public static extern void Write4ByteTxRx(int port_num, int protocol_version, byte id, UInt16 address, UInt32 data);

        [DllImport(DllPath)]
        public static extern void RegWriteTxOnly(int port_num, int protocol_version, byte id, UInt16 address, UInt16 length);
        [DllImport(DllPath)]
        public static extern void RegWriteTxRx(int port_num, int protocol_version, byte id, UInt16 address, UInt16 length);

        [DllImport(DllPath)]
        public static extern void SyncReadTx(int port_num, int protocol_version, UInt16 start_address, UInt16 data_length, UInt16 param_length);
        // syncReadRx   -> GroupSyncRead
        // syncReadTxRx -> GroupSyncRead

        [DllImport(DllPath)]
        public static extern void SyncWriteTxOnly(int port_num, int protocol_version, UInt16 start_address, UInt16 data_length, UInt16 param_length);

        [DllImport(DllPath)]
        public static extern void BulkReadTx(int port_num, int protocol_version, UInt16 param_length);
        // bulkReadRx   -> GroupBulkRead
        // bulkReadTxRx -> GroupBulkRead

        [DllImport(DllPath)]
        public static extern void BulkWriteTxOnly(int port_num, int protocol_version, UInt16 param_length);
        #endregion

        #region GroupBulkRead
        [DllImport(DllPath)]
        public static extern int GroupBulkRead(int port_num, int protocol_version);

        [DllImport(DllPath)]
        public static extern bool GroupBulkReadAddParam(int group_num, byte id, UInt16 start_address, UInt16 data_length);
        [DllImport(DllPath)]
        public static extern void GroupBulkReadRemoveParam(int group_num, byte id);
        [DllImport(DllPath)]
        public static extern void GroupBulkReadClearParam(int group_num);

        [DllImport(DllPath)]
        public static extern void GroupBulkReadTxPacket(int group_num);
        [DllImport(DllPath)]
        public static extern void GroupBulkReadRxPacket(int group_num);
        [DllImport(DllPath)]
        public static extern void GroupBulkReadTxRxPacket(int group_num);

        [DllImport(DllPath)]
        public static extern bool GroupBulkReadIsAvailable(int group_num, byte id, UInt16 address, UInt16 data_length);
        [DllImport(DllPath)]
        public static extern UInt32 GroupBulkReadGetData(int group_num, byte id, UInt16 address, UInt16 data_length);
        #endregion

        #region GroupBulkWrite
        [DllImport(DllPath)]
        public static extern int GroupBulkWrite(int port_num, int protocol_version);

        [DllImport(DllPath)]
        public static extern bool GroupBulkWriteAddParam(int group_num, byte id, UInt16 start_address, UInt16 data_length, UInt32 data, UInt16 input_length);
        [DllImport(DllPath)]
        public static extern void GroupBulkWriteRemoveParam(int group_num, byte id);
        [DllImport(DllPath)]
        public static extern bool GroupBulkWriteChangeParam(int group_num, byte id, UInt16 start_address, UInt16 data_length, UInt32 data, UInt16 input_length, UInt16 data_pos);
        [DllImport(DllPath)]
        public static extern void GroupBulkWriteClearParam(int group_num);

        [DllImport(DllPath)]
        public static extern void GroupBulkWriteTxPacket(int group_num);
        #endregion

        #region GroupSyncRead
        [DllImport(DllPath)]
        public static extern int GroupSyncRead(int port_num, int protocol_version, UInt16 start_address, UInt16 data_length);

        [DllImport(DllPath)]
        public static extern bool GroupSyncReadAddParam(int group_num, byte id);
        [DllImport(DllPath)]
        public static extern void GroupSyncReadRemoveParam(int group_num, byte id);
        [DllImport(DllPath)]
        public static extern void GroupSyncReadClearParam(int group_num);

        [DllImport(DllPath)]
        public static extern void GroupSyncReadTxPacket(int group_num);
        [DllImport(DllPath)]
        public static extern void GroupSyncReadRxPacket(int group_num);
        [DllImport(DllPath)]
        public static extern void GroupSyncReadTxRxPacket(int group_num);

        [DllImport(DllPath)]
        public static extern bool GroupSyncReadIsAvailable(int group_num, byte id, UInt16 address, UInt16 data_length);
        [DllImport(DllPath)]
        public static extern UInt32 GroupSyncReadGetData(int group_num, byte id, UInt16 address, UInt16 data_length);
        #endregion

        #region GroupSyncWrite
        [DllImport(DllPath)]
        public static extern int GroupSyncWrite(int port_num, int protocol_version, UInt16 start_address, UInt16 data_length);

        [DllImport(DllPath)]
        public static extern bool GroupSyncWriteAddParam(int group_num, byte id, UInt32 data, UInt16 data_length);
        [DllImport(DllPath)]
        public static extern void GroupSyncWriteRemoveParam(int group_num, byte id);
        [DllImport(DllPath)]
        public static extern bool GroupSyncWriteChangeParam(int group_num, byte id, UInt32 data, UInt16 data_length, UInt16 data_pos);
        [DllImport(DllPath)]
        public static extern void GroupSyncWriteClearParam(int group_num);

        [DllImport(DllPath)]
        public static extern void GroupSyncWriteTxPacket(int group_num);
        #endregion
    }

    class Constants
    {
        public const byte ADDR_CCW_ANGLE_LIMIT = 8;
        public const byte ADDR_TORQUE_ENABLE = 24;
        public const byte ADDR_LED_ENABLE = 25;
        public const byte ADDR_GOAL_POSITION = 30;
        public const byte ADDR_MOVING_SPEED = 32;
        public const byte ADDR_TORQUE_LIMIT = 34;
        public const byte ADDR_PRESENT_POSITION = 36;
        public const byte ADDR_PRESENT_SPEED = 38;
        public const byte ADDR_MOVING = 46;

        public const byte FRONT_RIGHT_WHEEL = 4;
        public const byte FRONT_LEFT_WHEEL = 2;
        public const byte REAR_RIGHT_WHEEL = 3;
        public const byte REAR_LEFT_WHEEL = 1;

        public const int TORQUE_ENABLE = 1;
        public const int TORQUE_DISABLE = 0;
        public const int COMM_SUCCESS = 0;
        public const int COMM_TX_FAIL = -1001;
    };
}
