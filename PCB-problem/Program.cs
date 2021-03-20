using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using PCB_problem.solutionSearch.GeneticAlgorithm;

namespace PCB_problem
{
    internal static class Program
    {
        private const int ExaminationRepeatQty = 10;
        private const string ResultsDirectoryName = "examination-results";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string ContentHeaderText = "epochNo;bestPenalty;avgPenalty;worstPenalty;avgExecTimeMs";

        private static Pcb pcb;
        private const int _defaultPopulationSize = 1000;
        private const int _defaultW1 = 40;
        private const int _defaultW2 = 1;
        private const int _defaultW3 = 2;
        private const int _defaultW4 = 30;
        private const int _defaultW5 = 30;
        private const double _defaultTournamentSizePercent = 0.002;
        private const double _defaultCrossoverProbability = 0.5;
        private const double _defaultMutationProbability = 0.1;
        private const int _defaultEpochsQty = 100;

        private static void Main(string[] args)
        {
            pcb = ReadPcbData();
            // InvestigateAffectOfPopulationSize(new[] {10, 50, 100, 500, 1000, 2000});
            // InvestigateEpochsQty(new[] {10, 50, 100, 500, 1000, 2000});
            InvestigateCrossoverProbability(new[] {0.1, 0.25, 0.5, 0.75, 0.9});
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
            var outputFilePathPrefix = $"../../../../{ResultsDirectoryName}/population-size-investigation";
            const string outputFilePathExtension = ".csv";

            var (w1, w2, w3, w4, w5) = (_defaultW1, _defaultW2, _defaultW3, _defaultW4, _defaultW5);
            const double tournamentSizePercent = _defaultTournamentSizePercent;
            const double crossoverProbability = _defaultCrossoverProbability;
            const double mutationProbability = _defaultMutationProbability;
            const int epochsQty = _defaultEpochsQty;

            var geneticAlgorithm = new GeneticAlgorithm(pcb, w1, w2, w3, w4, w5);
            var selectionOperator = new TournamentSelection(pcb, tournamentSizePercent, w1, w2, w3, w4, w5);
            var crossoverOperator = new UniformCrossover(crossoverProbability);
            var mutationOperator = new MutationA(mutationProbability);

            _logger.Info("----------------------------------------------");
            _logger.Info("-- Started population size investigations --");
            _logger.Info("----------------------------------------------");

            foreach (var populationSize in populationSizes)
            {
                var outputFilePath = $"{outputFilePathPrefix}-{populationSize.ToString()}{outputFilePathExtension}";
                File.WriteAllLines(outputFilePath, new[] {ContentHeaderText});
                var startedPopulation = GeneticAlgorithmUtils.GetStartedPopulation(pcb, populationSize);
                GeneticAlgorithmSolution(outputFilePath, selectionOperator, geneticAlgorithm, crossoverOperator,
                    mutationOperator, epochsQty, startedPopulation);
                _logger.Info(
                    $"Calulated for:  {populationSize.ToString()},{w1.ToString()},{w2.ToString()},{w3.ToString()},{w4.ToString()},{w5.ToString()},{tournamentSizePercent.ToString()},{crossoverProbability.ToString()},{mutationProbability.ToString()},{epochsQty.ToString()}");
            }
        }

