using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using PCB_problem.solutionSearch.GeneticAlgorithm;

namespace PCB_problem
{
    internal static class Program
    {
        private const int examinationRepeatQty = 10;
        private const string resultsDirectoryName = "examination-results";

        private static int _populationSize = 1500;
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

            InvestigateAffectOfPopulationSize(new[] {10, 50});
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
            var outputFilePathPrefix = $"../../../../{resultsDirectoryName}/population-size-investigation";
            const string outputFilePathExtension = ".csv";
           
            geneticAlgorithm = new GeneticAlgorithm(pcb, _w1, _w2, _w3, _w4, _w5);
            selectionOperator = new TournamentSelection(pcb, _tournamentSizePercent, _w1, _w2, _w3, _w4, _w5);
            crossoverOperator = new UniformCrossover(_crossoverProbability);
            mutationOperator = new MutationA(_mutationProbability);
            foreach (var t in populationSizes)
            {
                _populationSize = t;
                var outputFilePath = $"{outputFilePathPrefix}-{_populationSize.ToString()}{outputFilePathExtension}";
                const string contentHeader = "epochNo;bestPenalty;avgPenalty;worstPenalty;avgExecTimeMs";
                File.WriteAllLines(outputFilePath, new[] {contentHeader});
                
                startedPopulation = GeneticAlgorithmUtils.GetStartedPopulation(pcb, _populationSize);
                new Thread(GeneticAlgorithmSolution).Start(outputFilePath);
            }
        }

        private static void GeneticAlgorithmSolution(object outputFilePath)
        {
            var outputFilePathTxt = (string) outputFilePath;
            var bestPenaltiesForEpochs = new Dictionary<int, List<int>>(); // <epoch, penalties>
            for (var i = 0; i < epochsQty; i++)
            {
                bestPenaltiesForEpochs[i] = new List<int>(examinationRepeatQty);
            }

            var execTimes = new long[examinationRepeatQty];

            for (var i = 0; i < examinationRepeatQty; i++)
            {
                var (_, penaltiesForEpoch, execTime) = geneticAlgorithm.FindBestIndividual(
                    startedPopulation,
                    epochsQty,
                    selectionOperator,
                    crossoverOperator,
                    mutationOperator
                );
                for (var j = 0; j < penaltiesForEpoch.Length; j++)
                {
                    var penalty = penaltiesForEpoch[j];
                    bestPenaltiesForEpochs[j].Add(penalty);
                }

                execTimes[i] = execTime;
            }

            var bestPenaltiesForEpoch = bestPenaltiesForEpochs.Select(e => e.Value.Min()).ToList();
            var avgPenaltiesForEpoch = bestPenaltiesForEpochs.Select(e => (int) e.Value.Average()).ToList();
            var worstPenaltiesForEpoch = bestPenaltiesForEpochs.Select(e => e.Value.Max()).ToList();
            var avgExecTimeMs = (int) execTimes.Average();
            SaveExaminationResult(outputFilePathTxt, worstPenaltiesForEpoch, avgPenaltiesForEpoch,
                bestPenaltiesForEpoch, avgExecTimeMs);
        }

        private static void SaveExaminationResult
        (string outputFilePath, IReadOnlyList<int> worstPenaltiesForEpoch, IReadOnlyList<int> avgPenaltiesForEpoch,
            IReadOnlyList<int> bestPenaltiesForEpoch, int avgExecTimeMs, char delimiter = ';')
        {
            var delimiterTxt = delimiter.ToString();
            var avgExecTimeMsTxt = avgExecTimeMs.ToString();
            var resultLines = new string[epochsQty];
            for (var i = 0; i < epochsQty; i++)
            {
                var worstPenaltyTxt = worstPenaltiesForEpoch[i].ToString();
                var avgPenaltyTxt = avgPenaltiesForEpoch[i].ToString();
                var bestPenaltyTxt = bestPenaltiesForEpoch[i].ToString();

                var resultLine =
                    $"{i}{delimiterTxt}{bestPenaltyTxt}{delimiterTxt}{avgPenaltyTxt}{delimiterTxt}{worstPenaltyTxt}{delimiterTxt}{avgExecTimeMsTxt}";
                resultLines[i] = resultLine;
            }

            File.AppendAllLines(outputFilePath, resultLines);
        }
    }
}