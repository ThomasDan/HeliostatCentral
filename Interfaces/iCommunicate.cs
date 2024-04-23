using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Interfaces
{
    public interface iCommunicate
    {
        public void Initialize();
        public void InitializeReceiver();
        public List<string> GetMessages();
        public void SendCommunication(string message);
    }
}
