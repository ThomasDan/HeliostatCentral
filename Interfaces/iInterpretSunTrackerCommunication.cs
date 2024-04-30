using SunTrackerCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTrackerCentral.Interfaces
{
    public interface iInterpretSunTrackerCommunication
    {
        public string ConvertSunTrackerRecordingToString(SunTrackerRecording hr);
        public SunTrackerRecording ConvertStringToSunTrackerRecording(string message);
    }
}
