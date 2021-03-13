using PCB_problem.solutionSearch;
using PCB_problem.solutionSearch.GeneticAlgorithm;

namespace PCB_problem
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var pcb = ReadAndParseInputData();

            // var solutionRandomSearch = RandomSearchSolution(pcb);
            var solutionGeneticAlgorithm = GeneticAlgorithmSolution(pcb);

            DataUtils.SaveIndividual(solutionGeneticAlgorithm, "../../../../solution.json");
        }


        private static Pcb ReadAndParseInputData()
        {
            const string inputDataFilePath = "../../../../data.txt";
            const string parsedInputDataFilePath = "../../../../parsed-data.json";
            const string dataDelimiter = ";";

            var inputDataText = DataUtils.ReadDataFromFile(inputDataFilePath);
            DataUtils.ParseEndpointsDataForUi(inputDataText, dataDelimiter, parsedInputDataFilePath);
            var pcb = DataUtils.ConvertDataToPcb(inputDataText, dataDelimiter);
            return pcb;
        }

        // Best: 
        // tournamentSize = 4
        // w1, w2, w3, w4, w5 = 30, 1, 1, 30, 30
        // crossoverProbability = 0.5
        // mutationProbability = 0.2
        // populationSize = 2000
        // epochsQty = 30
        private static Individual GeneticAlgorithmSolution(Pcb pcb)
        {
            var (w1, w2, w3, w4, w5) = (15, 1, 1, 5,5);
            const int tournamentSize = 4;
            const double crossoverProbability = 0.35;
            const double mutationProbability = 0.25;
            const int populationSize = 2750;
            const int epochsQty = 25;

            var geneticAlgorithm = new GeneticAlgorithm(pcb, w1, w2, w3, w4, w5);
            var tournamentOperator = new TournamentSelection(pcb, tournamentSize, w1, w2, w3, w4, w5);
            var rouletteOperator = new RouletteSelection(pcb, w1, w2, w3, w4, w5);
            var crossoverOperator = new UniformCrossover(crossoverProbability);
            var mutationOperator = new MutationA(mutationProbability);
            var bestIndividual = geneticAlgorithm.FindBestIndividual(populationSize, epochsQty, tournamentOperator,
                crossoverOperator,
                mutationOperator);
            return bestIndividual;
        }

        private static Individual RandomSearchSolution(Pcb pcb)
        {
            const int attemptsQty = 100;

            var randomSearch = new RandomSearch(pcb);
            var bestIndividual = randomSearch.FindBestIndividual(attemptsQty);
            return bestIndividual;
        }
    }
}