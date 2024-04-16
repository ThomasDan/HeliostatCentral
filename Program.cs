using HeliostatCentral.DAL;
using HeliostatCentral.Handlers;
using HeliostatCentral.Models;

namespace HeliostatCentral
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //SerialPortHandler sph = new SerialPortHandler();
            //sph.Run();

            TextBasedDAL dal = new TextBasedDAL();

            HeliostatRecording hr = new HeliostatRecording(10, 10, 10, DateTime.Now);
            dal.SaveRecording(hr);
            HeliostatRecording hr1 = new HeliostatRecording(20, 10, 100, DateTime.Now);
            dal.SaveRecording(hr1);
            HeliostatRecording hr2 = new HeliostatRecording(10, 30, 300, DateTime.Now);
            dal.SaveRecording(hr2);
            HeliostatRecording hr3 = new HeliostatRecording(10, 150, 40, DateTime.Now);
            dal.SaveRecording(hr3);


            List<HeliostatRecording> hrs = dal.LoadRecordings();

            foreach (HeliostatRecording record in hrs)
            {
                Console.WriteLine(record.ToString());
            }
        }
    }
}
