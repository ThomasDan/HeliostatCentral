using HeliostatCentral.Interfaces;
using HeliostatCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.Handlers
{
    public class CommaSeparationConvertionHandler : iInterpretHeliostatCommunication
    {
        public string ConvertHeliostatRecordingToString(HeliostatRecording hr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(hr.HorizontalDegrees);
            sb.Append(',');
            sb.Append(hr.VerticalDegrees);
            sb.Append(',');
            sb.Append(hr.LightLevel);

            return sb.ToString();
        }

        public HeliostatRecording ConvertStringToHeliostatRecording(string message)
        {
            string[] splitString = message.Split(",");

            string horizontalString = splitString[0];
            string verticalString = splitString[1];
            string lightString = splitString[2];

            int horizontal = 0;
            int vertical = 0;
            int light = 0;
            bool valid = true;

            try
            {
                // Ideally, each value shoyld have a Try around itself, but this is fine
                horizontal = int.Parse(horizontalString);
                vertical = int.Parse(verticalString);
                light = int.Parse(lightString);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                valid = false;
            }

            HeliostatRecording hr = new HeliostatRecording(
                horizontalDegrees: horizontal,
                verticalDegrees: vertical,
                lightLevel: light,
                dateTimeStamp: DateTime.Now,
                valid
                );

            return hr;
        }
    }
}
