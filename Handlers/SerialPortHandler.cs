using HeliostatCentral.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace HeliostatCentral.Handlers
{
    class SerialPortHandler : iCommunicate
    {
        SerialPort serialPort;

        List<string> receivedMessages;
        object receivedMessagesLock;
        string name;

        public SerialPortHandler(string _name) 
        {
            name = _name;
            serialPort = new SerialPort();
            serialPort.PortName = GetPortName();

            // Arduino Unos by default use baud rates of 9600
            serialPort.BaudRate = 9600;
            // Timing out in 500 miliseconds is arbitrary
            serialPort.ReadTimeout = 500;

            receivedMessages = new List<string>();
            receivedMessagesLock = new object();

            serialPort.Open();
        }

        /// <summary>
        /// This is a dirty piece of placeholder code, purely existing for test purposes
        /// </summary>
        private string GetPortName()
        {
            bool validPortFound = false;
            int choice = 0;
            string[] portNames = new string[0];
            while (!validPortFound)
            {
                portNames = SerialPort.GetPortNames();
                if (portNames.Count() > 0)
                {
                    Console.WriteLine($"Ports for {name}:");
                    for (int i = 0; i < portNames.Length; i++)
                    {
                        Console.WriteLine(" " + (i + 1) + ". " + portNames[i]);
                    }

                    Console.Write("Enter Serial Port Number: ");
                    choice = Convert.ToInt32(Console.ReadLine()) - 1;
                    validPortFound = true;
                }
                else
                {
                    Console.WriteLine($"...No serial port found for {name} ...");
                    Thread.Sleep(2000);
                }
            }

            return portNames[choice];
        }

        public void Initialize()
        {
            Thread receiver = new Thread(receiveCommunication);
            receiver.Start();

            // Ideally, there would be a serialPort.Close(); call for when the program ends. But the program doesn't end.
        }

        /// <summary>
        /// Infinite loop that receives any messages waiting in the serialPort 20 times per second
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
                Thread.Sleep(50);
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
