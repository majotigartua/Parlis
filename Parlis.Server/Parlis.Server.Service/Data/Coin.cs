using System.Runtime.Serialization;
using System.ServiceModel;

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
        public bool FisrtLeap { get; set; }
        [DataMember]
        public bool AtFinalRow { get; set; }
        [DataMember]
        public bool IsWinner { get; set; }
        [DataMember]
        public bool IsPlaying { get; set; }
        [DataMember]
        public int NumRolls { get; set; }
        [DataMember]
        public int Poinst { get; set; }

        public Coin(int colorValue)
        {
            this.ColorTeamValue = colorValue;
            this.IsPlaying = true;

            //NORMAL
            /*
            switch (ColorTeamValue)
            {
                case 0:
                    this.AtSlot = 37;
                    this.ColorTeamText = "Red";
                    break;
                case 1:
                    this.AtSlot = 20;
                    this.ColorTeamText = "Blue";
                    break;
                case 2:
                    this.AtSlot = 54;
                    this.ColorTeamText = "Green";
                    break;
                case 3:
                    this.AtSlot = 3;
                    this.ColorTeamText = "Yellow";
                    break;
            }*/
            

            /*//Prueba EatCoin 1 Casilla Normal
            
            switch (ColorTeamValue)
            {
                case 0:
                    this.AtSlot = 37;
                    this.ColorTeamText = "Red";
                    break;
                case 1:
                    this.AtSlot = 33;
                    this.ColorTeamText = "Blue";
                    break;
                case 2:
                    this.AtSlot = 54;
                    this.ColorTeamText = "Green";
                    break;
                case 3:
                    this.AtSlot = 3;
                    this.ColorTeamText = "Yellow";
                    break;
            }
            */

            //Prueba EatCoin 1 Casilla al limte & Share
            
            switch (ColorTeamValue)
            {
                case 0:
                    this.AtSlot = 37;
                    this.ColorTeamText = "Red";
                    break;
                case 1:
                    this.AtSlot = 33;
                    this.ColorTeamText = "Blue";
                    break;
                case 2:
                    this.AtSlot = 3;
                    this.ColorTeamText = "Green";
                    FisrtLeap = true;
                    break;
                case 3:
                    this.AtSlot = 9;
                    this.ColorTeamText = "Yellow";
                    break;
            }
            
        }
    }
}
