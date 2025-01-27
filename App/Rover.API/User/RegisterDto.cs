using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string emailUser { get; set; }

    // przynajmniej jedna mała litera, jedna duża litera, jedna cyfra i długość pomiędzy 4-8 znaków
    [Required]
    [RegularExpression("(?=^.{4,8}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Password must be complex")]
    public string passwordUser { get; set; }

    [Required]
    public string telnumUser { get; set; }

    [Required]
    public string nameUser { get; set; }

    [Required]
    public string surnameUser { get; set; }

    [Required]
    public DateTime birthdayUser { get; set; }

    [Required]
    public string sexUser { get; set; }
}