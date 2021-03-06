using System;

namespace PCB_problem
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filepath = "../../../../data.txt";
            var data = DataUtils.ReadDataFromFile(filepath);
            var pcb = DataUtils.ConvertDataToPcb(data, ";");
            Console.WriteLine(pcb);
        }
    }
}