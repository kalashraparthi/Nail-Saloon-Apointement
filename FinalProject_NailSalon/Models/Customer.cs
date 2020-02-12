using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinalProject_NailSalon
{
    public class Customer
    {
        private string id;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string address;

        public Customer()
        {
            id = "";
            firstName = "";
            lastName = "";
            phoneNumber = "";
            email = "";
            address = "";
        }

        public Customer(string firstName, string lastName, string phoneNumber, string email, string address)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.address = address;
        }

        public Customer(string id, string firstName, string lastName, string phoneNumber, string email, string address)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.address = address;
        }

        [XmlElement("CustomerID")]
        public string ID { get => id; set => id = value; }

        [XmlElement("FirstName")]
        public string FirstName { get => firstName; set => firstName = value; }

        [XmlElement("LastName")]
        public string LastName { get => lastName; set => lastName = value;}
        public string FullName { get => firstName + " " + lastName; }

        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }

        [XmlElement("Email")]
        public string Email { get => email; set => email = value; }

        [XmlElement("Address")]
        public string Address { get => address; set => address = value; }
    }
}
