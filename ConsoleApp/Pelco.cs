namespace Pelco
{
    class Constants
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
    class Packet
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