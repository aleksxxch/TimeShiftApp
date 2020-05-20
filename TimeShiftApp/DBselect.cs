using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TimeShiftApp
{
    class DBselect
    {
        public static async Task<double> CalculateDelay(string connstr, string zname,string sql_expr)
        {
            MySqlConnection connection = new MySqlConnection(connstr);
            double x=9; //9- код возврата обозначающий что выборка из базы не удалась
                await connection.OpenAsync();
                MySqlCommand command = new MySqlCommand(sql_expr, connection);
                var reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                       x=Convert.ToDouble(reader["Delay"].ToString().TrimStart('+'));
                    }
                
                    reader.Close();
                await connection.CloseAsync();
            }
                else
                {
                    reader.Close();
                    x = 0;
                await connection.CloseAsync();
            }
            return x;
        }

        public static async Task<List<string>> CalculateTimes(string connstr,string zname,string sql_expr)
        {
            List<string> timesList = new List<string>();
            
            MySqlConnection connection = new MySqlConnection(connstr);
            await connection.OpenAsync();
            MySqlCommand command = new MySqlCommand(sql_expr, connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while(await reader.ReadAsync())
                {
                    timesList.Add(reader["Time"].ToString());
                }
                reader.Close();
                await connection.CloseAsync();
            }
            else
            {
                reader.Close();
                await connection.CloseAsync();
            }
            return timesList;
        }

        public static async Task<List<string>> SelectDishStops(string connstr, string zname,string sql_expr)
        {
            List<string> dishList = new List<string>();

            MySqlConnection connection = new MySqlConnection(connstr);
            await connection.OpenAsync();
            MySqlCommand command = new MySqlCommand(sql_expr, connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    dishList.Add("- "+reader["Dish"].ToString());
                }
                reader.Close();
                await connection.CloseAsync();
            }
            else
            {
                reader.Close();
                await connection.CloseAsync();
            }
            return dishList;
        }
        public static async Task<List<string>> SelectOffers(string connstr, string zname, string sql_expr)
        {
            List<string> offersList = new List<string>();
            int n = 1;
            MySqlConnection connection = new MySqlConnection(connstr);
            await connection.OpenAsync();
            MySqlCommand command = new MySqlCommand(sql_expr, connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    offersList.Add(n.ToString()+".  " + reader["Name"].ToString());
                    offersList.Add(" - "+ reader["Description"].ToString());
                    n = n + 1;
                }
                reader.Close();
                await connection.CloseAsync();
            }
            else
            {
                reader.Close();
                await connection.CloseAsync();
            }
            return offersList;

        }
    }

}
