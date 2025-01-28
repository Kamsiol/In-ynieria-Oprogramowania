public class ReserveRequest
{
    public required int Id { get; set; }
    public required int BikeId { get; set; }
    public int IDpayment { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }

    public void Validate()
    {
        if (EndTime <= StartTime)
        {
            throw new ArgumentException("EndTime must be greater than StartTime.");
        }

        if (StartTime < DateTime.UtcNow)
        {
            throw new ArgumentException("StartTime must be in the future.");
        }
    }
}
