namespace Rover.Domain
{
    public class userAddress
    {
        public string Id { get; set; }
        public int regionUser { get; set; }
        public string cityUser { get; set; }
        public int postCodeUser { get; set; }
        public string streetUser { get; set; }
        public int numApartUser { get; set; }
    }


    public class bikeData
    {
        public int IDbike { get; set; } 
        public int IDmodel { get; set; } 
        public int? IDstore { get; set; } 
        public int? IDrepService { get; set; } 
        public bool availableBike { get; set; } 
        public string? IDreturnLocation { get; set; }
    }
    public class modelBike
    {
        public int IDmodel { get; set; }
        public int nameModel { get; set; }
        public string typeModel { get; set; }
        public decimal priceModel { get; set; }
        public string photoModel { get; set; }
        public int amountBike { get; set; }
        public int amountAvailable { get; set; }
    }


    public class orderUser
    {
        public int IDorder { get; set; }
        public string Id { get; set; }
        public int IDbike { get; set; }
        public int IDpayment { get; set; }
        public DateTime? totalTimeOrderStart { get; set; }
        public DateTime? totalTimeOrderFinish { get; set; }
        public DateTime? realTimeOrderStart { get; set; }
        public DateTime? realTimeOrderFinish { get; set; }
    }
}
