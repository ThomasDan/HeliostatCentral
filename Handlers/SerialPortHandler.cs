using HeliostatCentral.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace HeliostatCentral.Handlers
{
    class SerialPortHandler : iCommunicateWithHeliostat
    {
        SerialPort serialPort;

        List<string> receivedMessages;
        object receivedMessagesLock;

        public SerialPortHandler() 
        {
            serialPort = new SerialPort();
            serialPort.PortName = GetPortName();

            // Arduino Unos by default use baud rates of 9600
            serialPort.BaudRate = 9600;
            // Timing out in 500 miliseconds is arbitrary
            serialPort.ReadTimeout = 500;
            receivedMessages = new List<string>();
            receivedMessagesLock = new object();
        }

        /// <summary>
        /// This is a dirty piece of placeholder code, purely existing for test purposes
        /// </summary>
        private string GetPortName()
        {
            string[] portNames = SerialPort.GetPortNames();
            int choice = 0;
            if (portNames.Count() > 1) {
                Console.WriteLine("Ports:");
                for (int i = 0; i < portNames.Length; i++)
                {
                    Console.WriteLine(" " + (i + 1) + ". " + portNames[i]);
                }

                Console.Write("Enter Serial Port Number: ");
                choice = Convert.ToInt32(Console.ReadLine()) - 1;
            } else if (portNames.Count() == 1)
            {
                Console.WriteLine("Automatically chose port " + portNames[0] + " (only option)");
            } else
            {
                throw new IOException("no serial port found");
            }

            return portNames[choice];
        }

        public void Initialize()
        {
            Thread receiver = new Thread(receiveCommunication);
            serialPort.Open();
            receiver.Start();

            // Ideally, there would be a serialPort.Close(); call for when the program ends. But the program doesn't end.
        }

        /// <summary>
        /// Infinitely loop that receives any messages waiting in the serialPort every half second
        /// </summary>
        private void receiveCommunication()
        {
            while (true)
            {
                try
                {
                    if (Monitor.TryEnter(receivedMessagesLock))
                    {
                        string newMessage = serialPort.ReadLine();
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
                    Console.WriteLine(e.ToString());
                }
                Thread.Sleep(500);
            }
        }

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
