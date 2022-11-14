using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.Configuration;
using System.Reflection;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class GameConfigurationWindow : Window
    {
        private string language;
        private readonly Configuration gameConfiguration;
        private readonly KeyValueConfigurationElement musicOn;
        private readonly KeyValueConfigurationElement soundsOn;
        private PlayerProfile playerProfile;

        public GameConfigurationWindow()
        {
            language = "";
            gameConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            musicOn = gameConfiguration.AppSettings.Settings["MUSIC_ON"];
            soundsOn = gameConfiguration.AppSettings.Settings["SOUNDS_ON"];
            InitializeComponent();
            MusicSettings.IsChecked = musicOn.Value.Equals("true");
            SoundsSettings.IsChecked = soundsOn.Value.Equals("true");
            Utilities.PlayMusic();
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            gameConfiguration.Save();
            ConfigurationManager.RefreshSection("appSettings");
            GoToMainMenu();
        }

        private void GoToMainMenu()
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            GoToMainMenu();
        }

        private void EsMXFlagMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            language = "es-MX";
        }

        private void EnUSFlagMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            language = "";
        }

        private void FrFRFlagMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            language = "fr-FR";
        }

        private void PtBRFlagMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            language = "pt-BR";
        }

        private void MusicSettingsChecked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            musicOn.Value = "true";
            SoundsSettings.IsChecked = false;
            SoundsSettingsUnchecked(sender, e);
        }

        private void SoundsSettingsUnchecked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            soundsOn.Value = "false";
        }

        private void SoundsSettingsChecked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            soundsOn.Value = "true";
            MusicSettings.IsChecked = false;
            MusicSettingsUnchecked(sender, e);
        }

        private void MusicSettingsUnchecked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            musicOn.Value = "false";
        }
    }
}