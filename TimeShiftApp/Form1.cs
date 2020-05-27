using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeShiftApp;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Media;

namespace TimeShiftApp
{
    

    public partial class Form1 : Form
    {
        public int panel2_statusFlag = 0,
                  form_statusFlag = 1,
                    flag = 0;
        

        public string selectTimes = "Select Zone,Time from Times where Flag=1 and Zone='",
                     selectDDelays = "Select Delay from Delays where Flag=1 and Type='Доставка' and Zone='",
                     selectTADelays = "Select Delay from Delays where Flag=1 and Type='Вынос' and Zone='",
                     selectDishes = "Select Dish from DishesLog where Flag='Set' and Zone='",
                     selectOffers = "Select Name,Description,Zone,RunTime,EndTime FROM Offers where RunTime <= CURRENT_TIMESTAMP() and EndTime >= CURRENT_TIMESTAMP() and Zone ='",
                     connstr = ReadS("connstr");

        public AtcInfo Res = new AtcInfo();

        private void playSimpleSound()
        {
            SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.WHF);
            simpleSound.Play();
        }

        string OperNumberLoad()
        {
            string[] p = { "", "" };
            try
            {
                string[] userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\');
                string[] strs = File.ReadAllLines(@"C:\Users\" + userName[1] + @"\AppData\local\3CX VoIP Phone\3CXVoipPhone.ini", Encoding.Default);
                p = strs.Where(x => x.Contains("CallerID")).Select(x => x).ToArray()[0].Split('=');
                return p[1];
            }
            catch (Exception ex)
            {
               return "оператор не определён";
            }
        }

        public static string ReadS(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                //Console.WriteLine(result);
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return "";
            }
            
        }

        //Convert "1:30" like strings in (90)string function
        string ReturnMinutes(string tPar)
        {
            string res;
            TimeSpan ts = new TimeSpan(Convert.ToInt32(tPar.Split(':')[0]), Convert.ToInt32(tPar.Split(':')[1]), 0);
            res = "("+ts.TotalMinutes.ToString()+")";

            return res;
        }


