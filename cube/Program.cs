using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace cube
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading pretrained data...");

            Global.cp_move_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/cp_move_table.json"));
            Global.co_move_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/co_move_table.json"));
            Global.eo_move_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/eo_move_table.json"));

            Global.cp_co_prune_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/cp_co_prune_table.json"));
            Global.cp_eo_prune_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/cp_eo_prune_table.json"));
            Global.co_eo_prune_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/co_eo_prune_table.json"));

            Global.ep_dict = JsonSerializer.Deserialize<Dictionary<int, int>>(File_Operation.read_json("./data/ep_dict.json"));

            int[] new_ep = new int[12];

            string scramble;
            //scramble = "R' U' F R' B' F2 L2 D' U' L2 F2 D' L2 D' R B D2 L D2 F2 U2 L R' U' F";
            scramble = "R' U' F R' B' F2 L2 D' U' L2 F2 D' L2 D' R B";
            //scramble = "R' U' F R' B' F2 L2 D' U' L2 F2 D' L2 D' R";
            //scramble = "R' U' F R' B' F2 L2 D' U' L2 F2 D' L2";
            //scramble = "R' U' F R' B' F2 L2 D' U' L2 F2 D'";
            //scramble = "R' U' F R' B' F2 L2 D' U'";
            //scramble = "R' U' F R' B' F2 L2";
            //scramble = "R' U' F R' B'";
            //scramble = "R' U' F R'";

            State scramble2state(string scramble)
            {
                State start_state = new State(0, 0, Enumerable.Range(0, 12).ToArray(), 0);
                int[] moves = scramble.Split(" ").Select(x => Array.IndexOf(Global.move_names, x)).ToArray();
                foreach (int each_move in moves)
                {
                    start_state = start_state.apply_move(each_move);
                }
                return start_state;
            }

            State scrambled_state = scramble2state(scramble);

            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("Start searching...");

            void multi_task(State scrambled_state, int first_move, int depth)
            {
                State state_after_move = scrambled_state.apply_move(first_move);
                Mini_State mini_state = new Mini_State(state_after_move.cp, state_after_move.co, state_after_move.eo);
                Search search = new Search(mini_state, scrambled_state.ep, first_move);
                search.start_search(depth);
            }

            List<Task> create_task(int depth)
            {
                List<Task> task_list = new List<Task>();
                for (int i = 0; i < 18; i++)
                {
                    int temp_move = i;
                    Task task = new Task(() => multi_task(scrambled_state, temp_move, depth));
                    task_list.Add(task);
                }
                return task_list;
            }

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Start searching lenght {0}", i + 1);

                var task_list = create_task(i);

                foreach (Task each_task in task_list)
                {
                    each_task.Start();
                }
                Task.WaitAll(task_list.ToArray());
                if (Global.finished == true) break;
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Console.WriteLine("Finished!({0})", ts);

            Console.ReadKey();
        }
    }
}
