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

        List<string> messages;
        object messagesLock;

        public SerialPortHandler() 
        {
            serialPort = new SerialPort();
            serialPort.PortName = GetPortName();

            // Arduino Unos by default use baud rates of 9600
            serialPort.BaudRate = 9600;
            serialPort.ReadTimeout = 500;
            messages = new List<string>();
            messagesLock = new object();
        }

        /// <summary>
        /// This is a dirty piece of placeholder code, purely existing for test purposes
        /// </summary>
        private string GetPortName()
        {
            Console.WriteLine("Ports:");
            string[] portNames = SerialPort.GetPortNames();
            for (int i = 0; i < portNames.Length; i++)
            {
                Console.WriteLine(" " + (i + 1) + ". " + portNames[i]);
            }

            //Console.Write("Enter Serial Port Number: ");

            //int choice = Convert.ToInt32(Console.ReadLine()) - 1;

            return "COM1";//portNames[choice];
        }

        public void Run()
        {
            Thread receiver = new Thread(receiveCommunication);
            serialPort.Open();
            receiver.Start();

            // Ideally, there would be a serialPort.Close(); call for when the program ends. But the program doesn't end.
        }

        

        private void receiveCommunication()
        {
            while (true)
            {
                try
                {
                    if (Monitor.TryEnter(messagesLock))
                    {
                        string newMessage = serialPort.ReadLine();
                        try
                        {
                            messages.Add(newMessage);
                        }
                        finally
                        {
                            Monitor.PulseAll(messagesLock);
                            Monitor.Exit(messagesLock);
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
                if (Monitor.TryEnter(messagesLock))
                {
                    _messages = new List<string>(messages);
                    try
                    {
                        messages.Clear();
                        acquiredMessages = true;
                    }
                    finally
                    {
                        Monitor.PulseAll(messagesLock);
                        Monitor.Exit(messagesLock);
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
