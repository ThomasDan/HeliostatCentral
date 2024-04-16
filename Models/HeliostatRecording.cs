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
        internal int HorizontalDegrees;
        internal int VerticalDegrees;
        internal int LightLevel;
        internal DateTime DateTimeStamp;

        public HeliostatRecording(int horizontalDegrees, int verticalDegrees, int lightLevel, DateTime dateTimeStamp)
        {
            this.HorizontalDegrees = horizontalDegrees;
            this.VerticalDegrees = verticalDegrees;
            this.LightLevel = lightLevel;
            this.DateTimeStamp = dateTimeStamp;
        }
        public override string ToString()
        {
            return DateTimeStamp.ToString("dd/MM/yy HH:mm:ss") + " - H: " + HorizontalDegrees + " | V: " + VerticalDegrees + " | L: " + LightLevel;
        }
    }
}
