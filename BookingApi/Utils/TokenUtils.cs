using System.IdentityModel.Tokens.Jwt;

namespace BookingApi.Utils;

public class TokenUtils {
    public string GetUserIdFromToken(string bearer) {
        var jwt = new JwtSecurityTokenHandler();
        var decoded = jwt.ReadJwtToken(bearer);
        return decoded.Claims.First(claim => claim.Type == "id").Value;
    }
}