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
        // After creation, these being Records, they remain get-only.
        internal int HorizontalDegrees { get; }
        internal int VerticalDegrees { get; }
        internal int LightLevel { get; }
        internal DateTime DateTimeStamp { get; }
        internal bool IsValid { get; }

        public HeliostatRecording(int horizontalDegrees, int verticalDegrees, int lightLevel, DateTime dateTimeStamp, bool isValid)
        {
            this.HorizontalDegrees = horizontalDegrees;
            this.VerticalDegrees = verticalDegrees;
            this.LightLevel = lightLevel;
            this.DateTimeStamp = dateTimeStamp;
            this.IsValid = isValid;
        }
        public override string ToString()
        {
            return DateTimeStamp.ToString("dd/MM/yy HH:mm:ss.ff") + " - H: " + HorizontalDegrees + " | V: " + VerticalDegrees + " | L: " + LightLevel;
        }
    }
}
