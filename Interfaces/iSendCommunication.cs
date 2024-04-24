using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Interfaces
{
    public interface iSendCommunication
    {
        public void Initialize();
        public void SendCommunication(string message);
    }
}
