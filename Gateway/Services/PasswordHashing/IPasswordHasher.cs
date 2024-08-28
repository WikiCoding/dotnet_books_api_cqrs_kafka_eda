namespace Gateway.Services.PasswordHashing
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string requestPassword, string hashedPassword);
    }
}
