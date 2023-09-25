using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EncryptText();
        }

        void EncryptText()
        {
            var currentPwd = txtPassword.Password;
            var encodedString = Convert.ToBase64String(Encoding.Unicode.GetBytes(currentPwd.ToCharArray()));
            var reversedString = Encoding.Unicode.GetString(Convert.FromBase64String(encodedString));
            txtPassword.ToolTip = currentPwd;
            txtEncrypted.Text = encodedString;
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
