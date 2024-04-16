using HeliostatCentral.Interfaces;
using HeliostatCentral.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.DAL
{
    internal class TextBasedDAL : iHeliostatDataAccessLayer
    {
        string relativePath;

        TextBasedDAL()
        {
            this.relativePath = AppDomain.CurrentDomain.BaseDirectory + "recordings/record.txt";
        }

        private List<string> ReadAllLines()
        {
            StreamReader sr = new StreamReader(relativePath);
            List<string> lines = new List<string>();
            string? line = sr.ReadLine();

            while (line != null)
            {
                lines.Add(line.ToString());
                line = sr.ReadLine();
            }
            sr.Close();

            return lines;
        }

        public List<HeliostatRecording> LoadRecordings()
        {
            List<HeliostatRecording> hrs = new List<HeliostatRecording>();

            List<string> lines = ReadAllLines();

            foreach (string line in lines)
            {
                HeliostatRecording hr = ConvertDataToHeliostat(line);
                hrs.Add(hr);
            }

            return hrs;
        }

        private HeliostatRecording ConvertDataToHeliostat(string data)
        {
            string[] dataSeparated = data.Split(',');

            // Converting 3x int, Converting DateTime
            int hori = Convert.ToInt32(dataSeparated[0]);
            int vert = Convert.ToInt32(dataSeparated[1]);
            int light = Convert.ToInt32(dataSeparated[2]);
            DateTime stamp = Convert.ToDateTime(dataSeparated[3]);

            HeliostatRecording hr = new HeliostatRecording(hori, vert, light, stamp);

            return hr;
        }

        public void SaveRecording(HeliostatRecording hr)
        {
            List<string> existingRecords = ReadAllLines();

            StreamWriter sw = new StreamWriter(relativePath);

            sw.WriteLine(String.Join('\n', existingRecords));
            sw.WriteLine(
                hr.HorizontalDegrees.ToString() + "," + 
                hr.VerticalDegrees.ToString() + "," + 
                hr.LightLevel.ToString() + "," + 
                hr.DateTimeStamp.ToString()
                );

            sw.WriteLine();
            sw.Close();
        }
    }
}
