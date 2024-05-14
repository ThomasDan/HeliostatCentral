using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunTrackerCentral.Delegates;

namespace SunTrackerCentral.Interfaces
{
    public interface iReceiveCommunication
    {
        public void Initialize(SaveReceivedMessageDelegate _saveReceivedMessage);
        //public List<string> GetMessages();
    }
}
