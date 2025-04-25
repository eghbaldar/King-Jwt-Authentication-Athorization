using System.Security.Cryptography;
using System.Text;

namespace KingJwtAuth.Services.Token
{
    public static class EncryptionHelper
    {
        public static string EncryptGuidWithPublicKey(Guid guid, string publicKey)
        {
            // Convert GUID to bytes
            byte[] guidBytes = Encoding.UTF8.GetBytes(guid.ToString());

            // Derive AES key from publicKey string
            byte[] key = DeriveKeyFromPublicKey(publicKey);
            byte[] iv = new byte[16]; // Fixed zero IV (make sure both ends know this or send IV with ciphertext)

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var encryptor = aes.CreateEncryptor();
            byte[] encryptedBytes = encryptor.TransformFinalBlock(guidBytes, 0, guidBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }
        public static string DecryptEncryptedGuid(string base64Encrypted, string publicKey)
        {
            byte[] encryptedBytes = Convert.FromBase64String(base64Encrypted);

            byte[] key = DeriveKeyFromPublicKey(publicKey);
            byte[] iv = new byte[16]; // Must be the same IV as encryption

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            string decryptedGuidString = Encoding.UTF8.GetString(decryptedBytes);
            return decryptedGuidString;
        }
        private static byte[] DeriveKeyFromPublicKey(string publicKey)
        {
            // You should store and reuse the salt for consistent results!
            var salt = Encoding.UTF8.GetBytes("FixedSaltValue1234"); // Keep consistent across calls
            using var pbkdf2 = new Rfc2898DeriveBytes(publicKey, salt, 10000);
            return pbkdf2.GetBytes(32); // AES-256
        }
    }

}
