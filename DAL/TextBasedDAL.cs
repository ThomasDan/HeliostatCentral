using HeliostatCentral.Interfaces;
using HeliostatCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeliostatCentral.DAL
{
    internal class TextBasedDAL : iHeliostatDataAccessLayer
    {
        string relativePath;
        StreamWriter writer;

        TextBasedDAL(string _relativePath)
        {
            this.relativePath = _relativePath;
        }

        public List<HeliostatRecording> LoadRecordings()
        {
            throw new NotImplementedException();
        }

        public void SaveRecording(HeliostatRecording hr)
        {
            //Environment.GetFolderPath(Environment.SpecialFolder. ApplicationData);


            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Data\Orders\Test.xml");
            string sFilePath = Path.GetFullPath(sFile);
        }
    }
}
