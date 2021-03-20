using System.Collections.Generic;
using System.Threading;
using PCB_problem.solutionSearch;
using PCB_problem.solutionSearch.GeneticAlgorithm;

namespace PCB_problem
{
    internal static class Program
    {
        private static int _populationSize = 100;
        private static int _w1 = 40;
        private static int _w2 = 1;
        private static int _w3 = 2;
        private static int _w4 = 30;
        private static int _w5 = 30;
        private static double _tournamentSizePercent = 0.002;
        private static double _crossoverProbability = 0.5;
        private static double _mutationProbability = 0.1;
        private static int epochsQty = 30;

        private static Pcb pcb;
        private static Population startedPopulation;

        private static ISelection selectionOperator;
        private static GeneticAlgorithm geneticAlgorithm;
        private static ICrossover crossoverOperator;
        private static IMutation mutationOperator;

        private static void Main(string[] args)
        {
            pcb = ReadPcbData();

            InvestigateAffectOfPopulationSize(new[] {10, 50, 100, 500, 1000, 2000});


            // var outputFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "../../../..", "solution.json");
            // DataUtils.SaveIndividual(solutionGeneticAlgorithm, outputFilePath);
        }

        private static Pcb ReadPcbData()
        {
            const string inputDataFilePath = "../../../../data.txt";
            const string dataDelimiter = ";";
            var inputDataText = DataUtils.ReadDataFromFile(inputDataFilePath);
            return DataUtils.ConvertDataToPcb(inputDataText, dataDelimiter);
        }

        private static void InvestigateAffectOfPopulationSize(IEnumerable<int> populationSizes)
        {
            geneticAlgorithm = new GeneticAlgorithm(pcb, _w1, _w2, _w3, _w4, _w5);
            selectionOperator = new TournamentSelection(pcb, _tournamentSizePercent, _w1, _w2, _w3, _w4, _w5);
            crossoverOperator = new UniformCrossover(_crossoverProbability);
            mutationOperator = new MutationA(_mutationProbability);
            foreach (var size in populationSizes)
            {
                _populationSize = size;
                startedPopulation = GeneticAlgorithmUtils.GetStartedPopulation(pcb, _populationSize);
                new Thread(GeneticAlgorithmSolution).Start();
            }
        }

        private static void GeneticAlgorithmSolution()
        {
            // return 
            geneticAlgorithm.FindBestIndividual(
                startedPopulation,
                epochsQty,
                selectionOperator,
                crossoverOperator,
                mutationOperator
            );
        }

        private static Individual RandomSearchSolution(Pcb pcb)
        {
            const int attemptsQty = 100;
            var (w1, w2, w3, w4, w5) = (40, 1, 2, 30, 30);
            var randomSearch = new RandomSearch(pcb);
            var bestIndividual = randomSearch.FindBestIndividual(attemptsQty, w1, w2, w3, w4, w5);
            return bestIndividual;
        }
    }
}