using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            //Template: 0, 0, 0, 0, 0, 0
            List<List<int>> BoardList = new List<List<int>>();
            BoardList.Add(new List<int> { 0, 5, 0, 0, 0, 1 });
            BoardList.Add(new List<int> { 0, 0, 4, 6, 0, 0 });
            BoardList.Add(new List<int> { 4, 0, 0, 0, 5, 0 });
            BoardList.Add(new List<int> { 1, 0, 0, 0, 0, 4 });
            BoardList.Add(new List<int> { 0, 4, 3, 0, 0, 0 });
            BoardList.Add(new List<int> { 0, 6, 0, 2, 4, 0 });

            List<List<int>> Starts = new List<List<int>>();
            List<List<int>> Ends = new List<List<int>>();
            List<PossibleMembers> ValidMembers = new List<PossibleMembers>();
            List<int> Members = new List<int> { 1, 2, 3, 4, 5, 6 };
            int StartIndex = 0;
            int EndIndex = 0;
            int Steps = 0;

            for (int i = 0; i < BoardList[0].Count; i++)
            {
                Steps++;
                for (int j = 0; j < BoardList[0].Count; j++)
                {
                    Steps++;
                    if (i % 2 == 0 && j % 3 == 0)
                    {
                        Steps++;
                        Starts.Add(new List<int>());
                        Starts[StartIndex].Add(i);
                        Starts[StartIndex].Add(j);
                        StartIndex++;
                    }
                    
                    else if (i % 2 == 1 && j % 3 == 2)
                    {
                        Steps++;
                        Ends.Add(new List<int>());
                        Ends[EndIndex].Add(i);
                        Ends[EndIndex].Add(j);
                        EndIndex++;
                    }
                }
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            SolveSudoku(BoardList, ValidMembers, Members, Starts, Ends, Steps);
            sw.Stop();
            Console.WriteLine($"Time taken: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("Solution:");
            Console.WriteLine();
            foreach (var i in BoardList)
            {
                foreach (var j in i)
                {
                    Console.Write($"{j} ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void SolveSudoku(List<List<int>> BoardList, List<PossibleMembers> ValidMembers, List<int> Members, List<List<int>> Starts, List<List<int>> Ends, int Steps)
        {
            Steps++;
            ValidMembers.Clear();

            for (int i = 0; i < BoardList[0].Count; i++)
            {
                Steps++;
                for (int j = 0; j < BoardList[0].Count; j++)
                {
                    Steps++;
                    if (BoardList[i][j] == 0)
                    {
                        Steps++;
                        ValidMembers.Add(new PossibleMembers
                        {
                            x = i,
                            y = j,
                            members = new List<int>()
                        });
                    }
                }
            }

            foreach (var Item in ValidMembers)
            {
                Steps++;
                foreach (var Member in Members)
                {
                    Steps++;
                    if (!BoardList[Item.x].Contains(Member))
                    {
                        Steps++;
                        Item.members.Add(Member);
                    }
                }
            }

            foreach (var Item in ValidMembers)
            {
                Steps++;
                foreach (var Member in Members)
                {
                    Steps++;
                    for (int i = 0; i < 6; i++)
                    {
                        Steps++;
                        if (BoardList[i][Item.y] == Member)
                        {
                            Item.members.Remove(Member);
                        }
                    }
                }
            }

            for (int i = 0; i < Starts.Count; i++)
            {
                Steps++;
                List<PossibleMembers> TempList = new List<PossibleMembers>();

                for (int j = Starts[i][0]; j < Ends[i][0] + 1; j++)
                {
                    Steps++;
                    for (int k = Starts[i][1]; k < Ends[i][1] + 1; k++)
                    {
                        Steps++;
                        if (BoardList[j][k] == 0)
                        {
                            Steps++;
                            TempList.Add(ValidMembers.Find(Item => Item.x == j && Item.y == k));
                        }
                    }
                }

                foreach (var Temp in TempList)
                {
                    Steps++;
                    for (int j = Starts[i][0]; j < Ends[i][0] + 1; j++)
                    {
                        Steps++;
                        for (int k = Starts[i][1]; k < Ends[i][1] + 1; k++)
                        {
                            Steps++;
                            if (BoardList[j][k] != 0)
                            {
                                Steps++;
                                if (Temp.members.Contains(BoardList[j][k]))
                                {
                                    Steps++;
                                    Temp.members.Remove(BoardList[j][k]);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var Item in ValidMembers)
            {
                Steps++;
                if (Item.members.Count == 1)
                {
                    Steps++;
                    BoardList[Item.x][Item.y] = Item.members[0];
                }
            }

            RemoveMembers(ValidMembers);
            Steps++;

            if (ValidMembers.Count > 0)
            {
                Steps++;
                SolveSudoku(BoardList, ValidMembers, Members, Starts, Ends, Steps);
            }

            else if (ValidMembers.Count == 0)
            {
                Console.WriteLine($"No of steps taken: {Steps}");
            }
           
        }

        private static void RemoveMembers(List<PossibleMembers> ValidMembers)
        {
            foreach (PossibleMembers Item in ValidMembers.ToArray())
            {
                if (Item.members.Count == 1)
                {
                    ValidMembers.Remove(Item);
                }
            }
        }
    }

    class PossibleMembers
    {
        public int x;
        public int y;
        public List<int> members;
    }
}