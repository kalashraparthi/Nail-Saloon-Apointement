using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_NailSalon
{
    class AppointmentViewModel
    {
        private ObservableCollection<Employee> employees;
        private ObservableCollection<DateTime> workingHours;
        private DateTime today;
        public AppointmentViewModel()
        {
            employees = new ObservableCollection<Employee>();
            workingHours = new ObservableCollection<DateTime>();

            today = DateTime.Now;
        }
        public List<Employee> EmployeeList { get; set; }
        public ObservableCollection<Employee> Employees
        {
            get
            {
                //Refresh observable collection of employee list
                employees.Clear();
                foreach(Employee employee in EmployeeList)
                {
                    employees.Add(employee);
                }
                return employees;
            }
        }
        public List<Service> Services { get; set; }
        public TimeSpan StartHours {
            get
            {
                double []timeRange = UtilsHelper.TimeRange(today);
                return TimeSpan.FromHours(timeRange[0]);
            }
        }
        public TimeSpan EndHours {
            get
            {
                double[] timeRange = UtilsHelper.TimeRange(today);
                return TimeSpan.FromHours(timeRange[1]);
            }
        }
        public DateTime Today { get => today; set => today = value; }

        public Customer Customer { get; set; }

        public ObservableCollection<DateTime> WorkingHours
        {
            get
            {
                RefreshWorkingHours();
                return workingHours;
            }
        }

        public void RefreshWorkingHours()
        {
            List<DateTime> workingHourList = UtilsHelper.ListWorkingHour(StartHours, EndHours);
            workingHours.Clear();
            foreach (DateTime workingHour in workingHourList)
            {
                workingHours.Add(workingHour);
            }
        }
    }
}
