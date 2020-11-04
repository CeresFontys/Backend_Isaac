namespace Isaac_AccountService
{
    public enum Role
    {
        Admin,
        User
    }

    public class User
    {
        public int Id { get; private set; }

        public string Email { get; private set; }

        public string Name { get; private set; }

        public string PasswordHash { get; private set; }

        public string PasswordSalt { get; private set; }

        public Role Role { get; private set; }
    }
}