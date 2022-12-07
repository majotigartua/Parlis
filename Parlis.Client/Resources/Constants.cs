using System.Windows;

namespace Parlis.Client.Resources
{
    public class Constants
    {
        public const int NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH = 0;
        public const int MINIUM_OF_PLAYER_PROFILES_PER_MATCH = 1;
        public const int NUMBER_OF_PLAYER_PROFILES_PER_MATCH = 4;

        public const int THROW_DICE_CODE = 0;
        public const int MOVE_COIN_CODE = 1;
        public const int EAT_COIN_CODE = 2;
        public const int SHARE_SLOT_CODE = 3;
        public const int GO_TO_HOME_SLOT_CODE = 4;
        public const int COLOR_PATH_CODE = 5;
        public const int NEXT_TURN_CODE= 6;
        public const int BUMMER_CODE = 7;
        public const int WINNER_CODE = 8;

        public static Point[] HomeSlotCordinates = { new Point(25, 35), new Point(25, 175), new Point(90, 35), new Point(90, 175) };
        public static Point[] RedPathSlots = { new Point(6.3, 105.6), new Point(11, 105.6), new Point(15.8, 105.6), new Point(20.3, 105.6), new Point(25, 105.6), new Point(29.8, 105.6), new Point(34.3, 105.6), };
        public static Point[] BluePathSlots = {new Point(48, 195), new Point(48, 185), new Point(48, 175), new Point(48, 165), new Point(48, 155), new Point(48, 145), new Point(48, 135), };
        public static Point[] GreenPathSlots = {new Point(48, 16), new Point(48, 26), new Point(48, 36), new Point(48, 45.8), new Point(48, 56), new Point(48, 66), new Point(48, 75), };
        public static Point[] YellowPathSlots = {new Point(89.8, 105.5), new Point(85, 105.5), new Point(80.3, 105.5), new Point(75.8, 105.5), new Point(71, 105.5), new Point(66.3, 105.5), new Point(61.8, 105.5), };
        public static int[] InitialSlots = { 37, 20, 54, 3 };
        public static int[] SafeSlots = { 4, 11, 16, 21, 28, 33, 38, 45, 50, 55, 62, 67 };
        public static int[] InitialColorPathSlot = { 33, 16, 50, 67 };

        public static Point[] BoardSlots =
        {   new Point(94.3, 129),
            new Point(89.8, 129),
            new Point(85, 129), 
            new Point(80.3, 129),
            new Point(75.8 ,129),
            new Point(71, 129),
            new Point(66.3, 129),
            new Point(61.8, 129),
            new Point(59, 136),
            new Point(59, 145),
            new Point(59, 155),
            new Point(59, 165),
            new Point(59, 175),
            new Point(59, 185),
            new Point(59, 195),
            new Point(59, 205.5),
            new Point(48, 205.5),
            new Point(37, 205.5),
            new Point(37, 195),
            new Point(37, 185),
            new Point(37, 175),
            new Point(37, 165),
            new Point(37, 155),
            new Point(37, 145),
            new Point(37, 136),
            new Point(34.3, 129),
            new Point(29.8, 129),
            new Point(25, 129),
            new Point(20.3, 129),
            new Point(15.8, 129),
            new Point(11, 129),
            new Point(6.3, 129),
            new Point(1.8, 129),
            new Point(1.8, 105.6),
            new Point(1.8, 82.3),
            new Point(6.3, 82.3),
            new Point(11, 82.3),
            new Point(15.8, 82.3),
            new Point(20.3, 82.3),
            new Point(25, 82.3),
            new Point(29.8, 82.3),
            new Point(34.3, 82.3),
            new Point(37, 75),
            new Point(37, 66),
            new Point(37, 56),
            new Point(37, 45.8),
            new Point(37, 36),
            new Point(37, 26),
            new Point(37, 16),
            new Point(37, 5.4),
            new Point(48, 5.4),
            new Point(59, 5.4),
            new Point(59, 16),
            new Point(59, 26),
            new Point(59, 36),
            new Point(59, 45.8),
            new Point(59, 56),
            new Point(59, 66),
            new Point(59, 75),
            new Point(61.8, 82.3),
            new Point(66.3, 82.3),
            new Point(71, 82.3),
            new Point(75.8, 82.3),
            new Point(80.3, 82.3),
            new Point(85, 82.3),
            new Point(89.8, 82.3),
            new Point(94.3, 82.3),
            new Point(94.3, 105.5) 
        }; 
    }
}
