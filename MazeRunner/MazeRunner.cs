using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner
{
    class MazeRunner
    {
        private char[][] MazeChars;
        private int Height, Width;
        private Location Start;
        private Location Goal;

        private MyQueue<Location> UnvisitedQueue = new MyQueue<Location>();


        public MazeRunner()
        {

        }

        public void WriteOutMazeFile(string path)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create, FileAccess.Write)))
            {
                sw.Write($"{ToString()}");
                sw.Close();
            }
        }

        public void ReadInMazeFile(string path)
        {
            using (StreamReader sr = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read)))
            {
                string io = sr.ReadToEnd();
                string[] lines = io.Split('\n');

                string[] firstLine = lines[0].Split(' ');
                Width = int.Parse(firstLine[0]);
                Height = int.Parse(firstLine[1]);
                List<char[]> maze = new List<char[]>();

                for (int i = 1; i < lines.Length;i++)
                {
                    maze.Add(lines[i].ToCharArray());
                }

                MazeChars = maze.ToArray();
                Start = FindLocation('S');
                Goal = FindLocation('G');
            }
        }

        public override string ToString()
        {
            StringBuilder x = new StringBuilder();

            foreach (char[] s in MazeChars)
            {
                foreach (char c in s)
                {
                    x.Append($"{c}");
                }
                x.Append("\n");
            }

            return x.ToString();
        }

        public Location FindLocation(char c)
        {
            for (int i = 0; i < MazeChars.Length; i++)
            {
                for (int j = 0; j < MazeChars[i].Length; j++)
                {
                    if (MazeChars[i][j] == c)
                    {
                        return new Location(){X = i, Y = j};
                    }
                }
            }
            return null;
        }

        public bool FindGoal()
        {
            Location Current;
            UnvisitedQueue.Enqueue(Start);

            bool Victory = false;

            while (UnvisitedQueue.Count > 0 && Victory == false)
            {
                Current = UnvisitedQueue.Dequeue();
                //if (Current == null)
                //{
                //    break;
                //}
                MazeChars[Current.X][Current.Y] = 'V';
                if (AddNorth(Current))
                {
                    Victory = true;
                    break;
                }
                if (AddEast(Current))
                {
                    Victory = true;
                    break;
                }
                if (AddSouth(Current))
                {
                    Victory = true;
                    break;
                }
                if (AddWest(Current))
                {
                    Victory = true;
                    break;
                }
            }

            return Victory;
        }

        private bool AddWest(Location current)
        {
            if (current.X < MazeChars[current.X].Length - 1)
            {
                if (MazeChars[current.X + 1][current.Y] == 'G')
                {
                    return true;
                }
                if (MazeChars[current.X + 1][current.Y] == '.')
                {
                    UnvisitedQueue.Enqueue(new Location() {X = current.X + 1, Y = current.Y});
                }
            }

            return false;
        }

        private bool AddSouth(Location current)
        {
            if (current.Y < MazeChars.Length - 1)
            {
                if (MazeChars[current.X][current.Y + 1] == 'G')
                {
                    return true;
                }
                if (MazeChars[current.X][current.Y + 1] == '.')
                {
                    UnvisitedQueue.Enqueue(new Location() {X = current.X, Y = current.Y + 1});
                }
            }

            return false;
        }

        private bool AddEast(Location current)
        {
            if (current.X > 0)
            {
                if (MazeChars[current.X - 1][current.Y] == 'G')
                {
                    return true;
                }
                if (MazeChars[current.X - 1][current.Y] == '.')
                {
                    UnvisitedQueue.Enqueue(new Location(){X = current.X -1, Y = current.Y});
                }
            }
            return false;
        }

        private bool AddNorth(Location current)
        {
            if (current.Y > 0)
            {
                if (MazeChars[current.X][current.Y - 1] == 'G')
                {
                    return true;
                }
                if (MazeChars[current.X][current.Y - 1] == '.')
                {
                    UnvisitedQueue.Enqueue(new Location(){X = current.X, Y = current.Y - 1});
                }
            }

            return false;
        }
    }

    internal class Location
    {
        internal int X;
        internal int Y;
    }

    internal class MyQueue<T>
    {
        internal class Node
        {
            internal T Value;
            internal Node Next;


            /*
             *
             *   |T|-->|T|-->|T|
             *    ^           ^
             *    |           |
             *  Front        Back
             *
             * We add to the Back and remove from the Front
             *
             *
             * First Case:
             * Empty Queue
             * Count = 0
             *
             *  null <----
             *    ^      |
             *    |      |
             *  Front   Back
             *
             *
             *
             * Second Case:
             * One Item in the Queue
             * Count = 1
             *
             *   |T|-->null
             *    ^
             *    |-----|
             *  Front  Back
             *
             *
             *
             * Thrid Case
             * Many Items in the Queue
             *
             *   |T|-->|T|-->...-->|T|-->|T|-->null
             *    ^                       ^
             *    |                       |
             *  Front                    Back
             *
             *
             */


            internal Node(T value)
            {
                Value = value;
            }
        }

        private Node Front;
        private Node Back;
        public int Count;

        public void Enqueue(T value)
        {
            //Front doesn't move
            //Back moves to the new node

            Node t = new Node(value);

            if (Count == 0)
            {
                Front = t;
                Back = t;
                Count++;
                return;
            }
            else
            {
                Back.Next = t;
                Back = t;
                Count++;
                return;
            }



            //if (Front == null)
            //{
            //    Front = new Node(value);
            //    Back = Front;

            //}
            //else
            //{
            //    Node t = new Node(value) {Next = Back};
            //    Back = t;
            //}

            //Count++;
        }

        public T Dequeue()
        {
            //Front moves to the next Node in line
            //Back doesn't move unless back and front point to the same Node

            if (Count == 0)
            {
                return default(T);
            }
            else
            {
                T x = Front.Value;
                Front = Front.Next;
                Count--;
                if (Count == 0)
                {
                    Back = null;
                }
                return x;
            }

        }

    }
}
