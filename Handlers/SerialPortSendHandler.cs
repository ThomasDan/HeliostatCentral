using HeliostatCentral.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Xml.Linq;

namespace HeliostatCentral.Handlers
{
    class SerialPortSendHandler : SerialPortHandlerBase, iSendCommunication
    {
        public SerialPortSendHandler(string _name) : base(_name) { }

        public void SendCommunication(string message)
        {
            try
            {
                serialPort.WriteLine(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
