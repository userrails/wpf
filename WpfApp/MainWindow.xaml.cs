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
            btnUpdate.Visibility = System.Windows.Visibility.Hidden;
            btnDelete.Visibility = System.Windows.Visibility.Hidden;
        }

        // Save feature
        private void Button_Save(object sender, RoutedEventArgs e)
        {
                string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                string CmdString = String.Empty;
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    if (RequiredFieldIsBlank())
                    {
                        MessageBox.Show("All Fields are required!");
                    }
                    else
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
                            ClearAll();
                            con.Close();
                        }
                    }
                }
        }
        // Save feature


        // Display all the data to ListView
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
                    lvCus.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        // Display all the data to ListView


        // Button Cancel
        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }
        // Button Cancel


        //Clear all Text
        public void ClearAll()
        {
            txtCusID.Text = "";
            txtfn.Text = "";
            txtln.Text = "";
            txtdob.Text = "";
            txtage.Text = "";

            btnUpdate.Visibility = System.Windows.Visibility.Hidden;
            btnDelete.Visibility = System.Windows.Visibility.Hidden;
            btnSave.Visibility = System.Windows.Visibility.Visible;
        }
        //Clear all Text



        // Button Update
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string cmdString = String.Empty;
            using (SqlConnection con = new SqlConnection(conString))
            {
                if (string.IsNullOrEmpty(txtCusID.Text))
                {
                    MessageBox.Show("Select record from list first!");
                }
                else if (RequiredFieldIsBlank())
                {
                  MessageBox.Show("All Fields are required!");  
                }
                else
                {
                    try
                    {
                        con.Open();
                        cmdString = "UPDATE TbCus SET fn='" + txtfn.Text + "', ln='" + txtln.Text + "', dob='" + txtdob.Text + "', age=" + txtage.Text + "where CusID=" + txtCusID.Text + ";";
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("This record has been updated successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        GetAllData();
                        ClearAll();
                        con.Close();
                    }
                }
            }
        }
        // Button Update


        // Button Exit
        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        // Button Exit


        // ListViewItem_DoubleClick
        private void ListViewItem_DoubleClick(object sender, RoutedEventArgs e)
        {
           System.Data.DataRowView cusObj = lvCus.SelectedItem as System.Data.DataRowView;
           
           if (cusObj != null)
           {
               var myobj = cusObj.Row.ItemArray;
               txtCusID.Text = myobj[0].ToString();
               txtfn.Text = myobj[1].ToString();
               txtln.Text = myobj[2].ToString();
               txtdob.Text = myobj[3].ToString();
               txtage.Text = myobj[4].ToString();

               btnUpdate.Visibility = System.Windows.Visibility.Visible;
               btnDelete.Visibility = System.Windows.Visibility.Visible;
               btnSave.Visibility = System.Windows.Visibility.Hidden;
           }
        }
        // ListViewItem_DoubleClick


        // Check if all required fields are blank
        private bool RequiredFieldIsBlank()
        {
            if (txtfn.Text=="" && txtfn.Text=="" && txtln.Text=="" && txtdob.Text=="" && txtage.Text=="")
            {
                return true;
            }
            return false;
        }
        // Check if all required fields are blank


        // Delete Feature
        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string cmdString = String.Empty;

            using (SqlConnection con = new SqlConnection(conString))
            {

                if (string.IsNullOrEmpty(txtCusID.Text) && RequiredFieldIsBlank())
                {
                    MessageBox.Show("Select record from list to Delete!!");
                }
                else
                {
                    try
                    {
                        con.Open();
                        cmdString = "DELETE FROM TbCus where CusID=" + txtCusID.Text + ";";
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("This record has been deleted successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        GetAllData();
                        ClearAll();
                        con.Close();
                    }
                }
            }
        }
        // Delete Feature



        // Search on listview when Search button is clicked
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                string searchText = txtSearch.Text.ToLower();
                foreach (ListViewItem item in lvCus.Items)
                {
                    Console.WriteLine(item);  
                }
            }
        }
        // Search on listview when Search button is clicked

        private void ButtonEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee emp = new Employee();
            emp.ShowDialog();
        }
    }
}
