using System.Runtime.Serialization;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Coin
    {
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
        [DataMember]
        public string PlayerProfileUsername { get; set; }

        public Coin(int code)
        {
            ColorTeamValue = code;
            IsPlaying = true;      
            switch (ColorTeamValue)
            {
                case Constants.RED_COIN_CODE:
                    AtSlot = Constants.INITIAL_RED_COIN_SLOT;
                    ColorTeamText = "Red";
                    break;
                case Constants.BLUE_COIN_CODE:
                    AtSlot = Constants.INITIAL_BLUE_COIN_SLOT;
                    ColorTeamText = "Blue";
                    break;
                case Constants.GREEN_COIN_CODE:
                    AtSlot = Constants.INITIAL_GREEN_COIN_SLOT;
                    ColorTeamText = "Green";
                    break;
                case Constants.YELLOW_COIN_CODE:
                    AtSlot = Constants.INITIAL_YELLOW_COIN_SLOT;
                    ColorTeamText = "Yellow";
                    break;
            }  
        }
    }
}