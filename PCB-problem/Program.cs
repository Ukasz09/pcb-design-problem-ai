using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using PCB_problem.solutionSearch;
using PCB_problem.solutionSearch.GeneticAlgorithm;

namespace PCB_problem
{
    internal static class Program
    {
        private static readonly int examinationRepeatQty = 10;

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

            InvestigateAffectOfPopulationSize(new[] {10, 50, 100});
            // InvestigateAffectOfPopulationSize(new[] {10, 50, 100, 500, 1000, 2000});


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
            const string outputFilePath = "../../../../populationSizeInvestigation.txt";
            const string contentHeader = "bestPenalty;avgPenalty;worstPenalty;avgExecTimeMs";
            File.WriteAllLines(outputFilePath, new[] {contentHeader});

            geneticAlgorithm = new GeneticAlgorithm(pcb, _w1, _w2, _w3, _w4, _w5);
            selectionOperator = new TournamentSelection(pcb, _tournamentSizePercent, _w1, _w2, _w3, _w4, _w5);
            crossoverOperator = new UniformCrossover(_crossoverProbability);
            mutationOperator = new MutationA(_mutationProbability);
            foreach (var t in populationSizes)
            {
                _populationSize = t;
                startedPopulation = GeneticAlgorithmUtils.GetStartedPopulation(pcb, _populationSize);
                new Thread(GeneticAlgorithmSolution).Start(outputFilePath);
            }
        }

        private static void GeneticAlgorithmSolution(object outputFilePath)
        {
            var outputFilePathTxt = (string) outputFilePath;
            var bestPenalties = new int[examinationRepeatQty];
            var execTimes = new long[examinationRepeatQty];
            for (var i = 0; i < examinationRepeatQty; i++)
            {
                var (_, penalty, execTime) = geneticAlgorithm.FindBestIndividual(
                    startedPopulation,
                    epochsQty,
                    selectionOperator,
                    crossoverOperator,
                    mutationOperator
                );
                bestPenalties[i] = penalty;
                execTimes[i] = execTime;
            }

            var worstPenalty = bestPenalties.Min();
            var avgPenalty = (int) bestPenalties.Average();
            var bestPenalty = bestPenalties.Max();
            var avgExecTimeMs = (int) execTimes.Average();
            SaveExaminationResult(outputFilePathTxt, worstPenalty, avgPenalty, bestPenalty, avgExecTimeMs);
        }

        private static void SaveExaminationResult(string outputFilePath, int worstPenalty, int avgPenalty,
            int bestPenalty, int avgExecTimeMs, char delimiter = ';')
        {
            var delimiterTxt = delimiter.ToString();
            var worstPenaltyTxt = worstPenalty.ToString();
            var avgPenaltyTxt = avgPenalty.ToString();
            var bestPenaltyTxt = bestPenalty.ToString();
            var avgExecTimeMsTxt = avgExecTimeMs.ToString();

            var resultLine =
                $"{worstPenaltyTxt}{delimiterTxt}{avgPenaltyTxt}{delimiterTxt}{bestPenaltyTxt}{delimiterTxt}{avgExecTimeMsTxt}";
            File.AppendAllLines(outputFilePath, new[] {resultLine});
        }
    }
}