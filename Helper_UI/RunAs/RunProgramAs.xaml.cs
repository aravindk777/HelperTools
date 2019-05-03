using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Helper_UI.RunAs
{
    /// <summary>
    /// Interaction logic for RunProgramAs.xaml
    /// </summary>
    public partial class RunProgramAs : Window
    {
        public string ParameterVars { get; set; }

        OpenFileDialog openFileDialog;
        public RunProgramAs()
        {
            InitializeComponent();
            lblDomainName.Content = Environment.UserDomainName + " \\";
            ProgramNameWithParams.Text = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose any executable";
            //openFileDialog.Filter = "Executable (*.exe)|*.exe| DotnetCore apps (*.dll)|*.dll";
            ProgramNameWithParams.Text = string.Empty;
            if (rbDotnetCore.IsChecked == true)
            {
                //ProgramNameWithParams.Text = "dotnet ";
                openFileDialog.Filter = "DotnetCore apps (*.dll)|*.dll";
            }
            else
            {
                openFileDialog.Filter = "Executable (*.exe)|*.exe";
            }

            if (openFileDialog.ShowDialog(this) == true)
            {
                ProgramNameWithParams.Text += $"{openFileDialog.FileName}";
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(TextBox))
            {
                var currTextBox = (TextBox)sender;
                if (currTextBox.Text == "(Username)")
                    currTextBox.Text = string.Empty;
            }
        }

        private void TxtParameters_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ProgramNameWithParams.Text += $" {e.}";
            ParameterVars = txtParameters.Text;
        }

        private void RunAsChoices_Checked(object sender, RoutedEventArgs e)
        {
            //var currChoiceBtn = (RadioButton)sender;
            //if (currChoiceBtn.Name == "rbDotnetCore")
            //    ProgramNameWithParams.Text = "dotnet " + ProgramNameWithParams.Text;
            //else
            //    ProgramNameWithParams.Text = ProgramNameWithParams.Text.Replace("dotnet", string.Empty).Trim();
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            #region Impersonation Code - not used though
            //using (var safeTokenHandle = ImpersonatorModule.Impersonate(Environment.UserDomainName, txtUserName.Text.Trim(), txtPassword.Password.Trim()))
            //{
            //    Console.WriteLine("Value of Windows NT token: " + safeTokenHandle);

            //    // Check the identity.
            //    Console.WriteLine("Before impersonation: " + WindowsIdentity.GetCurrent().Name);
            //    // Use the token handle returned by LogonUser.
            //    using (WindowsIdentity newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle()))
            //    {
            //        using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
            //        {
            //            // Check the identity.
            //            Console.WriteLine("After impersonation: " + WindowsIdentity.GetCurrent().Name);

            //            // run the program
            //            System.Security.SecureString pwdStr = new System.Security.SecureString();
            //            txtPassword.Password.Trim().ToList().ForEach(c => pwdStr.AppendChar(c));
            //            //System.Diagnostics.Process.Start(ProgramNameWithParams.Text, txtUserName.Text.Trim(), pwdStr, Environment.UserDomainName);

            //            var procInfo = new System.Diagnostics.ProcessStartInfo
            //            {
            //                FileName = ProgramNameWithParams.Text,
            //                Arguments = ParameterVars,
            //                Domain = Environment.UserDomainName,
            //                UserName = txtUserName.Text.Trim(),
            //                Password = pwdStr,
            //                UseShellExecute = false
            //            };                        

            //            var p = System.Diagnostics.Process.Start(procInfo);
            //            p.WaitForExit();
            //        }
            //        //safeTokenHandle.Close();
            //        //safeTokenHandle.Dispose();
            //    }
            //    // Releasing the context object stops the impersonation
            //    // Check the identity.
            //    Console.WriteLine("After closing the context: " + WindowsIdentity.GetCurrent().Name);
            //}
            #endregion

            System.Security.SecureString pwdStr = new System.Security.SecureString();
            txtPassword.Password.Trim().ToList().ForEach(c => pwdStr.AppendChar(c));

            var procInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = rbDotnetCore.IsChecked == true ? @"C:\Program Files\dotnet\dotnet.exe" : ProgramNameWithParams.Text.Trim(),
                Arguments = rbDotnetCore.IsChecked == true ? string.Format("{0} {1}", ProgramNameWithParams.Text, ParameterVars) : ParameterVars,
                Domain = Environment.UserDomainName,
                UserName = txtUserName.Text.Trim(),
                Password = pwdStr,
                UseShellExecute = false
            };
            try
            {
                using (var p = System.Diagnostics.Process.Start(procInfo))
                {
                    p.WaitForExit();
                }
            }
            catch(Exception ex)
            {
                // handle exception
                Console.Error.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
