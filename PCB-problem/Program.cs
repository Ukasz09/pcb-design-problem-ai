using System;
using NLog;
using PCB_problem.solutionSearch;
using PCB_problem.solutionSearch.GeneticAlgorithm;

namespace PCB_problem
{
    internal static class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            var pcb = ReadAndParseInputData();
            // var solutionRandomSearch = RandomSearchSolution(pcb);
            var solutionGeneticAlgorithm = GeneticAlgorithmSolution(pcb);
            var outputFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "../../../..", "solution.json");
            DataUtils.SaveIndividual(solutionGeneticAlgorithm, outputFilePath);
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
            var (w1, w2, w3, w4, w5) = (30, 1, 1, 5, 5);
            const double tournamentSizePercent = 0.002;
            const double crossoverProbability = 0.5;
            const double mutationProbability = 0.4;
            const int populationSize = 2000;
            const int epochsQty = 25;
            _logger.Log(LogLevel.Info, "-------------------------");
            _logger.Log(LogLevel.Info, "--- Genetic Algorithm ---");
            _logger.Log(LogLevel.Info,
                $"w=({w1.ToString()}, {w2.ToString()}, {w3.ToString()}, {w4.ToString()}, {w5.ToString()})");
            _logger.Log(LogLevel.Info, $"tournament={tournamentSizePercent.ToString()}");
            _logger.Log(LogLevel.Info, $"crossoverProbability={crossoverProbability.ToString()}");
            _logger.Log(LogLevel.Info, $"mutationProbability={mutationProbability.ToString()}");
            _logger.Log(LogLevel.Info, $"populationSize={populationSize.ToString()}");
            _logger.Log(LogLevel.Info, $"epochsQty={epochsQty.ToString()}");

            var geneticAlgorithm = new GeneticAlgorithm(pcb, w1, w2, w3, w4, w5);
            var tournamentOperator = new TournamentSelection(pcb, tournamentSizePercent, w1, w2, w3, w4, w5);
            var rouletteOperator = new RouletteSelection(pcb, w1, w2, w3, w4, w5);
            var crossoverOperator = new UniformCrossover(crossoverProbability);
            var mutationOperator = new MutationA(mutationProbability);
            _logger.Log(LogLevel.Info, "Generating started population ...");
            var startedPopulation = GeneticAlgorithmUtils.GetStartedPopulation(pcb, populationSize);
            
            var bestIndividual = geneticAlgorithm.FindBestIndividual(startedPopulation, epochsQty, tournamentOperator,
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