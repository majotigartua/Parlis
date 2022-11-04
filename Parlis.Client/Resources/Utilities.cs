using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Media;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Parlis.Client.Resources
{
    public class Utilities
    {
        private static readonly SoundPlayer MUSIC = new SoundPlayer(Properties.Resources.Music);
        private static readonly SoundPlayer BUTTON_CLICK = new SoundPlayer(Properties.Resources.ButtonClick);

        public static string ComputeSHA256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedPassword = new StringBuilder();
                for (int bit = 0; bit < (bytes.Length); bit++)
                {
                    hashedPassword.Append(bytes[bit].ToString("x2"));
                }
                return hashedPassword.ToString();
            }
        }

        public static int GenerateRandomCode()
        {
            var random = new Random();
            return random.Next(100000, 999999);
        }

        public static void PlayButtonClickSound()
        {
            if (ConfigurationManager.AppSettings["SOUNDS_ON"].Equals("true"))
            {
                BUTTON_CLICK.Play();
            }
        }

        public static void PlayMusic()
        {
            MUSIC.Stop();
            if (ConfigurationManager.AppSettings["MUSIC_ON"].Equals("true"))
            {
                MUSIC.Play();
            }
        }

        public static void SaveProfilePicture(string username, Image profilePicture)
        {
            var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "../../ProfilePictures/" + username + ".jpg";
            using (var fileStream = new FileStream(profilePicturePath, FileMode.Create))
            {
                var jpegBitmapEncoder = new JpegBitmapEncoder();
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) profilePicture.Source));
                jpegBitmapEncoder.Save(fileStream);
            }
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