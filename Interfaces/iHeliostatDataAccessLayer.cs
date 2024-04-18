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
        public List<HeliostatRecording> LoadRecordings();
        public void SaveRecording(HeliostatRecording hr);
    }
}
