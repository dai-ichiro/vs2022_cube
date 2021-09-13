﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;


namespace cube
{
#pragma warning disable SYSLIB0001

    public class File_Operation
    {
        public static int[,] read_array(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (int[,])bf.Deserialize(fs);
            }
        }

        public static Dictionary<int, int> read_dict(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (Dictionary<int, int>)bf.Deserialize(fs);
            }
        }

    }
}