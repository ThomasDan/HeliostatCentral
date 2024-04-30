using SunTrackerCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTrackerCentral.Interfaces
{
    public interface iSunTrackerDataAccessLayer
    {
        public List<SunTrackerRecording> LoadRecordings();
        public void SaveRecordings(List<SunTrackerRecording> hrs);
    }
}
