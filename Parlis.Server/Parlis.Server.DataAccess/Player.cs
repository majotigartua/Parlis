namespace Parlis.Server.DataAccess
{  
    public partial class Player
    {
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string PlayerProfileUsername { get; set; }
    
        public virtual PlayerProfile PlayerProfile { get; set; }
    }
}