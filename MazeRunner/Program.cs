using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintWelcomeMessage();
            string p = GetFileNameFromUser();
            MazeRunner mr = new MazeRunner();
            mr.ReadInMazeFile(p);
            PrintMaze(mr);
            mr.FindGoal();
            PrintMaze(mr);
        }

        private static void PrintMaze(MazeRunner mr)
        {
            Console.WriteLine(mr.ToString());
        }

        static string GetFileNameFromUser()
        {
            Console.WriteLine("Please give me the location of the Maze File");
            string path = Console.ReadLine();
            return path;
        }

        static void PrintWelcomeMessage()
        {
            Console.WriteLine("Hello Welcome to MazeRunner");
        }


    }
}
