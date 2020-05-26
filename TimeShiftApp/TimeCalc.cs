using System;

namespace TimeShiftApp//Функции округления времени для всех кроме выносов схема (0>=n>10+n и так далее) для выносов (0>5>10)
{
    class TimeCalc
    {
        //добавлен временной промежуток в который все время доставки увеличивается на 20 минут
        public static TimeSpan timeFrom = new TimeSpan(Convert.ToInt32(Form1.ReadS("timeFromHour")), Convert.ToInt32(Form1.ReadS("timeFromMinutes")), 0);
        public static TimeSpan timeTo = new TimeSpan(Convert.ToInt32(Form1.ReadS("timeToHour")), Convert.ToInt32(Form1.ReadS("timeToMinutes")), 0);

        public static String CalculateNear(DateTime tempTime, int h_shift, int m_shift, int n_shift)
        {

            /*var timeNow = DateTime.Now.TimeOfDay;
            if (timeFrom < timeNow && timeNow < timeTo)
            {
                m_shift = m_shift + Convert.ToInt32(Form1.ReadS("MinutesToAdd"));
            }*/



            String TS;

            if (((tempTime.Minute + m_shift) >= 0) && ((tempTime.Minute + m_shift) <= n_shift))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":00";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift) && ((tempTime.Minute + m_shift) <= n_shift + 10))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":10";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 10) && ((tempTime.Minute + m_shift) <= n_shift + 20))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":20";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 20) && ((tempTime.Minute + m_shift) <= n_shift + 30))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":30";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 30) && ((tempTime.Minute + m_shift) <= n_shift + 40))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":40";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 40) && ((tempTime.Minute + m_shift) <= n_shift + 50))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":50";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 50) && ((tempTime.Minute + m_shift) <= n_shift + 60))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":00";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 60) && ((tempTime.Minute + m_shift) <= n_shift + 70))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":10";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 70) && ((tempTime.Minute + m_shift) <= n_shift + 80))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":20";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 80) && ((tempTime.Minute + m_shift) <= n_shift + 90))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":30";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 90) && ((tempTime.Minute + m_shift) <= n_shift + 100))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":40";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 100) && ((tempTime.Minute + m_shift) <= n_shift + 110))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":50";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 110) && ((tempTime.Minute + m_shift) <= n_shift + 120))
            {
                TS = tempTime.AddHours(h_shift + 2).Hour.ToString() + ":00";
                return TS;
            }
            return TS = "00:00";
        }

        public static String CalculateTaway(DateTime tempTime, int h_shift, int m_shift, int n_shift)
        {
            String TS;
            if (((tempTime.Minute + m_shift) >= 0) && ((tempTime.Minute + m_shift) <= n_shift))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":05";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift) && ((tempTime.Minute + m_shift) <= n_shift + 5))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":10";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 10) && ((tempTime.Minute + m_shift) <= n_shift + 15))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":15";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 15) && ((tempTime.Minute + m_shift) <= n_shift + 20))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":20";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 20) && ((tempTime.Minute + m_shift) <= n_shift + 25))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":25";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 25) && ((tempTime.Minute + m_shift) <= n_shift + 30))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":30";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 30) && ((tempTime.Minute + m_shift) <= n_shift + 35))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":35";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 35) && ((tempTime.Minute + m_shift) <= n_shift + 40))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":40";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 40) && ((tempTime.Minute + m_shift) <= n_shift + 45))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":45";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 45) && ((tempTime.Minute + m_shift) <= n_shift + 50))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":50";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 50) && ((tempTime.Minute + m_shift) <= n_shift + 55))
            {
                TS = tempTime.AddHours(h_shift).Hour.ToString() + ":55";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 55) && ((tempTime.Minute + m_shift) <= n_shift + 60))
            {
                TS = tempTime.AddHours(h_shift+1).Hour.ToString() + ":00";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 60) && ((tempTime.Minute + m_shift) <= n_shift + 65))
            {
                TS = tempTime.AddHours(h_shift+1).Hour.ToString() + ":05";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 65) && ((tempTime.Minute + m_shift) <= n_shift + 70))
            {
                TS = tempTime.AddHours(h_shift+1).Hour.ToString() + ":10";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 70) && ((tempTime.Minute + m_shift) <= n_shift + 75))
            {
                TS = tempTime.AddHours(h_shift+1).Hour.ToString() + ":15";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 75) && ((tempTime.Minute + m_shift) <= n_shift + 80))
            {
                TS = tempTime.AddHours(h_shift+1).Hour.ToString() + ":20";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 80) && ((tempTime.Minute + m_shift) <= n_shift + 85))
            {
                TS = tempTime.AddHours(h_shift+1).Hour.ToString() + ":25";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 85) && ((tempTime.Minute + m_shift) <= n_shift + 90))
            {
                TS = tempTime.AddHours(h_shift+1).Hour.ToString() + ":30";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 90) && ((tempTime.Minute + m_shift) <= n_shift + 95))
            {
                TS = tempTime.AddHours(h_shift+1).Hour.ToString() + ":35";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 95) && ((tempTime.Minute + m_shift) <= n_shift + 100))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":40";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 100) && ((tempTime.Minute + m_shift) <= n_shift + 105))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":45";
                return TS;
            }
            else if (((tempTime.Minute + m_shift) > n_shift + 95) && ((tempTime.Minute + m_shift) <= n_shift + 100))
            {
                TS = tempTime.AddHours(h_shift + 1).Hour.ToString() + ":40";
                return TS;
            }
            return TS = "00:00";
        }
    }

    

}

