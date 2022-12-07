using System.Runtime.Serialization;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Coin
    {
        [DataMember]
        public string PlayerProfileUsername { get; set; }
        [DataMember]
        public int ColorTeamValue { get; set; }
        [DataMember]
        public string ColorTeamText { get; set; }
        [DataMember]
        public int AtSlot { get; set; }
        [DataMember]
        public bool FirstLeap { get; set; }
        [DataMember]
        public bool AtFinalRow { get; set; }
        [DataMember]
        public bool IsWinner { get; set; }
        [DataMember]
        public bool IsPlaying { get; set; }
        [DataMember]
        public int NumRolls { get; set; }
        [DataMember]
        public int Points { get; set; }

        public Coin(int colorValue)
        {
            ColorTeamValue = colorValue;
            IsPlaying = true;      
            switch (ColorTeamValue)
            {
                case Constants.RED_COIN_CODE:
                    AtSlot = 37;
                    ColorTeamText = "Red";
                    break;
                case Constants.BLUE_COIN_CODE:
                    AtSlot = 33;
                    ColorTeamText = "Blue";
                    break;
                case Constants.GREEN_COIN_CODE:
                    AtSlot = 3;
                    ColorTeamText = "Green";
                    FirstLeap = true;
                    break;
                case Constants.YELLOW_COIN_CODE:
                    AtSlot = 9;
                    ColorTeamText = "Yellow";
                    break;
            }  
        }
    }
}