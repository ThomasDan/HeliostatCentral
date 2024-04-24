using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Handlers
{
    public abstract class SerialPortHandlerBase
    {
        protected string name;
        protected SerialPort serialPort;

        public SerialPortHandlerBase(string _name)
        {
            name = _name;
            serialPort = new SerialPort();
            serialPort.PortName = GetPortName();

            // Arduino Unos by default use baud rates of 9600
            serialPort.BaudRate = 9600;
            // Timing out in 500 miliseconds is arbitrary
            serialPort.ReadTimeout = 500;
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

        public virtual void Initialize()
        {
            serialPort.Open();
            // Ideally, there would be a serialPort.Close(); call for when the program ends. But the program doesn't end.
        }
    }
}
