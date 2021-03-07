using System;
using PCB_problem.solutionSearch;

namespace PCB_problem
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filepath = "../../../../data.txt";
            var data = DataUtils.ReadDataFromFile(filepath);
            var pcb = DataUtils.ConvertDataToPcb(data, ";");
            IPcbSolution solution = new RandomSearch();
            var paths = solution.FindSolution(pcb);
            DataUtils.SaveSolution(paths, "../../../../solution.txt");
        }
    }
}