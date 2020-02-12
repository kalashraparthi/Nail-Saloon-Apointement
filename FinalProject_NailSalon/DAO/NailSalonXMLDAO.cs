using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinalProject_NailSalon
{
    [XmlRoot("NailSalon")]
    public class NailSalonXMLDAO
    {
        private List<Employee> employees;
        private List<Service> services;
        private List<Customer> customers;
        private List<Appointment> appointments;

        public NailSalonXMLDAO()
        {
            employees = new List<Employee>();
            services = new List<Service>();
            appointments = new List<Appointment>();
            customers = new List<Customer>();
        }
        public NailSalonXMLDAO(List<Employee> employees, List<Service> services, List<Appointment> appointments, List<Customer> customers)
        {
            this.employees = employees;
            this.services = services;
            this.appointments = appointments;
            this.customers = customers;
        }


        [XmlArray("Employees")]
        [XmlArrayItem("Employee", typeof(Employee))]
        public List<Employee> Employees { get => employees; set => employees = value; }

        [XmlArray("Services")]
        [XmlArrayItem("Service", typeof(Service))]
        public List<Service> Services { get => services; set => services = value; }

        [XmlArray("Customers")]
        [XmlArrayItem("Customer", typeof(Customer))]
        public List<Customer> Customers { get => customers; set => customers = value; }

        [XmlArray("Appointments")]
        [XmlArrayItem("Appointment", typeof(Appointment))]
        public List<Appointment> Appointments { get => appointments; set => appointments = value; }

        
    }
}
