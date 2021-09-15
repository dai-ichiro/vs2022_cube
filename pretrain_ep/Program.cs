using System.Text;
using System.Text.Json;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace pretrain_ep;

class Program
{
    static void Main(string[] args)
    {
        static string ReadAllLine(string filePath)
        {
            StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
            string allLine = sr.ReadToEnd();
            sr.Close();
            return allLine;
        }
        string jsonStr;

        jsonStr = ReadAllLine("./data/cp_move_table.json");
        Global.cp_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

        jsonStr = ReadAllLine("./data/co_move_table.json");
        Global.co_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

        jsonStr = ReadAllLine("./data/eo_move_table.json");
        Global.eo_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

        jsonStr = ReadAllLine("./data/cp_co_prune_table.json");
        Global.cp_co_prune_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

        jsonStr = ReadAllLine("./data/cp_eo_prune_table.json");
        Global.cp_eo_prune_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

        jsonStr = ReadAllLine("./data/co_eo_prune_table.json");
        Global.co_eo_prune_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

        int[] new_ep = new int[12];
        int now_ep_index;

        State scrambled_state = new State(0, 0, Enumerable.Range(0, 12).ToArray(), 0);
        Mini_State scrambled_mini_state = new Mini_State(0, 0, 0);
        int[] initial_ep = scrambled_state.ep;

        bool is_move_available(int pre, int now)
        {
            if (pre == -1) return true;
            if (pre / 3 == now / 3) return false;
            if (pre / 3 == 0 && now / 3 == 1) return false; //U→Dはダメ
            if (pre / 3 == 3 && now / 3 == 2) return false; //R→Lはダメ
            if (pre / 3 == 4 && now / 3 == 5) return false; //F→Bはダメ
            return true;
        }

        int ep_to_index(int[] ep)
        {
            int index = 0;
            for (int i = 0; i < 12; i++)
            {
                index *= 12 - i;
                for (int j = i + 1; j < 12; j++)
                {
                    if (ep[i] > ep[j]) index += 1;
                }
            }
            return index;
        }

        bool is_solved(Mini_State m_state)
        {
            return (m_state.cp == 0 && m_state.co == 0 && m_state.eo == 0);
        }

        bool prune(int depth, Mini_State m_state)
        {
            if (depth < Global.cp_co_prune_table[m_state.cp][m_state.co]) return true;
            if (depth < Global.cp_eo_prune_table[m_state.cp][m_state.eo]) return true;
            if (depth < Global.co_eo_prune_table[m_state.co][m_state.eo]) return true;
            return false;
        }

        int ep_move(int[] ep, List<int> moves)
        {
            new_ep = ep;
            foreach (int each_move in moves)
            {
                new_ep = Global.ep_move_dict[each_move].Select(x => new_ep[x]).ToArray();
            }

            return ep_to_index(new_ep);
        }

        List<int> current_solution = new List<int> { };
        string[] last_5_solution = new string[] { };

        List<int> result = new List<int>();

        void depth_limited_search(Mini_State m_state, int depth)
        {
            if (depth == 0 && is_solved(m_state))
            {
                now_ep_index = ep_move(initial_ep, current_solution);

                result.Add(now_ep_index);
                return;
            }

            if (depth == 0)
            {
                return;
            }

            if (prune(depth, m_state))
            {
                return;
            }

            int prev_move = current_solution.Count == 0 ? -1 : current_solution.Last();
            for (int move_num = 0; move_num < 18; move_num++)
            {
                if (!(is_move_available(prev_move, move_num))) continue;
                current_solution.Add(move_num);
                depth_limited_search(m_state.apply_move(move_num), depth - 1);
                current_solution.RemoveAt(current_solution.Count - 1);
            }
            return;
        }

        var sw = new Stopwatch();
        sw.Start();

        Console.WriteLine("Start searching...");


        int depth = 6;

        Console.WriteLine("Start searching lenght {0}", depth);
        depth_limited_search(scrambled_mini_state, depth);


        HashSet<int> final_result = new HashSet<int>(result);

        Console.WriteLine($"list_count: {result.Count}");
        Console.WriteLine($"hash_count: {final_result.Count}");

        string new_jsonStr = JsonSerializer.Serialize(final_result);
        string file_name = $"ep_index_{depth}.json";

        StreamWriter writer = new StreamWriter(file_name, false, Encoding.GetEncoding("utf-8"));
        writer.WriteLine(new_jsonStr);
        writer.Close();

        sw.Stop();
        TimeSpan ts = sw.Elapsed;
        Console.WriteLine("Finished!({0})", ts);

        Console.ReadKey();
    }
}