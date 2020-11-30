using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeShiftApp;
using System.Configuration;


namespace TimeShiftApp
{
    class CalculateFinal
    {
        public static DateTime timeFromString = Convert.ToDateTime(Form1.ReadS("timeFromHour") + ":" + Form1.ReadS("timeFromMinutes") + ":00");
        public static DateTime timeToString = Convert.ToDateTime(Form1.ReadS("timeToHour") + ":" + Form1.ReadS("timeToMinutes") + ":00");
        public static string tEdit;

        //Функция подсчета времени цветовой зоны с учетом стопов и оттяжек
        public static async Task<string> CalcZoneTime(DateTime TZ, string shift, string nn, string zname, List<string> ListTimes)
        {
            if (shift != "0:00")
            {
                int h = Int32.Parse(shift.Split(new char[] { ':' })[0]);
                int m = Int32.Parse(shift.Split(new char[] { ':' })[1]);
                int _nn = Int32.Parse(nn);

                string selectDDelays = "Select Delay from Delays where Flag=1 and Type='Доставка' and Zone='",
                          connstr = ConfigurationManager.AppSettings.Get("connstr");


                //string name = ((Button)sender).Name;
                //вычисляем оттяжку на доставку    
                double DDelay = await DBselect.CalculateDelay(connstr, zname, selectDDelays + zname + "'");

                //double TADelay = DBselect.CalculateDelay(connstr, zname, selectTADelays + zname + "'").Result;
                //вычисляем время для цветовой зоны и добавляем время оттяжки если оно есть
                //в функцию TimeCalc.CalculateNear подставляем значения переменных из App.Config для каждой цветовой зоны при
                if(TZ > timeFromString && TZ < timeToString)
                {
                    tEdit = Convert.ToDateTime(TimeCalc.CalculateNear(TZ.AddMinutes(Convert.ToInt32(Form1.ReadS("timeAddMinutes"))), h, m, _nn)).AddMinutes(Convert.ToDouble(DDelay)).ToShortTimeString();
                }
                else
                {
                    tEdit = Convert.ToDateTime(TimeCalc.CalculateNear(TZ, h, m, _nn)).AddMinutes(Convert.ToDouble(DDelay)).ToShortTimeString();
                }
                

                //Сравниваем вычисленное время по цветовой зоне с временами на стопе, если совпадает, то прибавляем 10 мин, и сравниваем далее, времена в списке отсортированы по возрастанию
                foreach (string s in ListTimes)
                {
                    if (tEdit == s)
                    {
                        tEdit = Convert.ToDateTime(tEdit).AddMinutes(10).ToShortTimeString();
                    }
                }

                return tEdit;
            }
            else
            {
                return "";
            }

        }

        //Вычисление времени на вынос
        public static async Task<string> CalcZoneTATime(DateTime TZ, string shift, string nn, string zname)
        {
            if (shift != "0:00")
            {
                int h = Int32.Parse(shift.Split(new char[] { ':' })[0]);
                int m = Int32.Parse(shift.Split(new char[] { ':' })[1]);
                int _nn = Int32.Parse(nn);

                string selectDDelays = "Select Delay from Delays where Flag=1 and Type='Вынос' and Zone='",
                          connstr = ConfigurationManager.AppSettings.Get("connstr");


                //string name = ((Button)sender).Name;
                //вычисляем оттяжку на вынос    
                double TADelay = await DBselect.CalculateDelay(connstr, zname, selectDDelays + zname + "'");
                //double TADelay = DBselect.CalculateDelay(connstr, zname, selectTADelays + zname + "'").Result;
                //вычисляем время для цветовой зоны и добавляем время оттяжки если оно есть
                //в функцию TimeCalc.CalculateNear подставляем значения переменных из App.Config для каждой цветовой зоны при
                string tEdit = Convert.ToDateTime(TimeCalc.CalculateTaway(TZ, h, m, _nn)).AddMinutes(Convert.ToDouble(TADelay)).ToShortTimeString();

                //Сравниваем вычисленное время по цветовой зоне с временами на стопе, если совпадает, то прибавляем 10 мин, и сравниваем далее, времена в списке отсортированы по возрастанию
                return tEdit;
            }
            else
            {
                return "";
            }

        }

        public static async Task<string> CalcBlueZoneTime(DateTime TZ, string shift, string nn, string zname, List<string> ListTimes)
        {
            if (shift != "0:00")
            {
                int h = Int32.Parse(shift.Split(new char[] { ':' })[0]);
                int m = Int32.Parse(shift.Split(new char[] { ':' })[1]);
                int _nn = Int32.Parse(nn);

                string selectDDelays = "Select Delay from Delays where Flag=1 and Type='Пеший' and Zone='",
                          connstr = ConfigurationManager.AppSettings.Get("connstr");


                //string name = ((Button)sender).Name;
                //вычисляем оттяжку на доставку    
                double WDelay = DBselect.CalculateDelay(connstr, zname, selectDDelays + zname + "'").Result;
                //double TADelay = DBselect.CalculateDelay(connstr, zname, selectTADelays + zname + "'").Result;
                //вычисляем время для цветовой зоны и добавляем время оттяжки если оно есть
                //в функцию TimeCalc.CalculateNear подставляем значения переменных из App.Config для каждой цветовой зоны при
                //string tEdit = Convert.ToDateTime(TimeCalc.CalculateNear(TZ, h, m, _nn)).AddMinutes(Convert.ToDouble(DDelay)).ToShortTimeString();
                string tEdit = Convert.ToDateTime(TimeCalc.CalculateNear(TZ, h, m, _nn)).AddMinutes(Convert.ToDouble(WDelay)).ToShortTimeString();

                if (TZ > timeFromString && TZ < timeToString)
                {
                    tEdit = Convert.ToDateTime(TimeCalc.CalculateNear(TZ.AddMinutes(Convert.ToInt32(Form1.ReadS("timeAddMinutes"))), h, m, _nn)).AddMinutes(Convert.ToDouble(WDelay)).ToShortTimeString();
                }
                else
                {
                    tEdit = Convert.ToDateTime(TimeCalc.CalculateNear(TZ, h, m, _nn)).AddMinutes(Convert.ToDouble(WDelay)).ToShortTimeString();
                }


                //Сравниваем вычисленное время по цветовой зоне с временами на стопе, если совпадает, то прибавляем 10 мин, и сравниваем далее, времена в списке отсортированы по возрастанию
                foreach (string s in ListTimes)
                {
                    if (tEdit == s)
                    {
                        tEdit = Convert.ToDateTime(tEdit).AddMinutes(10).ToShortTimeString();
                    }
                }

                return tEdit;
            }
            else
            {
                return "";
            }

        }
    }
}
