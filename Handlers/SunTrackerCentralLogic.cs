using SunTrackerCentral.Interfaces;
using SunTrackerCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTrackerCentral.Handlers
{
    public class SunTrackerCentralLogic
    {
        private Thread thread;
        private iReceiveCommunication sunTrackerComm;
        private List<iSendCommunication> solarPanelComms;
        private iInterpretSunTrackerCommunication interpreter;
        private iSunTrackerDataAccessLayer dal;

        public SunTrackerCentralLogic(iInterpretSunTrackerCommunication _interp, iReceiveCommunication _sunTrackerComm, List<iSendCommunication> _solarPanelComms, iSunTrackerDataAccessLayer _dal)
        {
            this.interpreter = _interp;
            this.sunTrackerComm = _sunTrackerComm;
            this.solarPanelComms = _solarPanelComms;
            this.dal = _dal;
            this.thread = new Thread(Run);
            Initialize();
        }

        void Initialize()
        {
            sunTrackerComm.Initialize(SaveReceivedMessage);
            foreach ( iSendCommunication solarPanel in this.solarPanelComms )
            {
                solarPanel.Initialize();
            }

            thread.Start();
        }

        private async Task SaveReceivedMessage(string newMessage)
        {
            List<SunTrackerRecording> unsavedSTRs = new List<SunTrackerRecording>();
            
            // Then we convert the message into a SunTrackerRecording
            SunTrackerRecording str = interpreter.ConvertStringToSunTrackerRecording(newMessage);
            Console.WriteLine("New Recording: " + str.ToString());
            unsavedSTRs.Add(str);

            // Then we save the SunTrackerRecording to the database
            dal.SaveRecordings(unsavedSTRs);
        }

        private void Run()
        {
            List<SunTrackerRecording> strs;

            while (true)
            {
                // Here we get all SunTracker records
                strs = dal.LoadRecordings();
                if (strs.Count > 0)
                {
                    // Then we figure out which of the recent records seem like the best potential instruction for the Solar Panels
                    SunTrackerRecording bestSTR = DetermineBestRecording(strs);

                    Console.WriteLine("Best Recording: " + bestSTR.ToString());

                    // Then we convert the Recording into a string instruction
                    string instruction = interpreter.ConvertSunTrackerRecordingToString(bestSTR);
                    // ..and send the instruction to the Solar Panels
                    foreach (iSendCommunication solarPanel in solarPanelComms)
                    {
                        solarPanel.SendCommunication(instruction);
                    }
                }

                Thread.Sleep(2500);
            }
        }

        /// <summary>
        /// Looks at the recordings taken from around this time of day in the past 7 days, then returns the one with the highest Light Level
        /// </summary>
        /// <param name="strs">All SunTracker Recordings</param>
        /// <returns>The SunTracker Recording with the highest LightLevel</returns>
        private SunTrackerRecording DetermineBestRecording(List<SunTrackerRecording> strs)
        {
            // First we remove all records older than 7 days, as the sun's path has changed significantly since 7 days ago
            List<SunTrackerRecording> latestRecords = strs.Where(a => a.DateTimeStamp.Date > DateTime.Now.AddDays(-7).Date).ToList();

            SunTrackerRecording bestSTR = new SunTrackerRecording(0,0,0,DateTime.Now,false);
            
            foreach (SunTrackerRecording record in latestRecords)
            {
                TimeSpan now = DateTime.Now.TimeOfDay;
                
                // We look at recordings within 5 seconds of this time of day
                if (
                    record.DateTimeStamp.TimeOfDay < now.Add(new TimeSpan(0,0,5)) && 
                    record.DateTimeStamp.TimeOfDay > now.Add(new TimeSpan(0,0,-5))
                    )
                {
                    if (record.LightLevel > bestSTR.LightLevel)
                    {
                        bestSTR = record;
                    }
                }
            }

            return bestSTR;
        }
    }
}
