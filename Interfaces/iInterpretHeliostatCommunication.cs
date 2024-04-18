using HeliostatCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Interfaces
{
    public interface iInterpretHeliostatCommunication
    {
        public string ConvertHeliostatRecordingToString(HeliostatRecording hr);
        public HeliostatRecording ConvertStringToHeliostatRecording(string message);
    }
}
