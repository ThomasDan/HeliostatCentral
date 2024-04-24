using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Interfaces
{
    public interface iReceiveCommunication
    {
        public void Initialize();
        public List<string> GetMessages();
    }
}
