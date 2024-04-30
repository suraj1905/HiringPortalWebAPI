using HiringPortalWebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace HiringPortalWebAPI.Utilities
{
    public class Utils
    {
        private readonly IConfiguration _configuration;
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public Utils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int id, string userName, string role)
        {
            var claims = new[] {
                        new Claim("Id", id.ToString()),
                        new Claim("UserName", userName),
                        new Claim("Role", role)
                    };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn);



            string AccessToken = new JwtSecurityTokenHandler().WriteToken(token);


            return AccessToken;
        }


        public string GeneratePassword(int length = 8)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$";
            Random random = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(0, chars.Length)]);
            }

            return password.ToString();
        }

        public bool SendEmail(Credential credential, string password)
        {
            try
            {
                string senderEmail = _configuration["Mail:EmailId"]!;
                string senderPassword = _configuration["Mail:Password"]!;
                string recipientEmail = credential.UserId;
                // Create a new MailMessage instance
                MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail);

                // Set the subject and body of the email
                mailMessage.Subject = "Credentials for Hiring Portal";
                mailMessage.Body = $"Hi,\n Please find the below login credentials for Hiring Portal.\n" +
                    $"UserId = {credential.UserId}\n"+
                    $"Password = {password}\n";

                // Create a SmtpClient instance
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                // Specify credentials (sender's email address and password)
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                // Enable SSL/TLS encryption
                smtpClient.EnableSsl = true;

                // Send the email
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            );

            // Combine the salt and hash
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64 for storage
            string hashedPassword = Convert.ToBase64String(hashBytes);
            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Extract the salt and hash from the stored password
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            byte[] expectedHash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, expectedHash, 0, HashSize);

            // Compute the hash of the entered password using the extracted salt
            byte[] actualHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            );

            // Compare the computed hash with the stored hash
            for (int i = 0; i < HashSize; i++)
            {
                if (actualHash[i] != expectedHash[i])
                    return false;
            }
            return true;
        }

        public string GenerateFileName(string fileName, string candidateName){
            try
            {
                string strFileName = string.Empty;
                string[] strName = fileName.Split('.');
                strFileName = candidateName + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssff")+"."+
                strName[strName.Length-1];
                return strFileName;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}

