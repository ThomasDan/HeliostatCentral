using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTrackerCentral.Interfaces
{
    public interface iReceiveCommunication
    {
        public delegate Task SaveReceivedMessage(string message);

        public void Initialize(SaveReceivedMessage _saveReceivedMessage);
        //public Task SaveReceivedMessage();
        //public List<string> GetMessages();
    }
}
