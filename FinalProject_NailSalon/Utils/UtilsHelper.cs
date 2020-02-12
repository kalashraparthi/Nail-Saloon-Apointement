using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_NailSalon
{
    class UtilsHelper
    {
        enum IDType {
            Employee = 'E',
            Appointment = 'A',
            Customer = 'C',
            Service = 'S',
            Default = ' ',
        }
        public static List<DateTime> ListWorkingHour(TimeSpan startHours, TimeSpan endHours)
        {
            List<DateTime> workingHoursList = new List<DateTime>();
            for(TimeSpan currentHours = startHours; currentHours < endHours; currentHours = currentHours.Add(TimeSpan.FromMinutes(15)))
            {
                workingHoursList.Add(DateTime.Today.Add(currentHours));
            }
            return workingHoursList;
        }

        public static bool IsEndThedate()
        {
            DateTime today = DateTime.Now;
            if(today.DayOfWeek == DayOfWeek.Monday)
            {
                return true;
            }
            TimeSpan timeSpan = TimeSpan.FromHours(17.5);

            if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
            {
                if(TimeSpan.Compare(today.TimeOfDay, timeSpan) == 1)
                {
                    return true;
                }
            }
            timeSpan = TimeSpan.FromHours(18.5);
            if (TimeSpan.Compare(today.TimeOfDay, timeSpan) == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check the hour is available for booking
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static bool IsValidHoursOf(DateTime date, DateTime hour)
        {
            List<Appointment> appointmentLisbyDate = SearchHelper.ListAppointmentsOfToday(XMLHelpers.Instance().Appointments, date);
            foreach(Appointment appointment in appointmentLisbyDate)
            {
                if(hour >= appointment.TimeStart && hour <= appointment.TimeEnd)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Moday to Friday: 9AM to 7PM(19:00)
        /// Staturday and Sunday: 10AM to 6PM(18:00)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double[] TimeRange(DateTime date)
        {
            double[] hours = new double[] { 9,19};
            if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                hours[0] = 10;
                hours[1] = 18;
            }
            //Roud up minutes to a quater
            if(date.Date == DateTime.Today)
            {
                int minutes = DateTime.Now.Minute;
                int remainMinutes = (minutes + 15) % 15;
                remainMinutes = 15 - remainMinutes;
                minutes += remainMinutes;
                DateTime dateNow = DateTime.Now;
                TimeSpan timeSpan = new TimeSpan(dateNow.Hour, 0, 0);
                dateNow = dateNow.Date + timeSpan;
                dateNow = dateNow.AddMinutes(minutes);
                hours[0] = dateNow.Hour + (dateNow.Minute / 60.0);
            }
            return hours;
        }

        public static string GenerateID(object obj)
        {
            IDType type = GetTypeObj(obj);
            int id = GetMaxIDvalue(type) + 1;
            return (char)type+""+id;
        }

        private static IDType GetTypeObj(object obj)
        {
            if (obj is Employee)
            {
                return IDType.Employee;
            } else if (obj is Appointment)
            {
                return IDType.Appointment;
            } else if(obj is Customer)
            {
                return IDType.Customer;
            } else if (obj is Service)
            {
                return IDType.Customer;
            }

            return IDType.Default ;
        }

        /// <summary>
        /// Format of [Letter][0-9]
        /// Employee id will start from E1
        /// Customer id will start from C1
        /// Appointment id will start from A1
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static int GetMaxIDvalue(IDType type)
        {
            Random rand = new Random();
            switch (type)
            {
                case IDType.Employee:
                    return GetMaxEmployeeIDvalue(rand);
                case IDType.Appointment:
                    return GetMaxApppointmentIDvalue(rand);
                case IDType.Customer:
                    return GetMaxCustomerIDvalue(rand);
                case IDType.Service:
                    return GetMaxServiceIDvalue(rand);
                default:
                    return rand.Next();
            }
            

        }

        private static int GetMaxEmployeeIDvalue(Random rand)
        {
            List<Employee> employees = XMLHelpers.Instance().Employees;
            if(employees == null || employees.Count == 0)
            {
                return 0;
            }
            return employees.Select(employee => {
                string id = employee.ID.Substring(1);
                if (IsNumberString(id))
                {
                    return int.Parse(id);
                }
                return rand.Next();
            }).Max();
        }

        private static int GetMaxCustomerIDvalue(Random rand)
        {
            List<Customer> customers = XMLHelpers.Instance().Customers;
            if(customers == null || customers.Count == 0)
            {
                return 0;
            }
            return customers.Select(customer => {
                string id = customer.ID.Substring(1);
                if (IsNumberString(id))
                {
                    return int.Parse(id);
                }
                return rand.Next();
            }).Max();
        }
        private static int GetMaxServiceIDvalue(Random rand)
        {
            List<Service> services = XMLHelpers.Instance().Services;
            if(services == null || services.Count == 0)
            {
                return 0;
            }
            return services.Select(service => {
                string id = service.ID.Substring(1);
                if (IsNumberString(id))
                {
                    return int.Parse(id);
                }
                return rand.Next();
            }).Max();
        }
        private static int GetMaxApppointmentIDvalue(Random rand)
        {
            List<Appointment> appointments = XMLHelpers.Instance().Appointments;
            if(appointments == null || appointments.Count == 0)
            {
                return 0;
            }
            return appointments.Select(appointment => {
                string id = appointment.ID.Substring(1);
                if (IsNumberString(id))
                {
                    return int.Parse(id);
                }
                return rand.Next();
            }).Max();
        }
        public static bool IsNumberString(String inputString)
        {
            return float.TryParse(inputString, out float num);
        }

        public static bool IsEmployeeAvailableOn(DateTime dateTime, Employee employee, Appointment currentAppointment)
        {
            List<Appointment> appointments = XMLHelpers.Instance().Appointments;
            List<Appointment> reservedAppointments = appointments.Where(appointment => appointment.DateTime.Date == dateTime.Date && appointment.Employee == employee ).ToList();
            
            foreach(Appointment appointment in reservedAppointments)
            {
                if ((appointment.DateTime <= dateTime) && (dateTime <= appointment.TimeEnd) && (currentAppointment.ID != appointment.ID))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
