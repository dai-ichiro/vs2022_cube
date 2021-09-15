using System.Text;
using System.Text.Json;

string jsonStr;

using (var sr = new StreamReader("./data/cp_move_table.json", Encoding.UTF8))
{
    jsonStr = sr.ReadToEnd();
}

var cp_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

using (var sr = new StreamReader("./data/eo_move_table.json", Encoding.UTF8))
{
    jsonStr = sr.ReadToEnd();
}

var eo_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

Console.WriteLine("Computing cp_eo_prune_table");

int cp_count = cp_move_table.GetLength(0);
int eo_count = eo_move_table.GetLength(0);

int[][] cp_eo_prune_table = new int[cp_count][];

foreach (int i in Enumerable.Range(0, cp_count))
{
    cp_eo_prune_table[i] = new int[eo_count];
}

foreach (int i in Enumerable.Range(0, cp_count))
{
    foreach (int j in Enumerable.Range(0, eo_count))
    {
        cp_eo_prune_table[i][j] = -1;
    }
}

cp_eo_prune_table[0][0] = 0;
int distance = 0;
int num_filled = 1;

while (num_filled != cp_count * eo_count)
{
    Console.WriteLine($"distance= {distance}");
    Console.WriteLine($"num_filled= {num_filled}");

    foreach (int i_cp in Enumerable.Range(0, cp_count))
    {
        foreach (int i_eo in Enumerable.Range(0, eo_count))
        {
            if (cp_eo_prune_table[i_cp][i_eo] == distance)
            {
                foreach (int i_move in Enumerable.Range(0, 18))
                {
                    int next_cp = cp_move_table[i_cp][i_move];
                    int next_eo = eo_move_table[i_eo][i_move];
                    if (cp_eo_prune_table[next_cp][next_eo] == -1)
                    {
                        cp_eo_prune_table[next_cp][next_eo] = distance + 1;
                        num_filled += 1;
                    }
                }
            }
        }
    }

    distance += 1;
}

string new_jsonStr = JsonSerializer.Serialize(cp_eo_prune_table);

using (var sw = new StreamWriter("cp_eo_prune_table.json", false, Encoding.UTF8))
{
    sw.WriteLine(new_jsonStr);
}
