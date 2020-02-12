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

namespace FinalProject_NailSalon.Views
{
    /// <summary>
    /// Interaction logic for NewEmployeeWindow.xaml
    /// </summary>
    public partial class NewEmployeeWindow : Window
    {
        public NewEmployeeWindow()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            string firstName = tbFirstName.Text;
            string lastName = tbLastName.Text;
            string phoneNumber = tbPhoneNumber.Text;
            string email = tbEmail.Text;
            string address = tbAddress.Text;

            //Validate values
            if(string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageHelper.ShowWarningMessage("First Name, Last Name and Phone Number are mandatory!", "Information Error");
                return;
            }
            //If ok
            
            // Create new employee object
            Employee employee = new Employee(firstName,lastName,phoneNumber,email,address);
            string newID = UtilsHelper.GenerateID(employee);
            employee.ID = newID;

            // Store employee into DAO
            XMLHelpers.Instance().Employees.Add(employee);
            DialogResult = true;
            this.Close();
        }
    }
}
