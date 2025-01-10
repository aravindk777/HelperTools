using System;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Media;

namespace Helper_UI.Authenticators.AD
{
    /// <summary>
    /// Interaction logic for WinADAuth.xaml
    /// </summary>
    public partial class WinADAuth : Window
    {
        private string _path {  get; set; }
        public WinADAuth()
        {
            InitializeComponent();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPassword.ToolTip = txtPassword.Password;
        }

        private void btnAuthenticate_Click(object sender, RoutedEventArgs e)
        {
            var result = IsAuthenticated(txtUsername.Text, txtPassword.Password);
        }

        bool IsAuthenticated(string username, string pwd)
        {
            if (!username.Contains("\\"))
            {
                SetBackgroundForLabel(-1, "Please make sure to provide the domain");
                return false;
            }
            try
            {
                SetBackgroundForLabel(0, "Working...");
                DirectoryEntry entry = new DirectoryEntry(_path, username, pwd);
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username.Substring(username.IndexOf("\\")+1) + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    SetBackgroundForLabel(-1, $"Username: {username} x Password doesnt match to work.");
                    return false;
                }

                _path = result.Path;
                var _filterAttribute = result.Properties["cn"][0].ToString();

                SetBackgroundForLabel(1, "Success! Username and Password are valid.");

            }
            catch (Exception ex)
            {
                SetBackgroundForLabel(-1, $"Fail! Either Username or Password is invalid.{Environment.NewLine}{ex.Message}");
                return false;
            }

            return true;
        }

        void SetBackgroundForLabel(int flag, string message)
        {
            switch (flag)
            {
                case 0:
                default:
                    status.Content = message;
                    status.Background = Brushes.LightBlue;
                    break;

                case 1:
                    status.Content = message;
                    status.Background = Brushes.Green;
                    break;

                case -1:
                    status.Content = message;
                    status.Background = Brushes.Orange;
                    break;
            }
            status.FontSize = 20;
            status.HorizontalContentAlignment = HorizontalAlignment.Center;
        }
    }
}
