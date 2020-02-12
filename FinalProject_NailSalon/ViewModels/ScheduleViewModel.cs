using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_NailSalon
{
    public delegate List<Appointment> AppointmentDelegate(List<Appointment> appointments, object term);
    class ScheduleViewModel
    {
        private List<Appointment> appointmentList;
        private ObservableCollection<Appointment> appointmentCollection = new ObservableCollection<Appointment>();
        private DateTime today;
        

        public ScheduleViewModel()
        {
            AppointmentDelegate = SearchHelper.ListAppointmentsOfToday;
        }

        public ScheduleViewModel(List<Appointment> appointmentList, DateTime today)
        {
            this.appointmentList = appointmentList;
            this.today = today;

            AppointmentDelegate = SearchHelper.ListAppointmentsOfToday;
        }

        public void UpdateSchedule(List<Appointment> appointmentList, DateTime today)
        {
            this.appointmentList = appointmentList;
            this.today = today;
        }

        #region Observable Collection Appointment
        /// <summary>
        /// Observable collection for updating data binding of data grid
        /// </summary>
        public ObservableCollection<Appointment> Appointments {
            get
            {
                RefreshAppointments();
                return appointmentCollection;
            }
            set => appointmentCollection = value;
        }
        #endregion

        public List<Appointment> AppointmentList
        {
            get {
                    object term = null;
                    if(AppointmentDelegate == SearchHelper.SearchAppointments
                       || AppointmentDelegate == SearchHelper.SearchAppointmentsByCustomerName
                       || AppointmentDelegate == SearchHelper.SearchAppointmentsByServiceName
                       || AppointmentDelegate == SearchHelper.SearchAppointmentsByServiceAndCustomerName)
                    {
                        term = Keyword;
                    } else if(AppointmentDelegate == SearchHelper.ListAppointmentsOfToday)
                    {
                        term = today;
                    }
                return AppointmentDelegate(appointmentList, term);
                }
            set => appointmentList = value;
        }


        public Appointment SelectedAppointment { get ; set; }

        public DateTime Today {
            get => today;
            set
            {
                RefreshAppointments();
                today = value;

            }
        }
        public AppointmentDelegate AppointmentDelegate { get; set; }
        public string Keyword { get; set; }
   
        public void RefreshAppointments()
        {
            appointmentCollection.Clear();
            foreach (Appointment appointment in AppointmentList)
            {
                appointmentCollection.Add(appointment);
            }
        }
    }
}
