using SunTrackerCentral.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SunTrackerCentral.Handlers
{
    public class SerialPortReceiveHandler : SerialPortHandlerBase, iReceiveCommunication
    {
        List<string> receivedMessages;
        object receivedMessagesLock;

        public SerialPortReceiveHandler(string _name) : base(_name)
        {
            receivedMessages = new List<string>();
            receivedMessagesLock = new object();
        }

        public override void Initialize()
        {
            base.Initialize();
            Thread receiver = new Thread(ReceiveCommunication);
            receiver.Start();
        }

        /// <summary>
        /// Infinite loop that receives any messages waiting in the serialPort 20 times per second
        /// </summary>
        private void ReceiveCommunication()
        {
            string newMessage = "";
            while (true)
            {
                try
                {
                    if (Monitor.TryEnter(receivedMessagesLock))
                    {
                        newMessage = base.serialPort.ReadLine();
                        try
                        {
                            receivedMessages.Add(newMessage);
                        }
                        finally
                        {
                            Monitor.PulseAll(receivedMessagesLock);
                            Monitor.Exit(receivedMessagesLock);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString() + "\n" + newMessage);
                    base.AttemptReconnect();
                }
                Thread.Sleep(50);
            }
        }

        public List<string> GetMessages()
        {
            List<string> _messages = new List<string>();
            bool acquiredMessages = false;
            while (!acquiredMessages)
            {
                if (Monitor.TryEnter(receivedMessagesLock))
                {
                    _messages = new List<string>(receivedMessages);
                    try
                    {
                        receivedMessages.Clear();
                        acquiredMessages = true;
                    }
                    finally
                    {
                        Monitor.PulseAll(receivedMessagesLock);
                        Monitor.Exit(receivedMessagesLock);
                    }
                }
                else
                {
                    Thread.Sleep(25);
                }
            }

            return _messages;
        }
    }
}
