using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LogParser.data
{
    class Logdata
    {
        public string Host { get; set; }
        public string Lid { get; set; }
        public string Uid { get; set; }
        public string Time { get; set; }
        public string Type_request { get; set; }
        public string Request { get; set; }
        public string Proto { get; set; }
        public string Status { get; set; }
        public string Size { get; set; }
        public string Referer { get; set; }
        public string Agent { get; set; }

        public static void FromFileToBase(string filename)
        {
            try
            {
                int count = File.ReadAllLines(filename).Length;
                MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost;Database=logparse;Uid=logparse;Pwd=Password%123;");
                string[] strings = File.ReadAllLines(filename);
                string pattern = @"(\S+) (\S+) (\S+) \[([\w:/]+\s[+\-]\d{4})\] \""(\S+) (\S+)\s*(\S+)\s*\"" (\d{3}) (\S+) \""(\S+)\s*\"" \""(.*)\""";
                mySqlConnection.Open();
                for (int i = 0; i < count; i++)
                {
                    if (strings[i] != null && strings[i].IndexOf("]") > 0)
                    {
                        string Line = strings[i];
                        //string[] data = Line.Split(';');
                        Match match = Regex.Match(Line, pattern);
                        string CommandText = $"INSERT INTO `2022` (Host,Lid,Uid,Time,Type_request,Request,Proto,Status,Size,Referer,Agent) VALUES " +
                            $"('{match.Groups[1].Value}','{match.Groups[2].Value}','{match.Groups[3].Value}'," +
                            $"'{match.Groups[4].Value}','{match.Groups[5].Value}','{match.Groups[6].Value}'," +
                            $"'{match.Groups[7].Value}','{match.Groups[8].Value}','{match.Groups[9].Value}'," +
                            $"'{match.Groups[10].Value}','{match.Groups[11].Value}');";
                        MySqlCommand command = new MySqlCommand(CommandText, mySqlConnection);
                        command.ExecuteNonQuery();
                    };
                }
                mySqlConnection.Close();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Файл не существует");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        public static List<Logdata> GetDatafromDB()
        {
            {
                MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost;Database=logparse;Uid=logparse;Pwd=Password%123;");
                
                //MySqlDataReader data = null;
                string CommandText = "SELECT `2022`.`host`,`2022`.`lid`,`2022`.`uid`,`2022`.`time`,`2022`.`type_request`,`2022`.`request`,`2022`.`proto`,`2022`.`status`,`2022`.`size`,`2022`.`referer`,`2022`.`agent` FROM `logparse`.`2022`;";
                MySqlCommand command = new MySqlCommand(CommandText, mySqlConnection);
                try
                {
                    mySqlConnection.Open();
                    using (MySqlDataReader data = command.ExecuteReader())
                    {
                        List<Logdata> LogDataList = new List<Logdata>();
                        while (data.Read())
                        {
                            LogDataList.Add(new Logdata()
                            {
                                Host = data.GetString(0),
                                Lid = data.GetString(1),
                                Uid = data.GetString(2),
                                Time = data.GetString(3),
                                Type_request = data.GetString(4),
                                Request = data.GetString(5),
                                Proto = data.GetString(6),
                                Status = data.GetString(7),
                                Size = data.GetString(8),
                                Referer = data.GetString(9),
                                Agent = data.GetString(10),
                            });
                        }
                        int count = LogDataList.Count;
                        return LogDataList;
                        
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                    return null;
                }
            }
        }
    }
}
