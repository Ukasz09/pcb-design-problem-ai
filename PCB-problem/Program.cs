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
            var solution = new GeneticAlgorithm(pcb, 30, 1, 1, 30, 30);
            var selectionOperator =
                new TournamentSelection(pcb, 4, 30, 1, 1, 30, 30); //best=4, 30, 1, 1, 30, 30
            var crossoverOperator = new UniformCrossover(0.5);
            var mutationOperator = new MutationA(0.20);
            var paths = solution.FindBestIndividual(2500, 50, selectionOperator, crossoverOperator,
                mutationOperator); // 2000, 30
            DataUtils.SaveIndividual(paths, "../../../../solution.json");
        }
    }
}