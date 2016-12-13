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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetAllData();
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
                string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                string CmdString = String.Empty;
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    try
                    {
                        con.Open();
                        CmdString = "INSERT INTO TbCus (fn, ln, dob, age) VALUES ('" + txtfn.Text + "', '" + txtln.Text + "', '" + txtdob.Text + "'," + txtage.Text + ")";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("This record has been saved successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        GetAllData();
                        con.Close();
                    }
                }
        }

        public void GetAllData()
        {
            string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                try
                {
                    CmdString = "SELECT * FROM TbCus";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("Gov");
                    da.Fill(dt);
                    dgridGov.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
