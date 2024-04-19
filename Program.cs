using HeliostatCentral.DAL;
using HeliostatCentral.Handlers;
using HeliostatCentral.Models;

namespace HeliostatCentral
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextBasedDAL dal = new TextBasedDAL();
            //SerialPortHandler sp = new SerialPortHandler();
            //CommaSeparationConvertionHandler csc = new CommaSeparationConvertionHandler();

            //HeliostatCentralLogic logic = new HeliostatCentralLogic(csc, sp, dal);
            //logic.Initialize();

            List<HeliostatRecording> hrs = GenerateData(new TimeSpan(8, 0, 0), new TimeSpan(16, 0, 0));

        }

        static List<HeliostatRecording> GenerateData(TimeSpan sunUp, TimeSpan sunDown)
        {
            List<HeliostatRecording> hrs = new List<HeliostatRecording>();
            TimeSpan currTS = new TimeSpan(sunUp.Ticks);
            DateTime dt = DateTime.Now;

            int baseTimeProportion = ((int)sunDown.TotalSeconds - (int)sunUp.TotalSeconds) / 5;
            int currTimeProportion;
            int hori;
            int vert;
            int light;


            for (int i = 0; currTS < sunDown; i++)
            {
                // Here we generate the timestamp for the recording
                currTS = currTS.Add(new TimeSpan(0, 0, 5));
                DateTime hrdt = new DateTime(dt.Year, dt.Month, dt.Day-1, currTS.Hours, currTS.Minutes, currTS.Seconds);
                
                currTimeProportion = (((int)currTS.TotalSeconds - (int)sunDown.TotalSeconds) / 5) * -1;

                if (currTimeProportion == 0)
                {
                    currTimeProportion = 1;
                }

                Console.WriteLine(hrdt.ToString("HH:mm:ss") + " - " + currTimeProportion);

                // Then we generate a proportional horizontal value
                hori = 30 + (int)(((decimal)120 / (decimal)baseTimeProportion) * currTimeProportion);


                // Then we generate a proportional vertical value
                if (currTS.Hours < 12)
                {
                    vert = 175 + (int)(((decimal)50 / (decimal)baseTimeProportion) * currTimeProportion);
                } else
                {
                    vert = 175 + (50 - (int)(((decimal)50 / (decimal)baseTimeProportion) * currTimeProportion)) * 2;
                }


                // Then we generate a light level
                light = 250;

                HeliostatRecording hr = new HeliostatRecording(hori, vert, light, hrdt, true);
                Console.WriteLine(hr.ToString());
                hrs.Add(hr);

            }


            return hrs;
        }
        /*
            Max Vertikal Højde : 125 (Middag/Høj Sol)
            Min Vertikal Højde : 175 (Solopgang/Solnedgang)

            Max Horisontal : 30 (Vest)
            Min Horisontal : 150 (Øst)
        */
    }
}
