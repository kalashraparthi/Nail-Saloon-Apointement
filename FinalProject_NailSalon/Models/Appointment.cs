using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinalProject_NailSalon
{
    public class Appointment
    {
        private string id;
        private DateTime dateTime;
        private Customer customer;
        private Service service;
        private Employee employee;

        public Appointment()
        {
            id = "";
            customer = new Customer();
            dateTime = new DateTime();
            service = new Service();
            employee = new Employee();
        }

        public Appointment(DateTime dateTime, Customer customer, Service service, Employee employee)
        {
            this.dateTime = dateTime;
            this.customer = customer;
            this.service = service;
            this.employee = employee;
        }

        public Appointment(string id, DateTime dateTime, Customer customer, Service service, Employee employee)
        {
            this.id = id;
            this.dateTime = dateTime;
            this.customer = customer;
            this.service = service;
            this.employee = employee;
        }


        [XmlElement("AppointmentID")]
        public string ID { get => id; set => id = value; }

        [XmlIgnore]
        public DateTime DateTime { get => dateTime; set => dateTime = value; }

        //Parse XML DateTime Format String
        [XmlElement("DateTime")]
        public string DateTimeXML
        {
            get => this.dateTime.ToString("yyyy-MM-dd hh:mm tt");
            set => this.dateTime = DateTime.Parse(value);
        }

        [XmlElement("Customer")]
        public Customer Customer { get => customer; set => customer = value; }

        [XmlElement("Service")]
        public Service Service { get => service; set => service = value; }

        [XmlElement("Employee")]
        public Employee Employee { get => employee; set => employee = value; }

        public DateTime TimeStart
        {
            get => dateTime;
        }

        public DateTime TimeEnd
        {
            get => dateTime.AddMinutes(service.Minutes);
        }


    }
}
