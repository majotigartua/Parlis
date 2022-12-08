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
        private static readonly SoundPlayer music = new SoundPlayer(Properties.Resources.Music);
        private static readonly SoundPlayer buttonClick = new SoundPlayer(Properties.Resources.ButtonClick);
        private static readonly SoundPlayer throwDice = new SoundPlayer(Properties.Resources.ThrowDice);
        private static readonly SoundPlayer moveCoin = new SoundPlayer(Properties.Resources.MoveCoin);
        private static readonly SoundPlayer eatCoin = new SoundPlayer(Properties.Resources.EatCoin);
        private static readonly SoundPlayer shareSlot = new SoundPlayer(Properties.Resources.ShareSlot);
        private static readonly SoundPlayer goToHomeSlot = new SoundPlayer(Properties.Resources.GoToHomeSlot);
        private static readonly SoundPlayer colorPath = new SoundPlayer(Properties.Resources.ColorPath);
        private static readonly SoundPlayer nextTurn = new SoundPlayer(Properties.Resources.NextTurn);
        private static readonly SoundPlayer bummer = new SoundPlayer(Properties.Resources.Bummer);
        private static readonly SoundPlayer winner = new SoundPlayer(Properties.Resources.Winner);

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
                buttonClick.Play();
            }
        }

        public static void PlayGameSound(int code)
        {
            if (ConfigurationManager.AppSettings["SOUNDS_ON"].Equals("true"))
            {
                switch (code)
                {
                    case Constants.THROW_DICE_CODE:
                        throwDice.Play();
                        break;
                    case Constants.MOVE_COIN_CODE:
                        moveCoin.Play();
                        break;
                    case Constants.EAT_COIN_CODE:
                        eatCoin.Play();
                        break;
                    case Constants.SHARE_SLOT_CODE:
                        shareSlot.Play();
                        break;
                    case Constants.GO_TO_HOME_SLOT_CODE:
                        goToHomeSlot.Play();
                        break;
                    case Constants.COLOR_PATH_CODE:
                        colorPath.Play();
                        break;
                    case Constants.NEXT_TURN_CODE:
                        nextTurn.Play();
                        break;
                    case Constants.BUMMER_CODE:
                        bummer.Play();
                        break;
                    case Constants.WINNER_CODE:
                        winner.Play();
                        break;
                }
            }
        }

        public static void PlayMusic()
        {
            music.Stop();
            if (ConfigurationManager.AppSettings["MUSIC_ON"].Equals("true"))
            {
                music.Play();
            }
        }

        public static void SaveProfilePicture(string username, Image profilePicture)
        {
            var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "../../ProfilePictures/" + username + ".jpg";
            using (var fileStream = new FileStream(profilePicturePath, FileMode.Create))
            {
                var jpegBitmapEncoder = new JpegBitmapEncoder();
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)profilePicture.Source));
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

        public static bool ValidateTextLengthOverflowed(int lenght, string text)
        {
            if (text.Length > lenght)
            {
                return true;
            }
            return false;
        }
    }
}