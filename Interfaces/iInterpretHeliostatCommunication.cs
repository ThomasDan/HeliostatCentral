using HeliostatCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Interfaces
{
    internal interface iInterpretHeliostatCommunication
    {
        internal string ConvertHeliostatRecordingToString(HeliostatRecording hr);
        internal HeliostatRecording ConvertStringToHeliostatRecording(string message);
    }
}
