namespace Application.API.Dtos
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
    }
}