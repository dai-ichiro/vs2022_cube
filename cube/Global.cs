﻿using System.Collections.Generic;
using System.Text.Json;

namespace cube
{
    public class Global
    {
        public static bool finished = false;

        public static readonly int[][] cp_move_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/cp_move_table.json"));
        public static readonly int[][] co_move_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/co_move_table.json"));
        public static readonly int[][] eo_move_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/eo_move_table.json"));

        public static readonly int[][] cp_co_prune_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/cp_co_prune_table.json"));
        public static readonly int[][] cp_eo_prune_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/cp_eo_prune_table.json"));
        public static readonly int[][] co_eo_prune_table = JsonSerializer.Deserialize<int[][]>(File_Operation.read_json("./data/co_eo_prune_table.json"));

        public static readonly string[] move_names = { "U", "U2", "U'", "D", "D2", "D'", "L", "L2", "L'", "R", "R2", "R'", "F", "F2", "F'", "B", "B2", "B'" };

        public static readonly Dictionary<int, int[]> ep_move_dict = new Dictionary<int, int[]>()
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
        public static readonly Dictionary<int, int> ep_dict = JsonSerializer.Deserialize<Dictionary<int, int>>(File_Operation.read_json("./data/ep_dict.json"));
    }
}