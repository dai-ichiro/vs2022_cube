using System.Text;
using System.Text.Json;

string jsonStr;

using (var sr = new StreamReader("./data/co_move_table.json", Encoding.UTF8))
{
    jsonStr = sr.ReadToEnd();
}

var co_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

using (var sr = new StreamReader("./data/eo_move_table.json", Encoding.UTF8))
{
    jsonStr = sr.ReadToEnd();
}

var eo_move_table = JsonSerializer.Deserialize<int[][]>(jsonStr);

Console.WriteLine("Computing co_eo_prune_table");

int co_count = co_move_table.GetLength(0);
int eo_count = eo_move_table.GetLength(0);

int[][] co_eo_prune_table = new int[co_count][];

foreach (int i in Enumerable.Range(0, co_count))
{
    co_eo_prune_table[i] = new int[eo_count];
}

foreach (int i in Enumerable.Range(0, co_count))
{
    foreach (int j in Enumerable.Range(0, eo_count))
    {
        co_eo_prune_table[i][j] = -1;
    }
}

co_eo_prune_table[0][0] = 0;
int distance = 0;
int num_filled = 1;

while (num_filled != co_count * eo_count)
{
    Console.WriteLine($"distance= {distance}");
    Console.WriteLine($"num_filled= {num_filled}");

    foreach (int i_co in Enumerable.Range(0, co_count))
    {
        foreach (int i_eo in Enumerable.Range(0, eo_count))
        {
            if (co_eo_prune_table[i_co][i_eo] == distance)
            {
                foreach (int i_move in Enumerable.Range(0, 18))
                {
                    int next_co = co_move_table[i_co][i_move];
                    int next_eo = eo_move_table[i_eo][i_move];
                    if (co_eo_prune_table[next_co][next_eo] == -1)
                    {
                        co_eo_prune_table[next_co][next_eo] = distance + 1;
                        num_filled += 1;
                    }
                }
            }
        }
    }

    distance += 1;
}

string new_jsonStr = JsonSerializer.Serialize(co_eo_prune_table);

using (var sw = new StreamWriter("co_eo_prune_table.json", false, Encoding.UTF8))
{
    sw.WriteLine(new_jsonStr);
}

