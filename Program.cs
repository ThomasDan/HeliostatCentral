using HeliostatCentral.Handlers;

namespace HeliostatCentral
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SerialPortHandler sph = new SerialPortHandler();
            sph.Run();
        }
    }
}
