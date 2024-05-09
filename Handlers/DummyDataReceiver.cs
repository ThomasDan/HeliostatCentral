using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunTrackerCentral.Interfaces;
using static SunTrackerCentral.Interfaces.iReceiveCommunication;

namespace SunTrackerCentral.Handlers
{
    public class DummyDataReceiver : iReceiveCommunication
    {
        //public delegate void SaveReceivedMessage(string message);
        private SaveReceivedMessage saveReceivedMessage;

        public DummyDataReceiver() 
        {

        }

        public void Initialize(SaveReceivedMessage _saveReceivedMessage)
        {
            this.saveReceivedMessage = _saveReceivedMessage;
            Thread receiver = new Thread(ReceiveCommunication);
            receiver.Start();
        }

        /// <summary>
        /// Infinite loop that generates a new message 20 times per second
        /// </summary>
        private void ReceiveCommunication()
        {
            string newMessage = "";
            Random rnd;
            while (true)
            {
                rnd = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
                newMessage = $"{rnd.Next(180)},{rnd.Next(180)},{rnd.Next(10) * 100}";
                // https://stackoverflow.com/a/34518914
                // Here we create and run an async task, which awaits the saveReceivedMessage Task.
                // This allows the delegate Task to be run asynchronously.
                Task.Run(
                    async () => await saveReceivedMessage(newMessage)
                    );
                Thread.Sleep(25);
            }
        }
    }
}
