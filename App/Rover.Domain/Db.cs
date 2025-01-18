namespace Rover.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PhoneNunber { get; set; }
        public string Email { get; set; }
        public string MainCity { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class Bike
    {
        public Guid BikeId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
    }
}
