using System.IO;
using System.Text;

namespace cube
{
#pragma warning disable SYSLIB0001

    public class File_Operation
    {
        public static string read_json(string path)
        {
            string jsonStr;

            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                jsonStr = reader.ReadToEnd();
            }

            return jsonStr;
        }

    }
}