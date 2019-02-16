using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;

namespace RunMyQuery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection executeQueryConn = GetConnection();
            if (executeQueryConn.State == System.Data.ConnectionState.Open)
            {
                ResultSetTextBlock.Text = "connection opened successfully. " + Environment.NewLine;
                try
                {
                    SqlCommand executeCmd = new SqlCommand(QueryText.Text, executeQueryConn);
                    int resultSetsAffected = executeCmd.ExecuteNonQuery();

                    ResultSetTextBlock.Text = string.Format("{0} - record(s) affected!{1}", resultSetsAffected, Environment.NewLine);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ResultSetTextBlock.Text = "Error occurred in opening up a connection. Check your connection string!";
            }
        }

        private SqlConnection GetConnection()
        {
            string connectionString = GetConnectionString();

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return connection;
        }

        /// <summary>
        /// GetConnectionString
        /// </summary>
        /// <returns>connectionString</returns>
        private string GetConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProdConnect"].ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("No config found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return connectionString;
        }
    }
}
