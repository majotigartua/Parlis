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
        private static readonly SoundPlayer THROW_DICE = new SoundPlayer(Properties.Resources.ThrowDice);
        private static readonly SoundPlayer MOVE_COIN = new SoundPlayer(Properties.Resources.MoveCoin);
        private static readonly SoundPlayer EAT_COIN = new SoundPlayer(Properties.Resources.EatCoin);
        private static readonly SoundPlayer SHARE_SLOT = new SoundPlayer(Properties.Resources.ShareSlot);
        private static readonly SoundPlayer GO_TO_HOME_SLOT = new SoundPlayer(Properties.Resources.GoToHomeSlot);
        private static readonly SoundPlayer COLOR_PATH = new SoundPlayer(Properties.Resources.ColorPath);
        private static readonly SoundPlayer NEXT_TURN = new SoundPlayer(Properties.Resources.NextTurn);
        private static readonly SoundPlayer BUMMER = new SoundPlayer(Properties.Resources.Bummer);
        private static readonly SoundPlayer WINNER = new SoundPlayer(Properties.Resources.Winner);

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

        public static void PlayGameplaySound(int situation) 
        {
            if (ConfigurationManager.AppSettings["SOUNDS_ON"].Equals("true"))
            {
                switch (situation)
                {
                    case 0:
                        THROW_DICE.Play();
                        break;
                    case 1:
                        MOVE_COIN.Play();
                        break;
                    case 2:
                        EAT_COIN.Play();
                        break;
                    case 3:
                        SHARE_SLOT.Play();
                        break;
                    case 4:
                        GO_TO_HOME_SLOT.Play();
                        break;
                    case 5:
                        COLOR_PATH.Play();
                        break;
                    case 6:
                        NEXT_TURN.Play();
                        break;
                    case 7:
                        BUMMER.Play();
                        break;
                    case 8:
                        WINNER.Play();
                        break;
                }
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
            Console.WriteLine(Assembly.GetEntryAssembly().Location);
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