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

        public TextBasedDAL()
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

            if (existingRecords.Count > 0) 
            {
                string joinedExistingRecords = String.Join('\n', existingRecords);
                sw.WriteLine(joinedExistingRecords);
            }
            
            // The (probable) use of WriteLine above, ensures we are already on a new line, and therefore do not want to use WriteLine again,
            //      else we make a new line after this line, which will cause issues as it tries to load the empty line ("") as heliostat data.
            sw.Write(
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
