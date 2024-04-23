using HeliostatCentral.Interfaces;
using HeliostatCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Handlers
{
    public class HeliostatCentralLogic
    {
        private Thread thread;
        private iInterpretHeliostatCommunication interpreter;
        private iCommunicate sunTrackerComm;
        private List<iCommunicate> solarPanelComms;
        private iHeliostatDataAccessLayer dal;

        public HeliostatCentralLogic(iInterpretHeliostatCommunication _interp,  iCommunicate _sunTrackerComm, List<iCommunicate> _solarPanelComms, iHeliostatDataAccessLayer _dal)
        {
            this.interpreter = _interp;
            this.sunTrackerComm = _sunTrackerComm;
            this.solarPanelComms = _solarPanelComms;
            this.dal = _dal;
            this.thread = new Thread(Run);
        }

        public void Initialize()
        {
            //sunTrackerComm.Initialize();
            thread.Start();
        }

        private void Run()
        {
            List<string> rawReceivedMessages;
            List<HeliostatRecording> unsavedHRs;
            List<HeliostatRecording> hrs;
            while(true)
            {
                // Here we acquire the latest recording(s) from the serial port
                /*
                rawReceivedMessages = sunTrackerComm.GetMessages();
                unsavedHRs = new List<HeliostatRecording>();
                foreach(string message in rawReceivedMessages)
                {
                    // Then we convert each message (if any) into a HeliostatRecording
                    HeliostatRecording hr = interpreter.ConvertStringToHeliostatRecording(message);
                    unsavedHRs.Add(hr);
                }
                if (unsavedHRs.Count > 0)
                {
                    // Then we save the HeliostatRecording to the database
                    dal.SaveRecording(unsavedHRs);
                    // Thus we have updated the database
                }
                */

                hrs = new List<HeliostatRecording>()
                {
                    new HeliostatRecording(100, 100, 370, DateTime.Now, true)
                };
                dal.SaveRecording(hrs);

                // Here we get all freshly-updated heliostat records
                hrs = dal.LoadRecordings();
                Console.WriteLine(hrs.Count());
                if (hrs.Count > 0)
                {
                    // Then we figure out which of the recent records seem like the best potential instruction for the Heliostat
                    HeliostatRecording bestHR = DetermineBestRecording(hrs);

                    Console.WriteLine(bestHR.ToString());

                    // Then we convert the Recording into a string instruction
                    string instruction = interpreter.ConvertHeliostatRecordingToString(bestHR);
                    // ..and send the instruction to the Solar Panels
                    foreach (iCommunicate solarPanel in solarPanelComms)
                    {
                        solarPanel.SendCommunication(instruction);
                    }
                }

                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Looks at the recordings taken from around this time of day in the past 7 days, then returns the one with the highest Light Level
        /// </summary>
        /// <param name="hrs">All Heliostat Recordings</param>
        /// <returns>The Heliostat Recording with the highest LightLevel</returns>
        private HeliostatRecording DetermineBestRecording(List<HeliostatRecording> hrs)
        {
            // First we remove all records older than 7 days, as the sun's path has changed significantly since 7 days ago
            List<HeliostatRecording> latestRecords = hrs.Where(a => a.DateTimeStamp.Date > DateTime.Now.AddDays(-7).Date).ToList();

            HeliostatRecording bestHR = new HeliostatRecording(0,0,0,DateTime.Now,false);
            
            foreach (HeliostatRecording record in latestRecords)
            {
                TimeSpan now = DateTime.Now.TimeOfDay;
                
                // We look at recordings within 5 seconds of this time of day
                if (
                    record.DateTimeStamp.TimeOfDay < now.Add(new TimeSpan(0,0,5)) && 
                    record.DateTimeStamp.TimeOfDay > now.Add(new TimeSpan(0,0,-5))
                    )
                {
                    if (record.LightLevel > bestHR.LightLevel)
                    {
                        bestHR = record;
                    }
                }
            }

            return bestHR;
        }
    }
}
