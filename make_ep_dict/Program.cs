using System.Text;
using System.Text.Json;

namespace make_ep_dict
{
    class Program
    {
        static void Main(string[] args)
        {
            string read_json(string file_path)
            {
                string jsonStr;
                using (var sr = new StreamReader(file_path, Encoding.UTF8))
                {
                    jsonStr = sr.ReadToEnd();
                }
                return jsonStr;
            }

            Console.WriteLine("Loading pretrained data...");

            var ep_index_6 = JsonSerializer.Deserialize<HashSet<int>>(read_json("./data/ep_index_6.json"));
            var ep_index_7 = JsonSerializer.Deserialize<HashSet<int>>(read_json("./data/ep_index_7.json"));
            var ep_index_8 = JsonSerializer.Deserialize<HashSet<int>>(read_json("./data/ep_index_8.json"));
            var ep_index_9 = JsonSerializer.Deserialize<HashSet<int>>(read_json("./data/ep_index_9.json"));
            var ep_index_10 = JsonSerializer.Deserialize<HashSet<int>>(read_json("./data/ep_index_10.json"));
            var ep_index_11 = JsonSerializer.Deserialize<HashSet<int>>(read_json("./data/ep_index_11.json"));
            var ep_index_12 = JsonSerializer.Deserialize<HashSet<int>>(read_json("./data/ep_index_12.json"));

            Console.WriteLine("Making new dictionary...");

            Dictionary<int, int> ep_dict = new Dictionary<int, int>();

            foreach (int each in ep_index_6)
            {
                ep_dict.Add(each, 2);
            }

            foreach (int each in ep_index_7)
            {
                if (!(ep_dict.ContainsKey(each)))
                {
                    ep_dict.Add(each, 3);
                }
            }

            foreach (int each in ep_index_8)
            {
                if (!(ep_dict.ContainsKey(each)))
                {
                    ep_dict.Add(each, 3);
                }
            }

            foreach (int each in ep_index_9)
            {
                if (!(ep_dict.ContainsKey(each)))
                {
                    ep_dict.Add(each, 4);
                }
            }

            foreach (int each in ep_index_10)
            {
                if (!(ep_dict.ContainsKey(each)))
                {
                    ep_dict.Add(each, 4);
                }
            }

            foreach (int each in ep_index_11)
            {
                if (!(ep_dict.ContainsKey(each)))
                {
                    ep_dict.Add(each, 5);
                }
            }

            foreach (int each in ep_index_12)
            {
                if (!(ep_dict.ContainsKey(each)))
                {
                    ep_dict.Add(each, 5);
                }
            }

            string jsonStr = JsonSerializer.Serialize(ep_dict);

            using (var writer = new StreamWriter("ep_dict.json", false, Encoding.UTF8))
            {
                writer.WriteLine(jsonStr);
            }

            Console.WriteLine("Finished!");

            Console.ReadKey();
        }
    }
}