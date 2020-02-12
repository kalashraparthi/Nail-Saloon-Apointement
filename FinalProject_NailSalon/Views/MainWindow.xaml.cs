using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
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

using FinalProject_NailSalon.Views;

namespace FinalProject_NailSalon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScheduleViewModel scheduleViewModel = new ScheduleViewModel();

        #region Main Window
        public MainWindow()
        {
            InitializeComponent();

            UpdateDatagridSchedule();
            DataContext = scheduleViewModel;
        }
        #endregion

        #region Update Datagrid
        /// <summary>
        /// Read default Data XML Default
        /// Load data into data grid
        /// </summary>
        private void UpdateDatagridSchedule()
        {
            LoadDefaultDataFile();

            NailSalonXMLDAO NailSalonDAO = XMLHelpers.Instance();
            scheduleViewModel.Today = DateTime.Now;
            scheduleViewModel.AppointmentList = NailSalonDAO.Appointments;
        }
        #endregion


        #region Load Default Data File
        /// <summary>
        /// When program startup, the program will read default data file
        /// Default data file should be store in Debug or Release Folder
        /// If the defualt data file is not found, we should use Load Data feature
        /// to load data file into our program
        /// </summary>
        private void LoadDefaultDataFile()
        {
            string fileName = ConfigurationManager.AppSettings["XMLDataFile"];
            string path = PathHelper.GetFileLocation(fileName);
            if (File.Exists(path))
            {
                try
                {
                    XMLHelpers.ReadFromXML(fileName);
                }
                catch 
                {
                    //
                }
            }
        }
        #endregion

        #region Date Picker select date changed
        /// <summary>
        /// When the user select a different date to see appointments
        /// Data grid will update appointpment list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpCurrentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = dpCurrentDate.SelectedDate.Value.Date;
            scheduleViewModel.Today = date;
            scheduleViewModel.AppointmentDelegate = SearchHelper.ListAppointmentsOfToday;
            scheduleViewModel.RefreshAppointments();
        }

        #endregion

        #region Cancel Appointment
        /// <summary>
        /// The user only can upcomming appointment.
        /// When user click cancel appointment button,
        /// The program should check that the user select any appointment to cancel
        /// If not, the program will notify to user about that.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment selectedAppointment = (Appointment)dgTimeSchedule.SelectedItem;
            if(selectedAppointment == null)
            {
                MessageHelper.ShowWarningMessage("Please select an appointment first!", "Cancel Appointment");
                return;
            }

            if(selectedAppointment.DateTime < DateTime.Now)
            {
                MessageHelper.ShowWarningMessage("You only cancel upcoming appointments!", "Cancel Appointment");
                return;
            }

            MessageBoxResult answer = MessageHelper.ShowQuestionMessage(
                string.Format("Do you want to cancel appointment with {0} at {1:t}",selectedAppointment.Customer.FullName,selectedAppointment.DateTime), "Cancel Appointment");
            if(answer == MessageBoxResult.No || answer == MessageBoxResult.Cancel)
            {
                return;
            }
            //Reset select item of data grid
            dgTimeSchedule.SelectedItem = null;

            //Delete Appointment
            XMLHelpers.Instance().Appointments.Remove(selectedAppointment);
            scheduleViewModel.AppointmentList = XMLHelpers.Instance().Appointments;
            scheduleViewModel.RefreshAppointments();
        }
        #endregion

        #region Load Data from specific file path
        /// <summary>
        /// Open a dialog to select the xml file
        /// Load data from xml file 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoadData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                // a flag for store status of reading xml file, and catch exception message to display for users.
                try
                {
                    XMLHelpers.ReadFromXML(openFileDialog.FileName);
                    scheduleViewModel.AppointmentList = XMLHelpers.Instance().Appointments;
                    scheduleViewModel.RefreshAppointments();
                } catch(Exception error)
                {
                    MessageBox.Show(error.Message, "Failed to read " + System.IO.Path.GetFileName(openFileDialog.FileName), MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
        }
        #endregion

        #region Save Date to specific file path
        private void BtnSaveData_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == true)
            {
                XMLHelpers.WriteToXML(saveFileDialog.FileName);
            }
        }
        #endregion

        #region Search Feature
        /// <summary>
        /// When the user search the appointment, the system have 3 options for search
        /// If the user didn't choose any option, the search engine will be default(search everything)
        /// If the user choose option, the search engine will be followed the options.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = tbSearchTerm.Text;
            if (string.IsNullOrEmpty(searchTerm))
            {
                return;
            }

            scheduleViewModel.Keyword = searchTerm.ToLower().Trim();
            SelectSearchType();
            scheduleViewModel.RefreshAppointments();
        }
        #endregion

        #region Select Search Type Delegate to list appointment to data grid
        private void SelectSearchType()
        {
            if (cbCustomer.IsChecked == true && cbService.IsChecked == true)
            {
                scheduleViewModel.AppointmentDelegate = SearchHelper.SearchAppointmentsByServiceAndCustomerName;
            }
            else if (cbService.IsChecked == true)
            {
                scheduleViewModel.AppointmentDelegate = SearchHelper.SearchAppointmentsByServiceName;
            }
            else if (cbCustomer.IsChecked == true)
            {
                scheduleViewModel.AppointmentDelegate = SearchHelper.SearchAppointmentsByCustomerName;
            }
            else
            {
                scheduleViewModel.AppointmentDelegate = SearchHelper.SearchAppointments;
            }
        }
        #endregion

        #region Move to context of current date
        private void BtnToday_Click(object sender, RoutedEventArgs e)
        {
            dpCurrentDate.SelectedDate = DateTime.Now;
            scheduleViewModel.AppointmentDelegate = SearchHelper.ListAppointmentsOfToday;
            scheduleViewModel.RefreshAppointments();
        }
        #endregion

        #region New Appointment
        private void BtnNewAppointment_Click(object sender, RoutedEventArgs e)
        {
            //Reset for new apppointment
            StateHelper.EditedAppointment = null;
            StateHelper.IsEditedAppointmentMode = false;
            NewAppointmentWindow newAppointmentWindow = new NewAppointmentWindow();
            var result = newAppointmentWindow.ShowDialog();
            // When enter information successfully
            if (result.HasValue && result.Value)
            {
                scheduleViewModel.RefreshAppointments();
            }
        }
        #endregion

        #region New Employee
        private void BtnNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            NewEmployeeWindow newEmployeeWindow = new NewEmployeeWindow();
            var result = newEmployeeWindow.ShowDialog();

            // When enter information successfully
            if(result.HasValue && result.Value)
            {
                scheduleViewModel.RefreshAppointments();
            }
        }
        #endregion

        #region Update Black out Date in the calendar
        private void DpCurrentDate_CalendarOpened(object sender, RoutedEventArgs e)
        {
            dpCurrentDate.DisplayDateStart = DateTime.Now - TimeSpan.FromDays(100);
            dpCurrentDate.DisplayDateEnd = DateTime.Now + TimeSpan.FromDays(365);//A year

            var minDate = dpCurrentDate.DisplayDateStart ?? DateTime.MinValue;
            var maxDate = dpCurrentDate.DisplayDateEnd ?? DateTime.MaxValue;


            for (var date = minDate; date <= maxDate && DateTime.MaxValue > date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Monday && DateTime.Today.Date != date.Date)
                {
                    try
                    {
                        dpCurrentDate.BlackoutDates.Add(new CalendarDateRange(date));

                    }
                    catch
                    {

                    }
                }
            }
        }
        #endregion

        private void BtnEditAppointment_Click(object sender, RoutedEventArgs e)
        {
            StateHelper.EditedAppointment = (Appointment)dgTimeSchedule.SelectedItem;
            if(StateHelper.EditedAppointment == null)
            {
                return;
            }

            NewAppointmentWindow newAppointmentWindow = new NewAppointmentWindow();
            StateHelper.EditedAppointment = (Appointment)dgTimeSchedule.SelectedItem;
            StateHelper.IsEditedAppointmentMode = true;
            var result = newAppointmentWindow.ShowDialog();
            // When enter information successfully
            if (result.HasValue && result.Value)
            {
                scheduleViewModel.RefreshAppointments();
            }
        }
    }
}
