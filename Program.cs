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
            SerialPortHandler sp = new SerialPortHandler();
            CommaSeparationConvertionHandler csc = new CommaSeparationConvertionHandler();

            HeliostatCentralLogic logic = new HeliostatCentralLogic(csc, sp, dal);
            logic.Initialize();
        }
    }
}
