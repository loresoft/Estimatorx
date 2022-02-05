using EstimatorX.Shared.Definitions;

namespace EstimatorX.Core.Services;

public class SecurityKeyGenerator : ISecurityKeyGenerator, IServiceSingleton
{
    private const string _alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public string GenerateKey()
    {
        return Nanoid.Nanoid.Generate(_alphabet, 12);
    }
}
