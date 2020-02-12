using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_NailSalon
{
    public class Workday
    {
        private int id;
        private DateTime date;
        private DateTime timeStart;
        private DateTime timeEnd;

        public Workday()
        {
            id = 0;
            date = new DateTime();
            timeStart = new DateTime();
            timeEnd = new DateTime();
        }

        public Workday(int id, DateTime date, DateTime timeStart, DateTime timeEnd)
        {
            this.id = id;
            this.date = date;
            this.timeStart = timeStart;
            this.timeEnd = timeEnd;
        }

        public int ID { get => id; set => id = value; }
        public DateTime Date { get => date; set => date = value; }
        public DateTime TimeStart { get => timeStart; set => timeStart = value; }
        public DateTime TimeEnd { get => timeEnd; set => timeEnd = value; }
    }
}
