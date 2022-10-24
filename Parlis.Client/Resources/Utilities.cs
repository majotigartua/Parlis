using Microsoft.Win32;
using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Parlis.Client.Resources
{
    public class Utilities
    {
        public static string ComputeSHA256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedPassword = new StringBuilder();
                for (int i = 0; i < (bytes.Length); i++)
                {
                    hashedPassword.Append(bytes[i].ToString("x2"));
                }
                return hashedPassword.ToString();
            }
        }

        public static int GenerateRandomCode()
        {
            var random = new Random();
            return random.Next(100000, 999999);
        }

        public static string SelectProfilePicture()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = Properties.Resources.PROFILE_PICTURE_WINDOW_TITLE,
                Filter = "Joint Photographic Experts Group (JPEG)|*.jpg"
            };
            openFileDialog.ShowDialog();
            return openFileDialog.FileName;
        }

        public static bool ValidateEmailAddressFormat(string emailAddress)
        {
            try
            {
                var mailAddress = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool ValidatePasswordFormat(string password)
        {
            var hasUpperLetter = new Regex(@"[A-Z]+");
            var hasNumber = new Regex(@"[0-9]+");
            var hasMiniumEightDigits = new Regex(@".{8,}");
            return (hasNumber.IsMatch(password) &&
                hasUpperLetter.IsMatch(password) &&
                hasMiniumEightDigits.IsMatch(password));
        }
    }
}