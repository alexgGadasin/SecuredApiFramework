using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Api
{
    public static class ApiHelper
    {
        public static DescriptionAttribute GetEnumDescriptionAttribute<T>(this T value) where T : struct
        {
            // The type of the enum, it will be reused.
            Type type = typeof(T);

            // If T is not an enum, get out.
            if (!type.IsEnum)
                throw new InvalidOperationException(
                    "The type parameter T must be an enum type.");

            // If the value isn't defined throw an exception.
            if (!Enum.IsDefined(type, value))
                throw new InvalidEnumArgumentException(
                    "value", Convert.ToInt32(value), type);

            // Get the static field for the value.
            FieldInfo fi = type.GetField(value.ToString(),
                BindingFlags.Static | BindingFlags.Public);

            // Get the description attribute, if there is one.
            return fi.GetCustomAttributes(typeof(DescriptionAttribute), true).
                Cast<DescriptionAttribute>().SingleOrDefault();
        }
        public static object GetPropValue(this object obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
        public static T GetPropValue<T>(this object obj, string name)
        {
            object retval = GetPropValue(obj, name);
            if (retval == null) { return default; }

            return (T)retval;
        }
        public static string Encrypt(string plainText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations
            );

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor
            (
                keyBytes,
                initVectorBytes
            );

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                encryptor,
                CryptoStreamMode.Write
            );

            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }
        public static string Decrypt(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            // Convert strings defining encryption key characteristics into byte arrays. 
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified passphrase and salt value. 
            // The password will be created using the specified hash algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations
            );

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                Mode = CipherMode.CBC
            };

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor
            (
                keyBytes,
                initVectorBytes
            );

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                decryptor,
                CryptoStreamMode.Read
            );
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read
            (
                plainTextBytes,
                0,
                plainTextBytes.Length
            );

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString
            (
                plainTextBytes,
                0,
                decryptedByteCount
            );

            // Return decrypted string.   
            return plainText;
        }
        public static string HashWithSalt(string password, string salt, HashAlgorithm hashAlgo)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return Convert.ToBase64String(digestBytes);
        }
        public static string GenerateRandomCryptographicKey(int keyLength)
        {
            return Convert.ToBase64String(GenerateRandomCryptographicBytes(keyLength));
        }
        private static byte[] GenerateRandomCryptographicBytes(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }
        public static string GeneratedID(int length, string value)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return value.Replace(" ", "-") + "-" + new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string JSONSerialize(object value, bool ignoreNullAndEmpty = false)
        {
            if(!ignoreNullAndEmpty)
                return JsonConvert.SerializeObject(value);
            else
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                };

                return JsonConvert.SerializeObject(value, settings);
            }
        }
        public static T JSONDeserialize<T>(string value) where T : class
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static string EncodeToBase64String(string basicCredentials)
        {
            var encodedCredentials = Encoding.UTF8.GetBytes(basicCredentials);
            var base64Credentials = Convert.ToBase64String(encodedCredentials);

            return base64Credentials;
        }
        public static string DecodeFromBase64String(string base64Credentials)
        {
            var encodedCredentials = Convert.FromBase64String(base64Credentials);
            var basicCredentials = Encoding.UTF8.GetString(encodedCredentials);

            return basicCredentials;
        }
        public static string GetEnumDescription(ApiMessageType type, object args = null)
        {
            return string.Format(type.GetEnumDescriptionAttribute().Description, args);
        }
        public static string GetEnumDescription(ApiValidationType type, object args = null)
        {
            return string.Format(type.GetEnumDescriptionAttribute().Description, args);
        }
        private static List<ApiClaim> GetTokenClaims(HttpRequest request)
        {
            var claims = new List<ApiClaim>();
            
            try
            {
                var authorization = request.Headers["Authorization"].ToString().Split(' ');

                if (authorization.Count() >= 2)
                {
                    var token = authorization[1];
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jsonToken = tokenHandler.ReadToken(token);
                    var securityToken = jsonToken as JwtSecurityToken;

                    foreach (Claim claim in securityToken.Claims)
                    {
                        claims.Add(new ApiClaim() { Type = claim.Type, Value = claim.Value });
                    }
                }
            } catch { }

            return claims;
        }
        public static List<ApiClaim> GetClaims(this HttpRequest request)
        {
            return GetTokenClaims(request);
        }
        public static string GetClaim(this HttpRequest request, string Type)
        {
            var claims = GetTokenClaims(request);

            if (!claims.Any(c => c.Type == Type))
                return "";
            else
                return claims.Where(c => c.Type == Type).First().Value;
        }
    }
}