        private static void InvestigateEpochsQty(IEnumerable<int> epochsQty)
        {
            var outputFilePathPrefix = $"../../../../{ResultsDirectoryName}/epochs-qty-investigation";
            const string outputFilePathExtension = ".csv";

            var (w1, w2, w3, w4, w5) = (_defaultW1, _defaultW2, _defaultW3, _defaultW4, _defaultW5);
            const double tournamentSizePercent = _defaultTournamentSizePercent;
            const double crossoverProbability = _defaultCrossoverProbability;
            const double mutationProbability = _defaultMutationProbability;
            const int populationSize = _defaultPopulationSize;

            var geneticAlgorithm = new GeneticAlgorithm(pcb, w1, w2, w3, w4, w5);
            var selectionOperator = new TournamentSelection(pcb, tournamentSizePercent, w1, w2, w3, w4, w5);
            var crossoverOperator = new UniformCrossover(crossoverProbability);
            var mutationOperator = new MutationA(mutationProbability);

            _logger.Info("----------------------------------------------");
            _logger.Info("-- Started epochs qty investigations --");
            _logger.Info("----------------------------------------------");
            var startedPopulation = GeneticAlgorithmUtils.GetStartedPopulation(pcb, populationSize);

            foreach (var epochQty in epochsQty)
            {
                var outputFilePath = $"{outputFilePathPrefix}-{epochQty.ToString()}{outputFilePathExtension}";
                File.WriteAllLines(outputFilePath, new[] {ContentHeaderText});

                GeneticAlgorithmSolution(outputFilePath, selectionOperator, geneticAlgorithm, crossoverOperator,
                    mutationOperator, epochQty, startedPopulation);
                _logger.Info(
                    $"Calulated for:  {populationSize.ToString()},{w1.ToString()},{w2.ToString()},{w3.ToString()},{w4.ToString()},{w5.ToString()},{tournamentSizePercent.ToString()},{crossoverProbability.ToString()},{mutationProbability.ToString()},{epochsQty}");
            }
        }

        private static void InvestigateCrossoverProbability(IEnumerable<double> crossoverProbabilities)
        {
            var outputFilePathPrefix = $"../../../../{ResultsDirectoryName}/crossover-probability-investigation";
            const string outputFilePathExtension = ".csv";

            var (w1, w2, w3, w4, w5) = (_defaultW1, _defaultW2, _defaultW3, _defaultW4, _defaultW5);
            const double tournamentSizePercent = _defaultTournamentSizePercent;
            const double mutationProbability = _defaultMutationProbability;
            const int populationSize = _defaultPopulationSize;
            const int epochsQty = _defaultEpochsQty;

            var geneticAlgorithm = new GeneticAlgorithm(pcb, w1, w2, w3, w4, w5);
            var selectionOperator = new TournamentSelection(pcb, tournamentSizePercent, w1, w2, w3, w4, w5);
            var mutationOperator = new MutationA(mutationProbability);

            _logger.Info("----------------------------------------------");
            _logger.Info("-- Started crossover probability investigations --");
            _logger.Info("----------------------------------------------");
            var startedPopulation = GeneticAlgorithmUtils.GetStartedPopulation(pcb, populationSize);

            foreach (var probability in crossoverProbabilities)
            {
                var outputFilePath = $"{outputFilePathPrefix}-{probability.ToString()}{outputFilePathExtension}";
                File.WriteAllLines(outputFilePath, new[] {ContentHeaderText});

                var crossoverOperator = new UniformCrossover(probability);
                GeneticAlgorithmSolution(outputFilePath, selectionOperator, geneticAlgorithm, crossoverOperator,
                    mutationOperator, epochsQty, startedPopulation);
                _logger.Info(
                    $"Calulated for:  {populationSize.ToString()},{w1.ToString()},{w2.ToString()},{w3.ToString()},{w4.ToString()},{w5.ToString()},{tournamentSizePercent.ToString()},{probability.ToString()},{mutationProbability.ToString()},{epochsQty.ToString()}");
            }
        }


        private static void GeneticAlgorithmSolution
        (string outputFilePath, ISelection selectionOperator, GeneticAlgorithm geneticAlgorithm,
            ICrossover crossoverOperator, IMutation mutationOperator, int epochsQty, Population startedPopulation)
        {
            var bestPenaltiesForEpochs = new Dictionary<int, List<int>>(); // <epoch, penalties>
            for (var i = 0; i < epochsQty; i++)
            {
                bestPenaltiesForEpochs[i] = new List<int>(ExaminationRepeatQty);
            }

            var execTimes = new long[ExaminationRepeatQty];

            for (var i = 0; i < ExaminationRepeatQty; i++)
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
            SaveExaminationResult(outputFilePath, worstPenaltiesForEpoch, avgPenaltiesForEpoch,
                bestPenaltiesForEpoch, avgExecTimeMs, epochsQty);
        }

        private static void SaveExaminationResult
        (string outputFilePath, IReadOnlyList<int> worstPenaltiesForEpoch, IReadOnlyList<int> avgPenaltiesForEpoch,
            IReadOnlyList<int> bestPenaltiesForEpoch, int avgExecTimeMs, int epochsQty, char delimiter = ';')
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