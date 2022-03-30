using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_STM
{
    public class DateAndTime
    {
        public int dd;
        public int mm;
        public int yy;
        public int hh;
        public int min;
        public int sec;

        public DateAndTime(int dd, int mm, int yy)
        {
            this.dd = dd;
            this.mm = mm;
            this.yy = yy;
        }

        public DateAndTime(int min, int sec)
        {
            this.min = min;
            this.sec = sec;
        }

        public DateAndTime(int dd, int mm, int yy,int hh, int min, int sec)
        {
            this.dd = dd;
            this.mm = mm;
            this.yy = yy;
            this.min = min;
            this.sec = sec;
            this.hh = hh;
        }

        public DateAndTime(int dd, int mm, int yy, int min, int sec)
        {
            this.dd = dd;
            this.mm = mm;
            this.yy = yy;
            this.min = min;
            this.sec = sec;
        }

        public DateAndTime(string minAndSec)
        {
            int.TryParse(minAndSec.Substring(0,2), out min);
            int.TryParse(minAndSec.Substring(3, 2), out sec);
        }

        public void setData(int dd, int mm, int yy)
        {
            this.dd = dd;
            this.mm = mm;
            this.yy = yy;
        }

        public void setCurrentData()
        {
            DateTime dat = DateTime.Today;
            this.dd = dat.Day;
            this.mm = dat.Minute;
            this.yy = dat.Year;
        }

        public string getTime()
        {
            string hours = "";
            if (hh > 0)
            {
                hours = hh.ToString() + ":";
                if (hh < 10)
                {
                    hours = "0" + hh.ToString() + ":";
                }
            }

            string minut = "";
            minut = min.ToString();
            if (min < 10)
            {
                minut = "0" + min.ToString();
            }

            string secun = "";
            secun = sec.ToString();
            if (sec < 10)
            {
                secun = "0" + sec.ToString();
            }

            return hours + minut + ":" + secun;
        }

        public string getData()
        {
            return dd.ToString() + ":" + mm.ToString() + ":" + yy.ToString();
        }

    }
}
