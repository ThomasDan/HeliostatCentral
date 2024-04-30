using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTrackerCentral.Interfaces
{
    public interface iSendCommunication
    {
        public void Initialize();
        public void SendCommunication(string message);
    }
}
