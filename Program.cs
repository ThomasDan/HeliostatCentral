using SunTrackerCentral.DAL;
using SunTrackerCentral.Handlers;
using SunTrackerCentral.Interfaces;
using SunTrackerCentral.Models;

namespace SunTrackerCentral
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextBasedDAL dal = new TextBasedDAL();
            SerialPortReceiveHandler tracker = new SerialPortReceiveHandler("Sun Tracker");
            List<iSendCommunication> solarPanels = new List<iSendCommunication>() { new SerialPortSendHandler("Solar Panel #1") };
            CommaSeparationConvertionHandler csc = new CommaSeparationConvertionHandler();

            SunTrackerCentralLogic logic = new SunTrackerCentralLogic(csc, tracker, solarPanels, dal);

            /*

            dal.SaveRecordings(hrs);


            //List<SunTrackerRecording> hrs = GenerateData(new TimeSpan(4, 0, 0), new TimeSpan(21, 0, 0));

            /*
            hrs = new List<SunTrackerRecording>()
                {
                    new SunTrackerRecording(100, 150, 370, DateTime.Now, true),
                    new SunTrackerRecording(50, 100, 370, DateTime.Now.AddSeconds(5), true),
                    new SunTrackerRecording(70, 130, 370, DateTime.Now.AddSeconds(10), true),
                    new SunTrackerRecording(130, 140, 370, DateTime.Now.AddSeconds(15), true),
                };
            */
        }

        /*
            Min Vertikal Højde : 180 (Solopgang/Solnedgang)
            Max Vertikal Højde : 0 (Middag/Høj Sol)

            Min Horisontal : 180 (Øst)
            Max Horisontal : 0 (Vest)
        */
        static List<SunTrackerRecording> GenerateData(TimeSpan sunUp, TimeSpan sunDown)
        {
            List<SunTrackerRecording> strs = new List<SunTrackerRecording>();
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
                DateTime strdt = new DateTime(dt.Year, dt.Month, dt.Day, currTS.Hours, currTS.Minutes, currTS.Seconds);
                
                currTimeProportion = (((int)currTS.TotalSeconds - (int)sunDown.TotalSeconds) / 5) * -1;

                if (currTimeProportion == 0)
                {
                    currTimeProportion = 1;
                }

                // Then we generate a proportional horizontal value
                hori = (int)(((decimal)180 / (decimal)baseTimeProportion) * currTimeProportion);


                // Then we generate a proportional vertical value
                if (currTS.Hours < 12)
                {
                    vert = (int)(((decimal)180 / (decimal)baseTimeProportion) * currTimeProportion * 2);
                } else
                {
                    vert = 180 - ((int)(((decimal)baseTimeProportion) * (decimal)currTimeProportion) * 2);
                }


                // Then we generate a light level
                light = 800;

                SunTrackerRecording str = new SunTrackerRecording(hori, vert, light, strdt, true);
                //Console.WriteLine(str.ToString());
                strs.Add(str);
            }

            return strs;
        }
    }
}
