using HeliostatCentral.DAL;
using HeliostatCentral.Handlers;
using HeliostatCentral.Interfaces;
using HeliostatCentral.Models;

namespace HeliostatCentral
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextBasedDAL dal = new TextBasedDAL();

            SerialPortReceiveHandler tracker = new SerialPortReceiveHandler("Sun Tracker");
            List<iSendCommunication> solarPanels = new List<iSendCommunication>() { new SerialPortSendHandler("Solar Panel #1") };
            CommaSeparationConvertionHandler csc = new CommaSeparationConvertionHandler();

            HeliostatCentralLogic logic = new HeliostatCentralLogic(csc, tracker, solarPanels, dal);
            logic.Initialize();
            /*

            List<HeliostatRecording> hrs = GenerateData(new TimeSpan(12, 0, 0), new TimeSpan(16, 0, 0));
            
            hrs = new List<HeliostatRecording>()
                {
                    new HeliostatRecording(100, 150, 370, DateTime.Now, true),
                    new HeliostatRecording(50, 100, 370, DateTime.Now.AddSeconds(5), true),
                    new HeliostatRecording(70, 130, 370, DateTime.Now.AddSeconds(10), true),
                    new HeliostatRecording(130, 140, 370, DateTime.Now.AddSeconds(15), true),
                };
            dal.SaveRecording(hrs);
            */            
        }

        /*
            Min Vertikal Højde : 175 (Solopgang/Solnedgang)
            Max Vertikal Højde : 125 (Middag/Høj Sol)

            Min Horisontal : 150 (Øst)
            Max Horisontal : 30 (Vest)
        */
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
                DateTime hrdt = new DateTime(dt.Year, dt.Month, dt.Day-3, currTS.Hours, currTS.Minutes, currTS.Seconds);
                
                currTimeProportion = (((int)currTS.TotalSeconds - (int)sunDown.TotalSeconds) / 5) * -1;

                if (currTimeProportion == 0)
                {
                    currTimeProportion = 1;
                }

                // Then we generate a proportional horizontal value
                hori = 30 + (int)(((decimal)120 / (decimal)baseTimeProportion) * currTimeProportion);


                // Then we generate a proportional vertical value
                if (currTS.Hours < 12)
                {
                    vert = 175 - (int)( 
                        ((decimal)50 / ((decimal)baseTimeProportion / 2)) * (currTimeProportion - ((decimal)baseTimeProportion / 2))
                        );
                } else
                {
                    vert = 175 - (50 - (int)(((decimal)50 / (decimal)baseTimeProportion) * (decimal)currTimeProportion) * 2);
                }


                // Then we generate a light level
                light = 150;

                HeliostatRecording hr = new HeliostatRecording(hori, vert, light, hrdt, true);
                Console.WriteLine(hr.ToString());
                hrs.Add(hr);
            }

            return hrs;
        }
    }
}
