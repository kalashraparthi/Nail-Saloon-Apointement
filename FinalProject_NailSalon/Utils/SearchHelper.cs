using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_NailSalon
{
    class SearchHelper
    {
        #region List Appointment of selected date delegate
        /// <summary>
        /// List all appointments
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static List<Appointment> ListAppointmentsOfToday(List<Appointment> appointments, object term)
        {
            DateTime today = (DateTime)term;
            if (appointments == null || appointments.Count == 0)
            {
                return new List<Appointment>();
            }
            var listedAppointments = from appointment in appointments
                                     where appointment.DateTime.Date == today.Date
                                     orderby appointment.DateTime ascending
                                     select appointment;

            return listedAppointments.ToList();
        }

        #endregion
        /// <summary>
        /// List all appoints by serch term
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static List<Appointment> SearchAppointments(List<Appointment> appointments, object term)
        {
            if (appointments == null || appointments.Count == 0)
            {
                return new List<Appointment>();
            }
            string keyword = (string)term;
            var listedAppointments = appointments.Where(appointment => appointment.Customer.FullName.ToLower().Contains(keyword) 
                                                                       || appointment.Service.Name.ToLower().Contains(keyword)
                                                                       || appointment.Employee.FullName.ToLower().Contains(keyword))
                .OrderBy(appointment => appointment.DateTime);

            return listedAppointments.ToList();
        }

        /// <summary>
        /// List all appointments by customer name with search term
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static List<Appointment> SearchAppointmentsByCustomerName(List<Appointment> appointments, object term)
        {
            if (appointments == null || appointments.Count == 0)
            {
                return new List<Appointment>();
            }
            string keyword = (string)term;
            var listedAppointments = appointments.Where(appointment => appointment.Customer.FullName.ToLower().Contains(keyword))
                .OrderBy(appointment => appointment.DateTime);

            return listedAppointments.ToList();
        }

        /// <summary>
        /// List all upcomming appointments by service name with search term
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static List<Appointment> SearchAppointmentsByServiceName(List<Appointment> appointments, object term)
        {
            if (appointments == null || appointments.Count == 0)
            {
                return new List<Appointment>();
            }
            string keyword = (string)term;
            var listedAppointments = appointments.Where(appointment => appointment.Service.Name.ToLower().Contains(keyword) && appointment.DateTime >= DateTime.Now)
                .OrderBy(appointment => appointment.DateTime);

            return listedAppointments.ToList();
        }

        public static List<Appointment> SearchAppointmentsByServiceAndCustomerName(List<Appointment> appointments, object term)
        {
            if (appointments == null || appointments.Count == 0)
            {
                return new List<Appointment>();
            }
            string keyword = (string)term;
            var listedAppointments = appointments.Where(appointment => appointment.Customer.FullName.ToLower().Contains(keyword)|| (appointment.Service.Name.ToLower().Contains(keyword) && appointment.DateTime >= DateTime.Now))
                .OrderBy(appointment => appointment.DateTime);

            return listedAppointments.ToList();
        }
    }
}
