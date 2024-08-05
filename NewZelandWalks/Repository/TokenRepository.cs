using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        //In oreder to get any data from appsettings.json file we have to inject the interface Iconfiguration.json in this class
        //Here we need the Jwt object (to ket the key) thus injecting the the dependency

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //creating claims for the JWT token
            var claims = new List<Claim>();

            //adding email of user as a claim (Payload secton)
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            //Iterating over roles array to add all the roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //here we are creating a secret with the help of the Jwt object which we created in appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            /*SigningCredentials: This class represents the cryptographic key and the security algorithm used to sign the JWT(necessary to protect against tempering of JWT).
              key: This is the cryptographic key that will be used to sign the JWT.
                   It is typically an instance of SymmetricSecurityKey, which is created using a secret key stored in your application settings.
              SecurityAlgorithms.HmacSha256: This specifies the security algorithm to be used for signing the JWT. HmacSha256 stands for HMAC-SHA256 (Hash-based Message Authentication Code using          SHA-256), which is a secure hash algorithm.*/

            //CREATING THE TOKEN
            /*This code initializes a JwtSecurityToken object with the specified issuer, audience, claims, expiration time, and signing credentials.
             At this stage, the token variable is an in-memory representation of the JWT. It has not yet been serialized into a string.*/
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            /*This line takes the JwtSecurityToken object you created and serializes it into a string that the client can use in subsequent requests for authentication.*/
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}


/*Signing the Token
    Signing the token involves the following steps:

    1)Create the Header and Payload: These are JSON objects containing relevant information.
    2)Encode the Header and Payload: Convert these JSON objects into Base64Url encoded strings.
    3)Generate the Signature: Use a cryptographic algorithm (like HMAC-SHA256) and a secret key to create a unique signature based on the encoded header, encoded payload, and the secret key.
    4)Combine the Parts: Concatenate the encoded header, encoded payload, and the signature with dots ('.') to form the final JWT.*/
