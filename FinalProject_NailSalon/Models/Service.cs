using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinalProject_NailSalon
{
    public class Service
    {
        private string id;
        private string name;
        private Decimal price;
        private double minutes;

        public Service()
        {
            id = "";
            name = "";
            price = 0;
            minutes = 0;
        }

        public Service(string id, string name, decimal price, double minutes)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.minutes = minutes;
        }

        [XmlElement("ServiceID")]
        public string ID { get => id; set => id = value; }

        [XmlElement("ServiceName")]
        public string Name { get => name; set => name = value; }

        [XmlElement("ServicePrice")]
        public Decimal Price { get => price; set => price = value; }

        [XmlElement("ServiceMinutes")]
        public double Minutes { get => minutes; set => minutes = value; }

        public override string ToString()
        {
            return name;
        }
    }
}
