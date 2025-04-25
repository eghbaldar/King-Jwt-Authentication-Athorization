using KingJwtAuth.Entities;
using KingJwtAuth.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace KingJwtAuth.Services.Token
{
    public class TokenAccessService
    {
        public string GenerateToken(UserTokenDto payload, string secretKey)
        {
            string header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
            string encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(header));

            string payloadJson = JsonConvert.SerializeObject(payload);
            string encodedPayload = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

            string signature = ComputeHMACSHA256($"{encodedHeader}.{encodedPayload}", secretKey);
            return $"{encodedHeader}.{encodedPayload}.{signature}";
        }
        private string ComputeHMACSHA256(string text, string key)
        {
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
            return Base64UrlEncode(hash);
        }
        public string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
        public byte[] Base64UrlDecode(string input)
        {
            string padded = input
                .Replace('-', '+')
                .Replace('_', '/');

            switch (padded.Length % 4)
            {
                case 2: padded += "=="; break;
                case 3: padded += "="; break;
            }

            return Convert.FromBase64String(padded);
        }
        public bool ValidateKingToken(string token, string secretKey, out UserTokenDto userPayload)
        {
            userPayload = null;

            var parts = token.Split('.');
            if (parts.Length != 3) return false;

            var header = parts[0];
            var payload = parts[1];
            var signature = parts[2];

            string expectedSignature = ComputeHMACSHA256($"{header}.{payload}", secretKey);
            if (signature != expectedSignature) return false;

            var jsonPayload = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            userPayload = JsonConvert.DeserializeObject<UserTokenDto>(jsonPayload);

            var _userTokenExp = userPayload.Exp.ToUniversalTime();
            var _now = DateTimeOffset.UtcNow;
            if (_userTokenExp < _now) return false;

            return true;
        }
    }
}
