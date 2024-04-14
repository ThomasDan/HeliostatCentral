using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Interfaces
{
    internal interface iCommunicateWithHeliostat
    {
        internal void Run();
        internal List<string> GetMessages();
        internal void SendCommunication(string message);
    }
}
