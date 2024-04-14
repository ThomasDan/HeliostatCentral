using HeliostatCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Interfaces
{
    public interface iHeliostatDataAccessLayer
    {
        internal List<HeliostatRecording> LoadRecordings();
        internal void SaveRecording(HeliostatRecording hr);
    }
}
