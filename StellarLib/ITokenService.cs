namespace StellarLib;

public interface ITokenService
{
    Task<string> GetAccessToken();
}