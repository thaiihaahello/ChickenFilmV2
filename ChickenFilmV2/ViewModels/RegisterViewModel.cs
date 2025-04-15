namespace ChickenFilmV2.ViewModels
{
    public class RegisterViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        // Không cần avatar ở đây vì avatar sẽ được gán mặc định
        public string Avatar { get; set; } = "default-avatar.png"; // avatar mặc định
    }
}