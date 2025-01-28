using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rover.Domain
{
    public class userAddress
    {
        public int Id { get; set; }
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
        public int amountBike { get; set; }
    }


    public class orderUser
    {
        [Key]
        public int IDorder { get; set; }
        public int Id { get; set; }
        public int IDbike { get; set; }
        public int IDpayment { get; set; }
        public DateTime? totalTimeOrderStart { get; set; }
        public DateTime? totalTimeOrderFinish { get; set; }
    }

    public class userData
    {
        [Key]
        public int Id { get; set; } // Убираем атрибут [DatabaseGenerated]
        public string emailUser { get; set; }
        public int telnumUser { get; set; }
        public string passwordUser { get; set; }
        public string nameUser { get; set; }
        public string surnameUser { get; set; }
        public DateTime birthdayUser { get; set; }
        public string sexUser { get; set; }
    }
}
