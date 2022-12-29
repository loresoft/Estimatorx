namespace EstimatorX.Core.Services;

[RegisterSingleton]
public class SecurityKeyGenerator : ISecurityKeyGenerator
{
    private const string _alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public string GenerateKey()
    {
        return Nanoid.Nanoid.Generate(_alphabet, 12);
    }
}
