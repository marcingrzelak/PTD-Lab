using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamixel
{
    class Constants
    {
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
        public const byte REAR_RIGTH_WHEEL = 3;
        public const byte REAR_LEFT_WHEEL = 1;

        public const int TORQUE_ENABLE = 1;
        public const int TORQUE_DISABLE = 0;
        public const int COMM_SUCCESS = 0;
        public const int COMM_TX_FAIL = -1001;

    }
}
