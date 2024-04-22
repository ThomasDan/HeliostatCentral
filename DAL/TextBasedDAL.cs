using HeliostatCentral.Interfaces;
using HeliostatCentral.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.DAL
{
    public class TextBasedDAL : iHeliostatDataAccessLayer
    {
        private readonly StreamReader _streamReader;
        private readonly StreamWriter _streamWriter;
        private bool _ownsStreams;

        // Constructor for production use
        public TextBasedDAL(string? filePath = null)
        {
            if (filePath == null)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "recordings/record.txt";
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                _streamReader = new StreamReader(fs);
                _streamWriter = new StreamWriter(fs) { AutoFlush = true };
                _ownsStreams = true;
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                _streamReader = new StreamReader(fs);
                _streamWriter = new StreamWriter(fs) { AutoFlush = true };
                _ownsStreams = true;
            }
        }

        // Constructor for testing purposes
        public TextBasedDAL(StreamReader streamReader, StreamWriter streamWriter)
        {
            _streamReader = streamReader;
            _streamWriter = streamWriter;
            _ownsStreams = false;
        }

        // ReadAllLines method reads all lines from the text file
        public List<string> ReadAllLines()
        {
            List<string> lines = new List<string>();
            string line;
            while ((line = _streamReader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }

        // LoadRecordings method reads the text file and converts the data to HeliostatRecording objects
        public List<HeliostatRecording> LoadRecordings()
        {
            List<HeliostatRecording> hrs = new List<HeliostatRecording>();
            List<string> lines = ReadAllLines();
            foreach (string line in lines)
            {
                HeliostatRecording hr = ConvertDataToHeliostat(line);
                hrs.Add(hr);
            }
            return hrs;
        }

        // ConvertDataToHeliostat method converts a string of data to a HeliostatRecording object
        public HeliostatRecording ConvertDataToHeliostat(string data)
        {
            string[] dataSeparated = data.Split(',');
            int hori = 0, vert = 0, light = 0;
            DateTime stamp = DateTime.Now;
            bool valid = true;
            try
            {
                hori = Convert.ToInt32(dataSeparated[0]);
                vert = Convert.ToInt32(dataSeparated[1]);
                light = Convert.ToInt32(dataSeparated[2]);
                stamp = Convert.ToDateTime(dataSeparated[3]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                valid = false;
            }
            return new HeliostatRecording(hori, vert, light, stamp, valid);
        }

        // SaveRecording method writes the HeliostatRecording objects to the text file
        public void SaveRecording(List<HeliostatRecording> hrs)
        {
            if (_streamWriter == null)
                throw new InvalidOperationException("StreamWriter is not initialized.");

            List<string> existingRecords = ReadAllLines();  // Ensure this method does not rely on uninitialized objects
            if (existingRecords.Count > 0)
            {
                _streamWriter.WriteLine(String.Join('\n', existingRecords));
            }

            foreach (HeliostatRecording hr in hrs)
            {
                if (hr.IsValid)
                {
                    // If it is the last element, we want it to use "Write()" instead of writeline, since previous "writeline" already created a fresh line.
                    //    Else the text file will have a spare empty line.
                    if (hrs.Last().DateTimeStamp != hr.DateTimeStamp)
                    {
                        _streamWriter.WriteLine($"{hr.HorizontalDegrees},{hr.VerticalDegrees},{hr.LightLevel},{hr.DateTimeStamp}");

                    } else
                    {
                        _streamWriter.Write($"{hr.HorizontalDegrees},{hr.VerticalDegrees},{hr.LightLevel},{hr.DateTimeStamp}");
                    }
                }
                else
                {
                    Console.WriteLine("Attempted to insert invalid record: " + hr.ToString());
                }
            }
            _streamWriter.Flush(); // Ensure all data is written to the underlying stream
        }

        public void Dispose()
        {
            if (_ownsStreams)
            {
                _streamReader?.Dispose();
                _streamWriter?.Dispose();
            }
        }

        // Destructor to ensure resources are released when the object is garbage collected 
        ~TextBasedDAL()
        {
            Dispose();
        }
    }
}


/*
OLD CODE
namespace HeliostatCentral.DAL
{
    public class TextBasedDAL : iHeliostatDataAccessLayer
    {
        string relativePath;

        public TextBasedDAL()
        {
            this.relativePath = AppDomain.CurrentDomain.BaseDirectory + "recordings/record.txt";
        }

        // Made public for testing purposes
        public List<string> ReadAllLines()
        {
            StreamReader sr = new StreamReader(relativePath);
            List<string> lines = new List<string>();
            string? line = sr.ReadLine();

            while (line != null)
            {
                lines.Add(line.ToString());
                line = sr.ReadLine();
            }
            sr.Close();

            return lines;
        }

        public List<HeliostatRecording> LoadRecordings()
        {
            List<HeliostatRecording> hrs = new List<HeliostatRecording>();

            List<string> lines = ReadAllLines();

            foreach (string line in lines)
            {
                HeliostatRecording hr = ConvertDataToHeliostat(line);
                hrs.Add(hr);
            }

            return hrs;
        }

        // Made public for testing purposes
        public HeliostatRecording ConvertDataToHeliostat(string data)
        {
            string[] dataSeparated = data.Split(',');

            // Converting 3x int, Converting DateTime
            int hori = 0;
            int vert = 0;
            int light = 0;
            DateTime stamp = DateTime.Now;
            bool valid = true;

            try
            {
                hori = Convert.ToInt32(dataSeparated[0]);
                vert = Convert.ToInt32(dataSeparated[1]);
                light = Convert.ToInt32(dataSeparated[2]);
                stamp = Convert.ToDateTime(dataSeparated[3]);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                valid = false;
            }

            HeliostatRecording hr = new HeliostatRecording(hori, vert, light, stamp, valid);

            return hr;
        }

        public void SaveRecording(List<HeliostatRecording> hrs)
        {
            // ReadAllLines() gets us a list of strings, each string representing One line in the text file we use to store HeliostatRecordings
            List<string> existingRecords = ReadAllLines();

            StreamWriter sw = new StreamWriter(relativePath);

            if (existingRecords.Count > 0)
            {
                // Here we join all the existing recordings into one string, across many lines (\n creating new lines between each existing record)
                string joinedExistingRecords = String.Join('\n', existingRecords);
                // Writing all the existing record-lines to the text file at once.
                sw.WriteLine(joinedExistingRecords);
                // We do this, because as soon as we open up the file and start writing, the existing lines are overwritten.
            }

           // Here we write the new 
            foreach (HeliostatRecording hr in hrs) {
                if (hr.IsValid)
                {
                    sw.WriteLine(
                        hr.HorizontalDegrees.ToString() + "," +
                        hr.VerticalDegrees.ToString() + "," +
                        hr.LightLevel.ToString() + "," +
                        hr.DateTimeStamp.ToString()
                    );
                } else
                {
                    Console.WriteLine("Attempted to insert invalid record: " + hr.ToString());
                }
            }

            sw.Close();
        }
    }
}
*/