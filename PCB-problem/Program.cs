using PCB_problem.solutionSearch;

namespace PCB_problem
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const string endpointsFilePath = "../../../../data.txt";
            const string parsedEndpointsFilePath = "../../../../parsed-data.json";
            var data = DataUtils.ReadDataFromFile(endpointsFilePath);
            DataUtils.ParseEndpointsDataForUi(data, ";", parsedEndpointsFilePath);
            var pcb = DataUtils.ConvertDataToPcb(data, ";");
            IPcbSolution solution = new RandomSearch(pcb);
            var paths = solution.FindBestIndividual(100);
            DataUtils.SaveIndividual(paths, "../../../../solution.json");
        }
    }
}