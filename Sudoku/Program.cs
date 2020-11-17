using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Template: 0, 0, 0, 0, 0, 0, 0, 0, 0
            List<List<int>> BoardList = new List<List<int>>();
            BoardList.Add(new List<int> { 0, 4, 0, 0, 7, 0, 0, 0, 0 });
            BoardList.Add(new List<int> { 0, 0, 1, 0, 0, 8, 0, 7, 0 });
            BoardList.Add(new List<int> { 0, 2, 7, 0, 3, 0, 5, 0, 9 });
            BoardList.Add(new List<int> { 0, 0, 6, 0, 0, 0, 0, 0, 0 });
            BoardList.Add(new List<int> { 7, 0, 0, 0, 6, 9, 0, 0, 0 });
            BoardList.Add(new List<int> { 0, 0, 0, 0, 0, 4, 3, 0, 0 });
            BoardList.Add(new List<int> { 3, 7, 0, 0, 1, 0, 0, 2, 0 });
            BoardList.Add(new List<int> { 0, 8, 0, 4, 0, 2, 0, 6, 0 });
            BoardList.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 5 });

            List<List<int>> Starts = new List<List<int>>();
            List<List<int>> Ends = new List<List<int>>();
            List<PossibleMembers> ValidMembers = new List<PossibleMembers>();
            List<int> Members = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int StartIndex = 0;
            int EndIndex = 0;

            for (int i = 0; i < BoardList[0].Count; i++)
            {
                for (int j = 0; j < BoardList[0].Count; j++)
                {
                    if (i % 3 == 0 && j % 3 == 0)
                    {
                        Starts.Add(new List<int>());
                        Starts[StartIndex].Add(i);
                        Starts[StartIndex].Add(j);
                        StartIndex++;
                    }
                    
                    else if (i % 3 == 2 && j % 3 == 2)
                    {
                        Ends.Add(new List<int>());
                        Ends[EndIndex].Add(i);
                        Ends[EndIndex].Add(j);
                        EndIndex++;
                    }
                }
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            await SolveSudoku(BoardList, ValidMembers, Members, Starts, Ends);
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

        private static async Task SolveSudoku(List<List<int>> BoardList, List<PossibleMembers> ValidMembers, List<int> Members, List<List<int>> Starts, List<List<int>> Ends)
        {
            ValidMembers.Clear();

            for (int i = 0; i < BoardList[0].Count; i++)
            {
                for (int j = 0; j < BoardList[0].Count; j++)
                {
                    if (BoardList[i][j] == 0)
                    {
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
                foreach (var Member in Members)
                {
                    if (!BoardList[Item.x].Contains(Member))
                    {
                        Item.members.Add(Member);
                    }
                }
            }

            foreach (var Item in ValidMembers)
            {
                foreach (var Member in Members)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (BoardList[i][Item.y] == Member)
                        {
                            Item.members.Remove(Member);
                        }
                    }
                }
            }

            for (int i = 0; i < Starts.Count; i++)
            {
                List<PossibleMembers> TempList = new List<PossibleMembers>();

                for (int j = Starts[i][0]; j < Ends[i][0] + 1; j++)
                {
                    for (int k = Starts[i][1]; k < Ends[i][1] + 1; k++)
                    {
                        if (BoardList[j][k] == 0)
                        {
                            TempList.Add(ValidMembers.Find(Item => Item.x == j && Item.y == k));
                        }
                    }
                }

                foreach (var Temp in TempList)
                {
                    for (int j = Starts[i][0]; j < Ends[i][0] + 1; j++)
                    {
                        for (int k = Starts[i][1]; k < Ends[i][1] + 1; k++)
                        {
                            if (BoardList[j][k] != 0)
                            {
                                if (Temp.members.Contains(BoardList[j][k]))
                                {
                                    Temp.members.Remove(BoardList[j][k]);
                                }
                            }
                        }
                    }
                }
            }

            List<List<PossibleMembers>> Colbased = new List<List<PossibleMembers>>();
            List<List<PossibleMembers>> Rowbased = new List<List<PossibleMembers>>();
            List<List<PossibleMembers>> GridBased = new List<List<PossibleMembers>>();

            for (int i = 0; i < 9; i++)
            {
                Colbased.Add(new List<PossibleMembers>());
                Rowbased.Add(new List<PossibleMembers>());

                for (int j = 0; j < 9; j++)
                {
                    foreach (var ValidMember in ValidMembers)
                    {
                        if (ValidMember.x == i && ValidMember.y == j)
                        {
                            Colbased[i].Add(ValidMember);
                        }

                        if (ValidMember.x == j && ValidMember.y == i)
                        {
                            Rowbased[i].Add(ValidMember);
                        }
                    }
                }
            }

            for (int i = 0; i < Starts.Count; i++)
            {
                GridBased.Add(new List<PossibleMembers>());

                for (int j = Starts[i][0]; j < Ends[i][0] + 1; j++)
                {
                    for (int k = Starts[i][1]; k < Ends[i][1] + 1; k++)
                    {
                        foreach (var ValidMember in ValidMembers)
                        {
                            if (ValidMember.x == j && ValidMember.y == k)
                            {
                                GridBased[i].Add(ValidMember);
                            }
                        }
                    }
                }
            }

            List<int> AllMembers = new List<int>();

            foreach (var MemberList in Colbased)
            {
                foreach (var Member in MemberList.ToArray())
                {
                    foreach (var member in Member.members)
                    {
                        AllMembers.Add(member);
                    }
                }

                foreach (var Member in MemberList.ToArray())
                {
                    foreach (var member in Member.members.ToArray())
                    {
                        int count = AllMembers.Where(temp => temp.Equals(member))
                                    .Select(temp => temp)
                                    .Count();

                        if (count == 1)
                        {
                            var find = Member.members.FindAll(item => item != member);
                            
                            foreach (var item in find)
                            {
                                Member.members.Remove(item);
                            }
                        }
                    }
                }

                AllMembers.Clear();
            }

            foreach (var MemberList in Rowbased)
            {
                foreach (var Member in MemberList)
                {
                    foreach (var member in Member.members)
                    {
                        AllMembers.Add(member);
                    }
                }

                foreach (var Member in MemberList.ToArray())
                {
                    foreach (var member in Member.members.ToArray())
                    {
                        int count = AllMembers.Where(temp => temp.Equals(member))
                                    .Select(temp => temp)
                                    .Count();

                        if (count == 1)
                        {
                            var find = Member.members.FindAll(item => item != member);

                            foreach (var item in find)
                            {
                                Member.members.Remove(item);
                            }
                        }
                    }
                }

                AllMembers.Clear();
            }

            foreach (var MemberList in GridBased)
            {
                foreach (var Member in MemberList)
                {
                    foreach (var member in Member.members)
                    {
                        AllMembers.Add(member);
                    }
                }

                foreach (var Member in MemberList.ToArray())
                {
                    foreach (var member in Member.members.ToArray())
                    {
                        int count = AllMembers.Where(temp => temp.Equals(member))
                                    .Select(temp => temp)
                                    .Count();

                        if (count == 1)
                        {
                            var find = Member.members.FindAll(item => item != member);

                            foreach (var item in find)
                            {
                                Member.members.Remove(item);
                            }
                        }
                    }
                }

                AllMembers.Clear();
            }

            foreach (var item in ValidMembers)
            {
                if (item.members.Count == 1)
                {
                    BoardList[item.x][item.y] = item.members[0];
                }
            }

            RemoveMembers(ValidMembers);

            RemoveMemberList(Colbased);
            RemoveMemberList(Rowbased);
            RemoveMemberList(GridBased);

            List<int> Matchingmembers = new List<int>();
            List<PossibleMembers> Matching = new List<PossibleMembers>();

            foreach (var MemberList in Colbased)
            {
                foreach (var Member in MemberList)
                {
                    if (Member.members.Count == 2)
                    {
                        foreach (var TwoCountMembers in MemberList)
                        {
                            int index = 0;

                            if (TwoCountMembers.y != Member.y)
                            {
                                if (!Enumerable.SequenceEqual(Member.members, TwoCountMembers.members))
                                {
                                    index++;
                                }

                                if (index == 0)
                                {
                                    Matchingmembers.AddRange(Member.members);
                                    Matching.Add(Member);
                                }
                            }
                        }
                    }
                }

                if (Matching.Count == 2)
                {
                    foreach (var Member in MemberList)
                    {
                        if (!Matching.Contains(Member))
                        {
                            foreach (var member in Member.members.ToArray())
                            {
                                if (Matchingmembers.Contains(member))
                                {
                                    Member.members.Remove(member);
                                }
                            }
                        }
                    }
                }

                Matchingmembers.Clear();
                Matching.Clear();
            }

            foreach (var MemberList in Rowbased)
            {
                foreach (var Member in MemberList)
                {
                    if (Member.members.Count == 2)
                    {
                        foreach (var TwoCountMembers in MemberList)
                        {
                            int index = 0;

                            if (TwoCountMembers.x != Member.x)
                            {
                                if (!Enumerable.SequenceEqual(Member.members, TwoCountMembers.members))
                                {
                                    index++;
                                }

                                if (index == 0)
                                {
                                    Matchingmembers.AddRange(Member.members);
                                    Matching.Add(Member);
                                }
                            }
                        }
                    }
                }

                if (Matching.Count == 2)
                {
                    foreach (var Member in MemberList)
                    {
                        if (!Matching.Contains(Member))
                        {
                            foreach (var member in Member.members.ToArray())
                            {
                                if (Matchingmembers.Contains(member))
                                {
                                    Member.members.Remove(member);
                                }
                            }
                        }
                    }
                }

                Matchingmembers.Clear();
                Matching.Clear();
            }

            foreach (var MemberList in GridBased)
            {
                foreach (var Member in MemberList)
                {
                    if (Member.members.Count == 2)
                    {
                        foreach (var TwoCountMembers in MemberList)
                        {
                            int index = 0;

                            if (TwoCountMembers.y != Member.y && TwoCountMembers.x != Member.x)
                            {
                                if (!Enumerable.SequenceEqual(Member.members, TwoCountMembers.members))
                                {
                                    index++;
                                }

                                if (index == 0)
                                {
                                    Matchingmembers.AddRange(Member.members);
                                    Matching.Add(Member);
                                }
                            }
                        }
                    }
                }

                if (Matching.Count == 2)
                {
                    foreach (var Member in MemberList)
                    {
                        if (!Matching.Contains(Member))
                        {
                            foreach (var member in Member.members.ToArray())
                            {
                                if (Matchingmembers.Contains(member))
                                {
                                    Member.members.Remove(member);
                                }
                            }
                        }
                    }
                }
                
                Matchingmembers.Clear();
                Matching.Clear();
            }

            foreach (var Item in ValidMembers)
            {
                if (Item.members.Count == 1)
                {
                    BoardList[Item.x][Item.y] = Item.members[0];
                }
            }

            RemoveMembers(ValidMembers);

            if (ValidMembers.Count == 10)
            {
                Console.WriteLine("Done");
            }

            else if (ValidMembers.Count > 0)
            {
                await SolveSudoku(BoardList, ValidMembers, Members, Starts, Ends);
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

        private static void RemoveMemberList(List<List<PossibleMembers>> ValidMembers)
        {
            foreach (var MemberList in ValidMembers)
            {
                foreach (var Member in MemberList.ToArray())
                {
                    if (Member.members.Count == 1)
                    {
                        MemberList.Remove(Member);
                    }
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