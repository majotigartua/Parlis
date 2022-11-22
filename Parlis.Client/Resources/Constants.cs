using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Parlis.Client.Resources
{
    internal class Constants
    {
        //HOME SLOTS
        public static Point[] HOME_SLOTS_CORDINATES = { new Point(25, 35), new Point(25, 175), new Point(90, 35), new Point(90, 175) };
        //INITIAL SLOTS
        public static int[] INITIAL_SLOTS = { 37,20,54,3};
        //SAFE SLOTS
            //public static Point[] SAFE_SLOTS_CORDINATES = { new Point(75.8, 129), new Point(59, 165), new Point(48, 205.5), new Point(37, 165), new Point(20.3,129), new Point(1.8,105.6), new Point(20.3,82.3), new Point(37,45.8), new Point(48,5.4), new Point(59,45.8), new Point(75.8,82.3), new Point(94.3,105.5), };
        public static int[] SAFE_SLOTS = {4,11,16,21,28,33,38,45,50,55,62,67};
        //COLOR PATH SLOTS
        public static int[] INITIAL_COLOR_PATH_SLOT = { 33, 16, 50, 67 };
        public static Point[] RED_PATH_SLOTS = { new Point(6.3, 105.6), new Point(11, 105.6), new Point(15.8, 105.6), new Point(20.3, 105.6), new Point(25, 105.6), new Point(29.8, 105.6), new Point(34.3, 105.6), };
        public static Point[] BLUE_PATH_SLOTS = {new Point(48, 195), new Point(48, 185), new Point(48, 175), new Point(48, 165), new Point(48, 155), new Point(48, 145), new Point(48, 135), };
        public static Point[] GREEN_PATH_SLOTS = {new Point(48, 16), new Point(48, 26), new Point(48, 36), new Point(48, 45.8), new Point(48, 56), new Point(48, 66), new Point(48, 75), };
        public static Point[] YELLOW_PATH_SLOTS = {new Point(89.8, 105.5), new Point(85, 105.5), new Point(80.3, 105.5), new Point(75.8, 105.5), new Point(71, 105.5), new Point(66.3, 105.5), new Point(61.8, 105.5), };

        //BOARD SLOTS
        public static Point[] BOARD_SLOTS =
        {   new Point(94.3,129), //SLOT 1, INDEX 0
            new Point(89.8,129), //SLOT 2, INDEX 1
            new Point(85,129), //SLOT 3, INDEX 2
            new Point(80.3,129), //SLOT 4, INDEX 3
            new Point(75.8,129), //SLOT 5, INDEX 4
            new Point(71,129), //SLOT 6, INDEX 5
            new Point(66.3,129), //SLOT 7, INDEX 6
            new Point(61.8,129), //SLOT 8, INDEX 7
            new Point(59,136), //SLOT 9, INDEX 8
            new Point(59,145), //SLOT 10, INDEX 9
            new Point(59,155), //SLOT 11, INDEX 10
            new Point(59,165), //SLOT 12, INDEX 11
            new Point(59,175), //SLOT 13, INDEX 12
            new Point(59,185), //SLOT 14, INDEX 13
            new Point(59,195), //SLOT 15, INDEX 14
            new Point(59,205.5), //SLOT 16, INDEX 15
            new Point(48,205.5), //SLOT 17, INDEX 16
            new Point(37,205.5), //SLOT 18, INDEX 17
            new Point(37,195), //SLOT 19, INDEX 18
            new Point(37,185), //SLOT 20, INDEX 19
            new Point(37,175), //SLOT 21, INDEX 20
            new Point(37,165), //SLOT 22, INDEX 21
            new Point(37,155), //SLOT 23, INDEX 22
            new Point(37,145), //SLOT 24, INDEX 23
            new Point(37,136), //SLOT 25, INDEX 24
            new Point(34.3,129), //SLOT 26, INDEX 25
            new Point(29.8,129), //SLOT 27, INDEX 26
            new Point(25,129), //SLOT 28, INDEX 27
            new Point(20.3,129), //SLOT 29, INDEX 28
            new Point(15.8,129), //SLOT 30, INDEX 29
            new Point(11,129), //SLOT 31, INDEX 30
            new Point(6.3,129), //SLOT 32, INDEX 31
            new Point(1.8,129), //SLOT 33, INDEX 32
            new Point(1.8,105.6), //SLOT 34, INDEX 33
            new Point(1.8,82.3), //SLOT 35, INDEX 34
            new Point(6.3,82.3), //SLOT 36, INDEX 35
            new Point(11,82.3), //SLOT 37, INDEX 36
            new Point(15.8,82.3), //SLOT 38, INDEX 37
            new Point(20.3,82.3), //SLOT 39, INDEX 38
            new Point(25,82.3), //SLOT 40, INDEX 39
            new Point(29.8,82.3), //SLOT 41, INDEX 40
            new Point(34.3,82.3), //SLOT 42, INDEX 41
            new Point(37,75), //SLOT 43, INDEX 42
            new Point(37,66), //SLOT 44, INDEX 43
            new Point(37,56), //SLOT 45, INDEX 44
            new Point(37,45.8), //SLOT 46, INDEX 45
            new Point(37,36), //SLOT 47, INDEX 46
            new Point(37,26), //SLOT 48, INDEX 47
            new Point(37,16), //SLOT 49, INDEX 48
            new Point(37,5.4), //SLOT 50, INDEX 49
            new Point(48,5.4), //SLOT 51, INDEX 50
            new Point(59,5.4), //SLOT 52, INDEX 51
            new Point(59,16), //SLOT 53, INDEX 52
            new Point(59,26), //SLOT 54, INDEX 53
            new Point(59,36), //SLOT 55, INDEX 54
            new Point(59,45.8), //SLOT 56, INDEX 55
            new Point(59,56), //SLOT 57, INDEX 56
            new Point(59,66), //SLOT 58, INDEX 57
            new Point(59,75), //SLOT 59, INDEX 58
            new Point(61.8,82.3), //SLOT 60, INDEX 59
            new Point(66.3,82.3), //SLOT 61, INDEX 60
            new Point(71,82.3), //SLOT 62, INDEX 61
            new Point(75.8,82.3), //SLOT 63, INDEX 62
            new Point(80.3,82.3), //SLOT 64, INDEX 63
            new Point(85,82.3), //SLOT 65, INDEX 64
            new Point(89.8,82.3), //SLOT 66, INDEX 65
            new Point(94.3,82.3), //SLOT 67, INDEX 66
            new Point(94.3,105.5)   }; //SLOT 68, INDEX 67
    }
}
