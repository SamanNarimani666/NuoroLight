using System.Security.Cryptography;
using System.Text;
namespace NuoroLight.Application.Security.PassWordHashing;
public class PasswordHelper : IPasswordHelper
{
    public string EncodePasswordSha512(string password) // Encrypt using SHA-512
    {
        byte[] originalBytes;
        byte[] encodedBytes;
        
        // Instantiate SHA512Managed, get bytes for original password and compute hash (encoded password)
        using (SHA512 sha512 = SHA512.Create())
        {
            originalBytes = Encoding.UTF8.GetBytes(password); // Use UTF8 encoding for better compatibility
            encodedBytes = sha512.ComputeHash(originalBytes); // Compute the SHA-512 hash
        }
        
        // Convert encoded bytes back to a 'readable' string
        return BitConverter.ToString(encodedBytes).Replace("-", "").ToLower(); // Optionally remove dashes and convert to lowercase
    }
}