        //Функция объединяющая весь сбор информации по всем рестам кроме перцев
        //Из-за особенностей хранения данных по Акция для Перцев не возможно передавать при нажатии на кнопку каждого ресторана Имя ресторана, потому что нужно слово 'Перцы' для всех 7 кнопок
        void GetAllInfo(object sender, string[] tParams, string oname)
        {
            string bluez,
                    lightgreenz,
                    greenz,
                    yellowz,
                    orangez,
                    redz,
                    grayz;


            this.Refresh();
            dateTimePicker1.Text = DateTime.Now.ToShortDateString();
            button16.Visible = false;
            TimeWindowResize();
            restName.Text = ((Button)sender).Text;
            string zname = ((Button)sender).Name;
            label11.Text = zname;
            DateTime TZ = DateTime.Now;

            curTime.Text = TZ.ToLongTimeString();
            List<string> ListTimes = new List<string>();
            List<string> ListDish = new List<string>();
            List<string> ListOffers = new List<string>();

            string whenStops = DateTime.Now.ToString("yyyy-MM-dd");

            ListTimes = DBselect.CalculateTimes(connstr, zname, selectTimes + zname + "' and DateofStop='" + whenStops + "' order by Time").Result;
            ListDish = DBselect.SelectDishStops(connstr, zname, selectDishes + zname + "'order by Dish").Result;
            ListOffers = DBselect.SelectOffers(connstr, zname, selectOffers + oname + "'").Result;

            double TADelay = DBselect.CalculateDelay(connstr, zname, selectTADelays + zname + "'").Result;
            double DDelay = DBselect.CalculateDelay(connstr, zname, selectDDelays + zname + "'").Result;

            if (DDelay != 0)
            {
                delay_label.Text = "+" + DDelay.ToString();
            }
            else
            {
                delay_label.Text = "";

            }
            
            TimeSpan timeFrom = new TimeSpan(Convert.ToInt32(Form1.ReadS("timeFromHour")), Convert.ToInt32(Form1.ReadS("timeFromMinutes")), 0);
            TimeSpan timeTo = new TimeSpan(Convert.ToInt32(Form1.ReadS("timeToHour")), Convert.ToInt32(Form1.ReadS("timeToMinutes")), 0);
            TimeSpan timeAdd = new TimeSpan(Convert.ToInt32(Form1.ReadS("timeAddHour")), Convert.ToInt32(Form1.ReadS("timeAddMinutes")), 0);

            bluez = CalculateFinal.CalcBlueZoneTime(TZ, tParams[0], tParams[7], zname, ListTimes).Result;
            lightgreenz = CalculateFinal.CalcZoneTime(TZ, tParams[1], tParams[7], zname, ListTimes).Result;
            greenz = CalculateFinal.CalcZoneTime(TZ, tParams[2], tParams[7], zname, ListTimes).Result;
            yellowz = CalculateFinal.CalcZoneTime(TZ, tParams[3], tParams[7], zname, ListTimes).Result;
            orangez = CalculateFinal.CalcZoneTime(TZ, tParams[4], tParams[7], zname, ListTimes).Result;
            redz = CalculateFinal.CalcZoneTime(TZ, tParams[5], tParams[7], zname, ListTimes).Result;
            grayz = CalculateFinal.CalcZoneTime(TZ, tParams[6], tParams[7], zname, ListTimes).Result;

            if (bluez != "")
            {
                if (TimeSpan.Parse(bluez) > timeFrom && TimeSpan.Parse(bluez) < timeTo)
                {
                    blueTZ.Text = TimeSpan.Parse(bluez).Add(timeAdd).ToString(@"hh\:mm") + ReturnMinutes(tParams[0]);
                }
                else
                {
                    blueTZ.Text = bluez + ReturnMinutes(tParams[0]);
                }
            }
            else
            {
                blueTZ.Text = bluez;
            }

            if (lightgreenz != "")
            {
                if (TimeSpan.Parse(lightgreenz) > timeFrom && TimeSpan.Parse(lightgreenz) < timeTo)
                {
                    lgreenTZ.Text = TimeSpan.Parse(lightgreenz).Add(timeAdd).ToString(@"hh\:mm") + ReturnMinutes(tParams[1]);
                }
                else
                {
                    lgreenTZ.Text = lightgreenz + ReturnMinutes(tParams[1]);
                }
            }
            else
            {
                lgreenTZ.Text = lightgreenz;
            }

            if (greenz != "")
            {
                if (TimeSpan.Parse(greenz) > timeFrom && TimeSpan.Parse(greenz) < timeTo)
                {
                    greenTZ.Text = TimeSpan.Parse(greenz).Add(timeAdd).ToString(@"hh\:mm") + ReturnMinutes(tParams[2]);
                }
                else
                {
                    greenTZ.Text = greenz + ReturnMinutes(tParams[2]);
                }
            }
            else
            {
                greenTZ.Text = greenz;
            }

            if (yellowz != "")
            {
                if (TimeSpan.Parse(yellowz) > timeFrom && TimeSpan.Parse(yellowz) < timeTo)
                {
                    yellowTZ.Text = TimeSpan.Parse(yellowz).Add(timeAdd).ToString(@"hh\:mm") + ReturnMinutes(tParams[3]);
                }
                else
                {
                    yellowTZ.Text = yellowz + ReturnMinutes(tParams[3]);
                }
            }
            else
            {
                yellowTZ.Text = yellowz;
            }

            if (orangez != "")
            {
                if (TimeSpan.Parse(orangez) > timeFrom && TimeSpan.Parse(orangez) < timeTo)
                {
                    orangeTZ.Text = TimeSpan.Parse(orangez).Add(timeAdd).ToString(@"hh\:mm") + ReturnMinutes(tParams[4]);
                }
                else
                {
                    orangeTZ.Text = orangez + ReturnMinutes(tParams[4]);
                }
            }
            else
            {
                orangeTZ.Text = orangez;
            }

            if (redz != "")
            {
                if (TimeSpan.Parse(redz) > timeFrom && TimeSpan.Parse(redz) < timeTo)
                {
                    redTZ.Text = TimeSpan.Parse(redz).Add(timeAdd).ToString(@"hh\:mm") + ReturnMinutes(tParams[5]);
                }
                else
                {
                    redTZ.Text = redz + ReturnMinutes(tParams[5]);
                }
            }
            else
            {
                redTZ.Text = redz;
            }

            if (grayz != "")
            {
                if (TimeSpan.Parse(grayz) > timeFrom && TimeSpan.Parse(grayz) < timeTo)
                {
                    grayTZ.Text = TimeSpan.Parse(grayz).Add(timeAdd).ToString(@"hh\:mm") + ReturnMinutes(tParams[6]);
                }
                else
                {
                    grayTZ.Text = grayz + ReturnMinutes(tParams[6]);
                }
            }
            else
            {
                grayTZ.Text = grayz;
            }


            /*blueTZ.Text = 
            lgreenTZ.Text = 
            //lgreenTZ.Text = "";
            greenTZ.Text = 
            yellowTZ.Text = 
            orangeTZ.Text = 
            redTZ.Text = 
            grayTZ.Text =*/ 




            tawayTZ.Text = CalculateFinal.CalcZoneTATime(TZ, tParams[8], tParams[9], zname).Result;

            foreach (string dish in ListDish)
            {
                dishStops.Items.Add(dish);
            }
            foreach (string time in ListTimes)
            {
                timeStops.Items.Add(time);
            }
            foreach (string offer in ListOffers)
            {
                richTextBox1.AppendText(offer);
                richTextBox1.AppendText("\n");
                offersList.Items.Add(offer);
                bool isInt = Int32.TryParse(offer.Substring(0, 1), out int r);
                if (isInt == true)
                {
                    offersList.SetSelected(offersList.FindString(offer), true);
                }
            }

        }

        

