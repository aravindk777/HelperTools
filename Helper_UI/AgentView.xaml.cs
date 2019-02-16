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
using System.Windows.Shapes;
using DataConnections;
using System.Data.Entity;

namespace Helper_UI
{
    /// <summary>
    /// Interaction logic for AgentView.xaml
    /// </summary>
    public partial class AgentView : Window
    {
        DataConnect dbConnect { get; set; }
        List<AgentProfileDataSet> MyDataSet { get; set; }
        AgentProfileDataSet oneUser { get; set; }

        public AgentView()
        {
            InitializeComponent();
            MyDataSet = new List<AgentProfileDataSet>();
            oneUser = new AgentProfileDataSet();
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            dbConnect = new DataConnect(ddEnvironment.Text);
            try
            {
                oneUser = dbConnect.GetAgentProfileData(txtUserId.Text);

                if (MyDataSet != null)
                {
                    //MessageBox.Show("We've got the data for UserId (" + oneUser.UserId + ") and Name is (" + oneUser.FirstName + ")");
                    MyDataSet.Add(oneUser);
                }
                else
                {
                    AgentProfileDataSet emptyData = new AgentProfileDataSet();
                    emptyData.FirstName= "-No data-";
                    emptyData.UserId = "-No data-";

                    MyDataSet.Add(emptyData);
                }

                dgAgentProfile.ItemsSource = MyDataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oops!" + Environment.NewLine + "Error = " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                
            }
        }
    }
}
