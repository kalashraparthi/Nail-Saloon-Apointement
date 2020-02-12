using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_NailSalon
{
    class PathHelper
    {
        public static string GetFileLocation(string filename)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            return Path.Combine(currentDirectory,filename);
        }
    }
}
