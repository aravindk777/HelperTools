using System;
using System.Text;
using System.Windows;

namespace Helper_UI.Encryptor
{
    /// <summary>
    /// Interaction logic for Encryptors.xaml
    /// </summary>
    public partial class Encryptors : Window
    {
        public Encryptors()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            EncryptText();
        }

        //private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    EncryptText();
        //}

        void EncryptText()
        {
            var currentPwd = txtPassword.Text;
            var encodedString = Convert.ToBase64String(Encoding.Unicode.GetBytes(currentPwd.ToCharArray()));
            var reversedString = Encoding.Unicode.GetString(Convert.FromBase64String(encodedString));
            txtPassword.ToolTip = currentPwd;
            txtEncrypted.Text = encodedString;
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var reversedString = Encoding.Unicode.GetString(Convert.FromBase64String(txtEncrypted.Text));
                txtPassword.Text = reversedString;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error while decoding the string - {ex}", ex.Message);
            }
        }

        private void txtPassword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
