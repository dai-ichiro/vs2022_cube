using System.Text;
using System.Text.Json;

string[] move_names = { "U", "U2", "U'", "D", "D2", "D'", "L", "L2", "L'", "R", "R2", "R'", "F", "F2", "F'", "B", "B2", "B'" };

var move_dict = new Dictionary<int, int[]>()
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

int cp_to_index(int[] cp)
{
    int index = 0;
    for (int i = 0; i < 8; i++)
    {
        index *= 8 - i;
        for (int j = i + 1; j < 8; j++)
        {
            if (cp[i] > cp[j]) index += 1;
        }
    }
    return index;
}

int[] index_to_cp(int x)
{
    int[] cp = new int[8];
    for (int i = 6; i > -1; i -= 1)
    {
        cp[i] = x % (8 - i);
        x /= (8 - i);
        for (int j = i + 1; j < 8; j++)
        {
            if (cp[j] >= cp[i]) cp[j] += 1;
        }
    }
    return cp;
}

int[][] cp_move_table = new int[40320][];

foreach (int i in Enumerable.Range(0, 40320))
{
    cp_move_table[i] = new int[18];
}

foreach (int i in Enumerable.Range(0, 40320))
{
    int[] cp_before_move = index_to_cp(i);
    for (int move_num = 0; move_num < 18; move_num++)
    {
        int[] cp_after_move = move_dict[move_num].Select(x => cp_before_move[x]).ToArray();
        cp_move_table[i][move_num] = cp_to_index(cp_after_move);
    }
}

string jsonStr = JsonSerializer.Serialize(cp_move_table);

using (var writer = new StreamWriter("cp_move_table.json", false, Encoding.UTF8))
{
    writer.Write(jsonStr);
}