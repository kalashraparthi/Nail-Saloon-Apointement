using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for NewAppointmentWindow.xaml
    /// </summary>
    public partial class NewAppointmentWindow : Window
    {
        private AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
        public NewAppointmentWindow()
        {
            InitializeComponent();
            DataContext = appointmentViewModel;
            UpdateRelatedAppointmentInfo();

            //Only for edit appointment
            SettupEditAppointment();
        }

        #region Edit Appointment mode
        private void SettupEditAppointment()
        {
            if(StateHelper.EditedAppointment == null)
            {
                return;
            }
            Appointment editedAppointment = StateHelper.EditedAppointment;
            appointmentViewModel.Customer = editedAppointment.Customer;
            dpCurrentDate.SelectedDate = editedAppointment.DateTime;
            ObservableCollection<DateTime> workingHours = appointmentViewModel.WorkingHours;

            int index = workingHours.IndexOf(workingHours.Where(day => TimeSpan.Compare(day.TimeOfDay, editedAppointment.DateTime.TimeOfDay) == 0).FirstOrDefault());
            cbTime.SelectedIndex = index;

            cbNailTechnician.SelectedIndex = appointmentViewModel.EmployeeList.
                FindIndex(employee => employee.ID == editedAppointment.Employee.ID);
            cbService.SelectedIndex = appointmentViewModel.Services.
                FindIndex(service => service.ID == editedAppointment.Service.ID);
            
        }
        #endregion

        #region Update Data for display Window
        /// <summary>
        /// When Window is loaded
        /// The program read data from DAO Layer and 
        /// push data to viewmodel for using binding in View
        /// </summary>
        private void UpdateRelatedAppointmentInfo()
        {
            NailSalonXMLDAO NailSalonDAO = XMLHelpers.Instance();
            //Load Employees 
            appointmentViewModel.EmployeeList = NailSalonDAO.Employees;
            //Load Services
            appointmentViewModel.Services = NailSalonDAO.Services;
            //Update current date
            appointmentViewModel.Today = DateTime.Now;
            if(StateHelper.IsEditedAppointmentMode == false && UtilsHelper.IsEndThedate())
            {
                dpCurrentDate.SelectedDate = DateTime.Now.AddDays(1);
            }
        }
        #endregion

        #region Create new Appointment
        /// <summary>
        /// Get Data from window
        /// Create new customer and appointment
        /// Save into DAO layer
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            string firstName = tbFirstName.Text;
            string lastName = tbLastName.Text;
            string phoneNumber = tbPhoneNumber.Text;
            string email = tbEmail.Text;
            string address = tbAddress.Text;

            Customer customer = new Customer(firstName,lastName,phoneNumber,email,address);
            customer.ID = UtilsHelper.GenerateID(customer);
            //Add new customer into DAO
            XMLHelpers.Instance().Customers.Add(customer);

            if (string.IsNullOrWhiteSpace(firstName) 
                || string.IsNullOrWhiteSpace(lastName) 
                || string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageHelper.ShowWarningMessage("First Name, Last Name and Phone Number are mandatory!", "Error");
                return;
            }

            DateTime date = dpCurrentDate.SelectedDate.Value.Date;
            if (cbTime.SelectedItem == null || cbService.SelectedItem == null || cbNailTechnician.SelectedItem == null)
            {
                MessageHelper.ShowWarningMessage("Date , Time, Service and Employee are mandatory!", "Error");
                return;
            }

            TimeSpan selectedTime = TimeSpan.FromHours(((DateTime)cbTime.SelectedItem).Hour);
            date = date.Date + selectedTime;
            Service service = (Service)cbService.SelectedItem;
            Employee employee = (Employee)cbNailTechnician.SelectedItem;


            Appointment newAppointment = new  Appointment(date,customer,service,employee);
            newAppointment.ID = UtilsHelper.GenerateID(newAppointment);

            if (StateHelper.IsEditedAppointmentMode)
            {
                newAppointment.ID = StateHelper.EditedAppointment.ID;
            }

            //Check is valid appointment
            if (!UtilsHelper.IsEmployeeAvailableOn(date, employee, newAppointment))
            {
                MessageHelper.ShowWarningMessage(String.Format("Technician {0} is not available on that time\nPlease choose another time!", employee.FullName), "Appointment");
                return;
            }

            //Update Appointment into DAO when open this windom for editing
            if (StateHelper.IsEditedAppointmentMode)
            {
                //Check is valid appointment
               
                int index = XMLHelpers.Instance().Appointments.FindIndex(appointment => appointment.ID == StateHelper.EditedAppointment.ID);
                XMLHelpers.Instance().Appointments[index] = newAppointment;
            } else
            {
                
                // Add new appointment into DAO
                XMLHelpers.Instance().Appointments.Add(newAppointment);

                
            }

            

            DialogResult = true;

            //Reset Selected Appointment for updating
            ResetState();
            this.Close();
        }

        #endregion

      

        #region Select Date for appointment
        /// <summary>
        /// When the date is select
        /// The working hours time should be update
        /// because working hours are different for different day
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpCurrentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            appointmentViewModel.Today = dpCurrentDate.SelectedDate.Value.Date;
            appointmentViewModel.RefreshWorkingHours();

        }
        #endregion

        #region Customize Date Selection
        /// <summary>
        /// Because the salon is closed on Monday
        /// The program prevent select that day
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpCurrentDate_CalendarOpened(object sender, RoutedEventArgs e)
        {
            dpCurrentDate.DisplayDateStart = DateTime.Now - TimeSpan.FromDays(50);
            dpCurrentDate.DisplayDateEnd = DateTime.Now + TimeSpan.FromDays(365);//A year

            var minDate = dpCurrentDate.DisplayDateStart ?? DateTime.MinValue;
            var maxDate = dpCurrentDate.DisplayDateEnd ?? DateTime.MaxValue;

            for(var date = minDate; date <= maxDate && DateTime.MaxValue > date; date = date.AddDays(1))
            {
                if((date.DayOfWeek == DayOfWeek.Monday && DateTime.Today.Date != date.Date) || date < DateTime.Today)
                {
                    try
                    {
                        dpCurrentDate.BlackoutDates.Add(new CalendarDateRange(date));

                    } catch
                    {

                    }
                }
            }
        }
        #endregion

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Reset Selected Appointment for updating
            ResetState();
            this.Close();
        }

        private void ResetState()
        {
            StateHelper.EditedAppointment = null;
            StateHelper.IsEditedAppointmentMode = false;
        }

     
    }
}
