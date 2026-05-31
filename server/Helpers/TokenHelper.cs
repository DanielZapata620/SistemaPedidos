using System.Security.Cryptography;

namespace PedidoApi.Helpers;

public class TokenHelper
{
    private readonly Dictionary<string, int> _tokens = new();

    public string CreateToken(int userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        _tokens[token] = userId;
        return token;
    }

    public int? GetUserId(string token)
    {
        return _tokens.TryGetValue(token, out var userId) ? userId : null;
    }
}
