using System.Text;
using System.Text.Json;

string[] move_names = { "U", "U2", "U'", "D", "D2", "D'", "L", "L2", "L'", "R", "R2", "R'", "F", "F2", "F'", "B", "B2", "B'" };

var cp_move_dict = new Dictionary<int, int[]>()
    {
        {0,  new int[]{ 3, 0, 1, 2, 4, 5, 6, 7} },
        {1,  new int[]{ 2, 3, 0, 1, 4, 5, 6, 7} },
        {2,  new int[]{ 1, 2, 3, 0, 4, 5, 6, 7} },
        {3,  new int[]{ 0, 1, 2, 3, 5, 6, 7, 4} },
        {4,  new int[]{ 0, 1, 2, 3, 6, 7, 4, 5} },
        {5,  new int[]{ 0, 1, 2, 3, 7, 4, 5, 6} },
        {6,  new int[]{ 4, 1, 2, 0, 7, 5, 6, 3} },
        {7,  new int[]{ 7, 1, 2, 4, 3, 5, 6, 0} },
        {8,  new int[]{ 3, 1, 2, 7, 0, 5, 6, 4} },
        {9,  new int[]{ 0, 2, 6, 3, 4, 1, 5, 7} },
        {10, new int[]{ 0, 6, 5, 3, 4, 2, 1, 7} },
        {11, new int[]{ 0, 5, 1, 3, 4, 6, 2, 7} },
        {12, new int[]{ 0, 1, 3, 7, 4, 5, 2, 6} },
        {13, new int[]{ 0, 1, 7, 6, 4, 5, 3, 2} },
        {14, new int[]{ 0, 1, 6, 2, 4, 5, 7, 3} },
        {15, new int[]{ 1, 5, 2, 3, 0, 4, 6, 7} },
        {16, new int[]{ 5, 4, 2, 3, 1, 0, 6, 7} },
        {17, new int[]{ 4, 0, 2, 3, 5, 1, 6, 7} }
    };

var co_move_dict = new Dictionary<int, int[]>()
    {
        {0,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {1,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {2,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {3,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {4,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {5,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {6,  new int[]{ 2, 0, 0, 1, 1, 0, 0, 2} },
        {7,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {8,  new int[]{ 2, 0, 0, 1, 1, 0, 0, 2} },
        {9,  new int[]{ 0, 1, 2, 0, 0, 2, 1, 0} },
        {10, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {11, new int[]{ 0, 1, 2, 0, 0, 2, 1, 0} },
        {12, new int[]{ 0, 0, 1, 2, 0, 0, 2, 1} },
        {13, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {14, new int[]{ 0, 0, 1, 2, 0, 0, 2, 1} },
        {15, new int[]{ 1, 2, 0, 0, 2, 1, 0, 0} },
        {16, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0} },
        {17, new int[]{ 1, 2, 0, 0, 2, 1, 0, 0} }
    };

int co_to_index(int[] x)
{
    int index = 0;
    foreach (int co_i in x.Take(x.Length - 1))
    {
        index *= 3;
        index += co_i;
    }
    return index;
}

int[] index_to_co(int x)
{
    int[] co = new int[8];
    int sum_co = 0;
    for (int i = 6; i > -1; i -= 1)
    {
        co[i] = x % 3;
        x /= 3;
        sum_co += co[i];
    }
    co[7] = (3 - sum_co % 3) % 3;
    return co;
}

int[][] co_move_table = new int[2187][];

foreach (int i in Enumerable.Range(0, 2187))
{
    co_move_table[i] = new int[18];
}

foreach (int i in Enumerable.Range(0, 2187))
{
    int[] co_before_move = index_to_co(i);
    for (int move_num = 0; move_num < 18; move_num++)
    {
        int[] co_after_move = cp_move_dict[move_num].Select((value, index) => (co_before_move[value] + co_move_dict[move_num][index]) % 3).ToArray();
        co_move_table[i][move_num] = co_to_index(co_after_move);
    }
}

string jsonStr = JsonSerializer.Serialize(co_move_table);

using (var sw = new StreamWriter("co_move_table.json", false, Encoding.UTF8))
{
    sw.WriteLine(jsonStr);
}