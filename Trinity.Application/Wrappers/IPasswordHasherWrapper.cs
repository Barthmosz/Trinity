namespace Trinity.Application.Wrappers
{
    public interface IPasswordHasherWrapper
    {
        bool Verify(string hash, string password, short keySize = 32, int iterations = 10000, char splitChar = '.', string privateKey = "");
    }
}
