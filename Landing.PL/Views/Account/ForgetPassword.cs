using System.ComponentModel.DataAnnotations;

namespace Landing.PL.Models
{
    public class ForgetPassword
    {
        [Required(ErrorMessage = " email is required ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
