using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab4
{
    public class DefaultFileService : IFileService
    {
        public string ReadFile(string filePath)
        {
            string newData = "";

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    newData = sr.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return newData;
        }
        public void SaveFile(string filePath, string data)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(data);
                }

                Console.WriteLine("Запись выполнена");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
