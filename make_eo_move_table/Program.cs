using System.Text;
using System.Text.Json;
class Program
{
    static void Main(string[] args)
    {
        string[] move_names = { "U", "U2", "U'", "D", "D2", "D'", "L", "L2", "L'", "R", "R2", "R'", "F", "F2", "F'", "B", "B2", "B'" };

        var ep_move_dict = new Dictionary<int, int[]>()
    {
        {0,  new int[]{ 0, 1, 2, 3, 7, 4, 5, 6, 8, 9, 10, 11 } },
        {1,  new int[]{ 0, 1, 2, 3, 6, 7, 4, 5, 8, 9, 10, 11 } },
        {2,  new int[]{ 0, 1, 2, 3, 5, 6, 7, 4, 8, 9, 10, 11 } },
        {3,  new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 8 } },
        {4,  new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 10, 11, 8, 9 } },
        {5,  new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 11, 8, 9, 10 } },
        {6,  new int[]{ 11, 1, 2, 7, 4, 5, 6, 0, 8, 9, 10, 3 } },
        {7,  new int[]{ 3, 1, 2, 0, 4, 5, 6, 11, 8, 9, 10, 7 } },
        {8,  new int[]{ 7, 1, 2, 11, 4, 5, 6, 3, 8, 9, 10, 0 } },
        {9,  new int[]{ 0, 5, 9, 3, 4, 2, 6, 7, 8, 1, 10, 11 } },
        {10, new int[]{ 0, 2, 1, 3, 4, 9, 6, 7, 8, 5, 10, 11 } },
        {11, new int[]{ 0, 9, 5, 3, 4, 1, 6, 7, 8, 2, 10, 11 } },
        {12, new int[]{ 0, 1, 6, 10, 4, 5, 3, 7, 8, 9, 2, 11 } },
        {13, new int[]{ 0, 1, 3, 2, 4, 5, 10, 7, 8, 9, 6, 11 } },
        {14, new int[]{ 0, 1, 10, 6, 4, 5, 2, 7, 8, 9, 3, 11 } },
        {15, new int[]{ 4, 8, 2, 3, 1, 5, 6, 7, 0, 9, 10, 11 } },
        {16, new int[]{ 1, 0, 2, 3, 8, 5, 6, 7, 4, 9, 10, 11 } },
        {17, new int[]{ 8, 4, 2, 3, 0, 5, 6, 7, 1, 9, 10, 11 } }
    };

        var eo_move_dict = new Dictionary<int, int[]>()
    {
        {0,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {1,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {2,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {3,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {4,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {5,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {6,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {7,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {10, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {11, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {12, new int[]{ 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0 } },
        {13, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {14, new int[]{ 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0 } },
        {15, new int[]{ 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 } },
        {16, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        {17, new int[]{ 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 } }
    };

        int eo_to_index(int[] x)
        {
            int index = 0;
            foreach (int eo_i in x.Take(x.Length - 1))
            {
                index *= 2;
                index += eo_i;
            }
            return index;
        }

        int[] index_to_eo(int x)
        {
            int[] eo = new int[12];
            int sum_eo = 0;
            for (int i = 10; i > -1; i -= 1)
            {
                eo[i] = x % 2;
                x /= 2;
                sum_eo += eo[i];
            }
            eo[11] = (2 - sum_eo % 2) % 2;
            return eo;
        }

        int[][] eo_move_table = new int[2048][];

        foreach (int i in Enumerable.Range(0, 2048))
        {
            eo_move_table[i] = new int[18];
        }

        foreach (int i in Enumerable.Range(0, 2048))
        {
            int[] eo_before_move = index_to_eo(i);
            for (int move_num = 0; move_num < 18; move_num++)
            {
                //new_eo = [(self.eo[p] + move.eo[i]) % 2 for i, p in enumerate(move.ep)]
                int[] eo_after_move = ep_move_dict[move_num].Select((value, index) => (eo_before_move[value] + eo_move_dict[move_num][index]) % 2).ToArray();
                eo_move_table[i][move_num] = eo_to_index(eo_after_move);
            }
        }

        string jsonStr = JsonSerializer.Serialize(eo_move_table);

        using (var sw = new StreamWriter("eo_move_table.json", false, Encoding.UTF8))
        {
            sw.WriteLine(jsonStr);
        }
    }
}