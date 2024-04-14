using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Models
{
    // Simple POCO class
    public class HeliostatRecording
    {
        internal int horizontalDegrees;
        internal int verticalDegrees;
        internal int lightLevel;
        internal DateTime datetime;

        public HeliostatRecording(int _hori, int _vert, int _light, DateTime _dt)
        {
            this.horizontalDegrees = _hori;
            this.verticalDegrees = _vert;
            this.lightLevel = _light;
            this.datetime = _dt;
        }
    }
}
