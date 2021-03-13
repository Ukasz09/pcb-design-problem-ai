using PCB_problem.solutionSearch;
using PCB_problem.solutionSearch.GeneticAlgorithm;

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
            
            // RAND
            // var solution = new RandomSearch(pcb);
            // var paths = solution.FindBestIndividual(100);

            // GA
            var solution = new GeneticAlgorithm(pcb, 8, 1, 1, 10, 10);
            var selectionMethod = new TournamentSelection(pcb, 4, 8, 1, 1, 10, 10); //1) tournament=7 //best=4, 8, 1, 1, 10, 10
            var crossover = new UniformCrossover(0.5);
            var paths = solution.FindBestIndividual(2500, 30, selectionMethod,crossover); //1) popSize=550, genQty=50, 2) popSize:2500, genQty=50 //best=2500, 30
            // best: time 54s
            DataUtils.SaveIndividual(paths, "../../../../solution.json");
        }
    }
}