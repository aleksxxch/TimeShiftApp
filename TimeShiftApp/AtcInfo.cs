using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net;
using System.Net.Http;


namespace TimeShiftApp
{
    public class AtcInfo
    {
        public string atsstatus;
        public Color stringCol;
        public int flag;

        

        public void getAtcStatus(string opernum)
        {
            WebClient wcl = new WebClient();
            string status;
            try
            {
                string getstring = wcl.DownloadString("http://192.168.10.5:8070/widget/operator/status/" + opernum);
                dynamic dobj = JsonConvert.DeserializeObject<dynamic>(getstring);
                status = dobj["status"].ToString();
            }
            catch(Exception ex)
            {
                status = "INVALID";
            }
            switch (status)
            {
                case "UNKNOWN":
                    this.atsstatus = "НЕИЗВЕСТНО";
                    this.stringCol = Color.Plum;
                    this.flag = 0;
                    break; 
                    
                case "NOT_INUSE":
                    this.atsstatus = "СВОБОДЕН";
                    this.stringCol = Color.Green;
                    this.flag = 0;
                    break;
                case "INUSE":
                    this.atsstatus = "РАЗГОВАРИВАЕТ";
                    this.stringCol = Color.Goldenrod;
                    this.flag = 1;
                    break;
                    
                case "BUSY":
                    this.atsstatus = "ЗАНЯТ";
                    this.stringCol = Color.Orange;
                    this.flag = 0;
                    break;
                    
                case "INVALID":
                    this.atsstatus = "ОШИБКА";
                    this.stringCol = Color.OrangeRed;
                    this.flag = 0;
                    break;
                    
                case "UNAVAILABLE":
                    this.atsstatus = "НЕДОСТУПЕН";
                    this.stringCol = Color.White;
                    this.flag = 0;
                    break;
                    
                case "RINGING":
                    this.atsstatus = "ВЫЗЫВАЕТСЯ";
                    this.stringCol = Color.BlueViolet;
                    this.flag = 0;
                    break;
                    
                case "RINGINUSE":
                    this.atsstatus = "ВЫЗЫВАЕТСЯ";
                    this.stringCol = Color.BlueViolet;
                    this.flag = 0;
                    break;
                case "ONHOLD":
                    this.atsstatus = "НА УДЕРЖАНИИ";
                    this.stringCol = Color.Yellow;
                    this.flag = 0;
                    break;
                case "ONPAUSE":
                    this.atsstatus = "НА ПАУЗЕ";
                    this.stringCol = Color.LightYellow;
                    this.flag = 0;
                    break;
                case "NOT_IN_QUEUE":
                    this.atsstatus = "НЕ В ОЧЕРЕДИ";
                    this.stringCol = Color.Peru;
                    this.flag = 0;
                    break;
                
            }


        }
    }

    public class AtsRequest
    {
        public string Operator { get; set; }
        public string token { get; set; }

        public int AtsPutUnpauseRequest(string opernum, string token)
        {
            HttpClient hClient = new HttpClient();
            AtsRequest putreq = new AtsRequest
            {
                Operator = opernum,
                token = token
            };


            //String json = JsonConvert.SerializeObject(putreq, Formatting.Indented);
            hClient.BaseAddress = new Uri("http://192.168.10.5:8070/");
            var response = hClient.PutAsJsonAsync("widget/operator/unpause", putreq).Result;
            //bool returnValue2 = response.Content.ReadAsAsync<bool>().Result;
            if (response.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int AtsPutPauseRequest(string opernum, string token)
        {
            HttpClient hClient = new HttpClient();
            AtsRequest putreq = new AtsRequest
            {
                Operator = opernum,
                token = token
            };


            //String json = JsonConvert.SerializeObject(putreq, Formatting.Indented);
            hClient.BaseAddress = new Uri("http://192.168.10.5:8070/");
            var response = hClient.PutAsJsonAsync("widget/operator/pause", putreq).Result;
            //bool returnValue2 = response.Content.ReadAsAsync<bool>().Result;
            if (response.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    
}
