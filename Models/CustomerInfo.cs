namespace CustomerInfoNoSQL.Models;

public class CustomerInfo
{
    public string id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public long BirthdayInEpoch { get; set; }
    public string Email {get; set; }
}