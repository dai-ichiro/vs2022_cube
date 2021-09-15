using System.Text;
using System.Text.Json;

string jsonStr;

using (var sr = new StreamReader("./data/cp_move_table.json", Encoding.UTF8))
{
    jsonStr = sr.ReadToEnd();
}

var cp_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

using (var sr = new StreamReader("./data/co_move_table.json", Encoding.UTF8))
{
    jsonStr = sr.ReadToEnd();
}

var co_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

Console.WriteLine("Computing cp_co_prune_table");

int cp_count = cp_move_table.GetLength(0);
int co_count = co_move_table.GetLength(0);

int[][] cp_co_prune_table = new int[cp_count][];

foreach (int i in Enumerable.Range(0, cp_count))
{
    cp_co_prune_table[i] = new int[co_count];
}

foreach (int i in Enumerable.Range(0, cp_count))
{
    foreach (int j in Enumerable.Range(0, co_count))
    {
        cp_co_prune_table[i][j] = -1;
    }
}

cp_co_prune_table[0][0] = 0;
int distance = 0;
int num_filled = 1;

while (num_filled != cp_count * co_count)
{
    Console.WriteLine($"distance= {distance}");
    Console.WriteLine($"num_filled= {num_filled}");

    foreach (int i_cp in Enumerable.Range(0, cp_count))
    {
        foreach (int i_co in Enumerable.Range(0, co_count))
        {
            if (cp_co_prune_table[i_cp][i_co] == distance)
            {
                foreach (int i_move in Enumerable.Range(0, 18))
                {
                    int next_cp = cp_move_table[i_cp][i_move];
                    int next_co = co_move_table[i_co][i_move];
                    if (cp_co_prune_table[next_cp][next_co] == -1)
                    {
                        cp_co_prune_table[next_cp][next_co] = distance + 1;
                        num_filled += 1;
                    }
                }
            }
        }
    }

    distance += 1;
}

string new_jsonStr = JsonSerializer.Serialize(cp_co_prune_table);

using (var sw = new StreamWriter("cp_co_prune_table.json", false, Encoding.UTF8))
{
    sw.WriteLine(new_jsonStr);
}