        public Form1()
        {
            
            InitializeComponent();

            panel2.Width = 12;
            this.Width = 156;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height / 2 - this.Height / 2);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            
            //Определение номера оператора и Опрос АТС о стаусе оператора
            label5.Text=OperNumberLoad();
            
            Res.getAtcStatus(label5.Text);
            operStatus.Text = Res.atsstatus;
            //цвет в соответствии
            operStatus.ForeColor = Res.stringCol;
            //label3.Text = DateTime.Now.AddMinutes(70).ToShortTimeString();
            //label4.Text = DateTime.Now.AddMinutes(-DateTime.Now.Minute).ToShortTimeString();
            timer3.Start();
        }

        private void Плаза_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("PlightBlue"), ReadS("PlightGreen"), ReadS("PGreen"), ReadS("PYellow"), ReadS("POrange"), ReadS("PRed"), ReadS("PGray"), ReadS("nn"), ReadS("PTaway"), ReadS("ta_nn") };
            string oname = "Перцы";
            GetAllInfo(sender, tParams, oname);
        }

        private void Армада_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("PlightBlue"), ReadS("PlightGreen"), ReadS("PGreen"), ReadS("PYellow"), ReadS("POrange"), ReadS("PRed"), ReadS("PGray"), ReadS("nn"), ReadS("PTaway"), ReadS("ta_nn") };
            string oname = "Перцы";
            GetAllInfo(sender, tParams, oname);
        }

        private void Маерчака_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("PlightBlue"), ReadS("PlightGreen"), ReadS("PGreen"), ReadS("PYellow"), ReadS("POrange"), ReadS("PRed"), ReadS("PGray"), ReadS("nn"), ReadS("PTaway"), ReadS("ta_nn") };
            string oname = "Перцы";
            GetAllInfo(sender, tParams, oname);
        }

        private void Пушкина_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("PlightBlue"), ReadS("PlightGreen"), ReadS("PGreen"), ReadS("PYellow"), ReadS("POrange"), ReadS("PRed"), ReadS("PGray"), ReadS("nn"), ReadS("PTaway"), ReadS("ta_nn") };
            string oname = "Перцы";
            GetAllInfo(sender, tParams, oname);
        }

        private void Мира_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("PlightBlue"), ReadS("PlightGreen"), ReadS("PGreen"), ReadS("PYellow"), ReadS("POrange"), ReadS("PRed"), ReadS("PGray"), ReadS("nn"), ReadS("PTaway"), ReadS("ta_nn") };
            string oname = "Перцы";
            GetAllInfo(sender, tParams, oname);
        }

        private void Гладкова_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("PlightBlue"), ReadS("PlightGreen"), ReadS("PGreen"), ReadS("PYellow"), ReadS("POrange"), ReadS("PRed"), ReadS("PGray"), ReadS("nn"), ReadS("PTaway"), ReadS("ta_nn") };
            string oname = "Перцы";
            GetAllInfo(sender, tParams, oname);
        }

        private void Правый_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("PlightBlue"), ReadS("PlightGreen"), ReadS("PGreen"), ReadS("PYellow"), ReadS("POrange"), ReadS("PRed"), ReadS("PGray"), ReadS("nn"), ReadS("PTaway"), ReadS("ta_nn") };
            string oname = "Перцы";
            GetAllInfo(sender, tParams, oname);
        }

        private void СиБ_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("RlightBlue"), ReadS("RlightGreen"), ReadS("RGreen"), ReadS("RYellow"), ReadS("ROrange"), ReadS("RRed"), ReadS("RGray"), ReadS("nn"), ReadS("SibTaway"), ReadS("ta_nn") };
            string oname = ((Button)sender).Name;
            GetAllInfo(sender, tParams, oname);
        }

        private void БиБ_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("RlightBlue"), ReadS("RlightGreen"), ReadS("RGreen"), ReadS("RYellow"), ReadS("ROrange"), ReadS("RRed"), ReadS("RGray"), ReadS("nn"), ReadS("BibTaway"), ReadS("ta_nn") };
            string oname = ((Button)sender).Name;
            GetAllInfo(sender, tParams, oname);
        }

        private void Крем_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("RlightBlue"), ReadS("RlightGreen"), ReadS("RGreen"), ReadS("RYellow"), ReadS("ROrange"), ReadS("RRed"), ReadS("RGray"), ReadS("nn"), ReadS("KremTaway"), ReadS("ta_nn") };
            string oname = ((Button)sender).Name;
            GetAllInfo(sender, tParams, oname);
        }

        private void Формаджи_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("RlightBlue"), ReadS("RlightGreen"), ReadS("RGreen"), ReadS("RYellow"), ReadS("ROrange"), ReadS("RRed"), ReadS("RGray"), ReadS("nn"), ReadS("FormTaway"), ReadS("ta_nn") };
            string oname = ((Button)sender).Name;
            GetAllInfo(sender, tParams, oname);
        }

        private void Якитория_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("YlightBlue"), ReadS("YlightGreen"), ReadS("YGreen"), ReadS("YYellow"), ReadS("YOrange"), ReadS("YRed"), ReadS("YGray"), ReadS("nn"), ReadS("YTaway"), ReadS("ta_nn") };
            string oname = ((Button)sender).Name;
            GetAllInfo(sender, tParams, oname);
        }

        private void Коко_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("YlightBlue"), ReadS("YlightGreen"), ReadS("YGreen"), ReadS("YYellow"), ReadS("YOrange"), ReadS("YRed"), ReadS("YGray"), ReadS("nn"), ReadS("YTaway"), ReadS("ta_nn") };
            string oname = ((Button)sender).Name;
            GetAllInfo(sender, tParams, oname);
        }

        private void Ромбаба_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("RBlightBlue"), ReadS("RBlightGreen"), ReadS("RBGreen"), ReadS("RBYellow"), ReadS("RBOrange"), ReadS("RBRed"), ReadS("RBGray"), ReadS("nn"), ReadS("RBTaway"), ReadS("ta_nn") };
            string oname = ((Button)sender).Name;
            GetAllInfo(sender, tParams, oname);
        }

        private void Мамас_Click(object sender, EventArgs e)
        {
            string[] tParams = new string[] { ReadS("MlightBlue"), ReadS("MlightGreen"), ReadS("MGreen"), ReadS("MYellow"), ReadS("MOrange"), ReadS("MRed"), ReadS("MGray"), ReadS("nn"), ReadS("MamasTaway"), ReadS("ta_nn") };
            string oname = ((Button)sender).Name;
            GetAllInfo(sender, tParams, oname);
        }

        private void dishStops_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((ListBox)sender).SelectedIndex = -1;
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            timer3.Stop();
            AtsRequest AtsReq = new AtsRequest();
            int res = AtsReq.AtsPutUnpauseRequest(label5.Text, "d2lkZ2V0cGFzc3dvcmQ");
            if(res==1)
            {
                operStatus.ForeColor = Color.Green;
                operStatus.Text = "Cнят с паузы!!";
            }
            else
            {
                operStatus.ForeColor = Color.Red;
                operStatus.Text = "Неудача X_X";
            }
            timer3.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Res.getAtcStatus(label5.Text);
            operStatus.Text = Res.atsstatus;
            //цвет в соответствии
            operStatus.ForeColor = Res.stringCol;
            if ((flag!=Res.flag) && (flag ==0))
            {
                flag = Res.flag;
                playSimpleSound();
            }
            else
            {
                flag = Res.flag;
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            curTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer3.Stop();
            AtsRequest AtsReq = new AtsRequest();
            int res = AtsReq.AtsPutPauseRequest(label5.Text, "d2lkZ2V0cGFzc3dvcmQ");
            if (res == 1)
            {
                operStatus.ForeColor = Color.Green;
                operStatus.Text = "Встал на паузу!";
            }
            else
            {
                operStatus.ForeColor = Color.Red;
                operStatus.Text = "Неудача X_X";
            }
            timer3.Start();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            if (label12.Text=="Скрыть акции")
            {
                richTextBox1.Visible = false;
                label12.Text = "Показать акции";

            }
            else if (label12.Text== "Показать акции")
            {
                richTextBox1.Visible = true;
                label12.Text = "Скрыть акции";
            }

               
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            timeStops.Items.Clear();
            List<string> List = new List<string>();
            List=DBselect.CalculateTimes(connstr, label11.Text, selectTimes + label11.Text + "' and DateofStop='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' order by Time").Result;
            foreach(string s in List)
            {
                timeStops.Items.Add(s);
            }
        }

        

        

        public void TimeWindowResize()
        {
            timer1.Enabled = true;
            timer1.Start();
            offersList.HorizontalScrollbar = true;
            timer5.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)//Ресайз панели со временем
        {
            if(panel2_statusFlag==0)
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = true;
                Point P = this.Location;
                while (panel2.Width != 132)
                {
                    panel2.Width = panel2.Width +12;
                    P = P+ new Size(-38,0);
                    this.Location = (P);
                    this.Width = this.Width + 38;
                }
                panel2_statusFlag = 1;
                //this.Opacity = 65;
            }
            else if(panel2_statusFlag == 1)
            {
                Point P = this.Location;
                while (panel2.Width != 12)
                {
                    panel2.Width = panel2.Width -12;
                    P = P + new Size(38, 0);
                    this.Location = (P);
                    this.Width = this.Width - 38;
                }
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                panel2_statusFlag = 0;
                //this.Opacity = 15;
                timer5.Stop();
            }
            timer1.Stop();
            timer1.Enabled = false;
            
        }

       

        private void button16_ClickAsync(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Text = DateTime.Now.ToShortDateString();
            button16.Visible = true;
            TimeWindowResize();
            offersList.Items.Clear();
            dishStops.Items.Clear();
            timeStops.Items.Clear();
            richTextBox1.Clear();
        }

        private void timer2_Tick(object sender, EventArgs e)//Ресайз формы
        {
            Point P = this.Location;
            if (form_statusFlag == 0)
            {
                int x = this.Location.X - 132;
                button16.BackgroundImage = TimeShiftApp.Properties.Resources.reverse_arrows;
                panel1.Visible = true;
                panel4.Visible = true;
                while (this.Location.X !=x)
                {
                    //this.Width = this.Width + 30;
                    P = P + new Size(-12, 0);
                    this.Location = (P);
                }
                form_statusFlag = 1;
                timer2.Stop();
                //this.Text = "TimeShiftApp";
                button16.BackgroundImage = TimeShiftApp.Properties.Resources.reverse_arrows;
            }
            else if (form_statusFlag == 1)
            {
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height / 2 - this.Height / 2);
                P = this.Location;
                //this.Text = "";
                int x = this.Location.X + 132;

                while (this.Location.X!= x)
                {
                    
                    P = P + new Size(12, 0);
                    this.Location = (P);
                    //this.Width = this.Width - 30;
                }
                button16.BackgroundImage = TimeShiftApp.Properties.Resources.arrows;
                panel1.Visible = false;
                panel4.Visible = false;
                form_statusFlag = 0;
                timer2.Stop();
            }
        }
    }

   
}
