using PCB_problem.solutionSearch;

namespace PCB_problem
{
    class Program
    {
        static void Main(string[] args)
        {
            const string endpointsFilePath = "../../../../data.txt";
            const string parsedEndpointsFilePath = "../../../../parsed-data.json";
            var data = DataUtils.ReadDataFromFile(endpointsFilePath);
            DataUtils.ParseEndpointsDataForUi(data, ";", parsedEndpointsFilePath);
            var pcb = DataUtils.ConvertDataToPcb(data, ";");
            IPcbSolution solution = new RandomSearch();
            var paths = solution.FindBestSolution(pcb,100000);
            DataUtils.SaveSolution(paths, "../../../../solution.json");
        }
    }
}