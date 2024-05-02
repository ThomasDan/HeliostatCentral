using SunTrackerCentral.Interfaces;
using SunTrackerCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTrackerCentral.Handlers
{
    public class CommaSeparationConvertionHandler : iInterpretSunTrackerCommunication
    {
        public string ConvertSunTrackerRecordingToString(SunTrackerRecording str)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('<');
            sb.Append(str.HorizontalDegrees);
            sb.Append(',');
            sb.Append(str.VerticalDegrees);
            sb.Append(',');
            sb.Append(str.LightLevel);
            sb.Append('>');

            return sb.ToString();
        }

        public SunTrackerRecording ConvertStringToSunTrackerRecording(string message)
        {
            int horizontal = 0;
            int vertical = 0;
            int light = 0;
            bool valid = true;
            try
            {
                string[] splitString = message.Split(",");

                string horizontalString = splitString[0];
                string verticalString = splitString[1];
                string lightString = splitString[2];
            
                // Ideally, each value shoyld have a Try around itself, but this is fine
                horizontal = int.Parse(horizontalString);
                vertical = int.Parse(verticalString);
                light = int.Parse(lightString);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\nThe Message causing error: " + message);
                valid = false;
            }

            SunTrackerRecording str = new SunTrackerRecording(
                horizontalDegrees: horizontal,
                verticalDegrees: vertical,
                lightLevel: light,
                dateTimeStamp: DateTime.Now,
                valid
                );

            return str;
        }
    }
}
