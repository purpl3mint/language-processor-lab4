using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    interface IFileService
    {
        string ReadFile(string filePath);
        void SaveFile(string filePath, string data);
    }
}
