using System;
using NLog;

namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private readonly Pcb _pcb;
        private readonly int _w1;
        private readonly int _w2;
        private readonly int _w3;
        private readonly int _w4;
        private readonly int _w5;
        private readonly ILogger _logger;

        public GeneticAlgorithm(Pcb pcb, int w1, int w2, int w3, int w4, int w5)
        {
            _pcb = pcb;
            _w1 = w1;
            _w2 = w2;
            _w3 = w3;
            _w4 = w4;
            _w5 = w5;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public Individual FindBestIndividual(int populationSize, int epochsQty, ISelection selectionOperator,
            UniformCrossover crossoverOperator, IMutation mutationOperator)
        {
            if (populationSize < 1)
            {
                throw new ArgumentException("Started population size must be greater than 1");
            }

            _logger.Log(LogLevel.Info, "Generating started population ...");
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var startedPopulation = GetStartedPopulation(populationSize);
            var minPenalty = PenaltyFunction.CalculatePenalty(startedPopulation.Individuals[0].Paths, _pcb, _w1, _w2,
                _w3, _w4, _w5);
            var bestIndividual = startedPopulation.Individuals[0];
            _logger.Log(LogLevel.Info, "Calculating generations ...");
            for (var i = 0; i < epochsQty; i++)
            {
                var newPopulation = new Population();

                // Always store the best individual
                newPopulation.AddIndividual(bestIndividual);

                while (newPopulation.IndividualsQty < populationSize)
                {
                    var parentA = selectionOperator.Select(startedPopulation);
                    var parentB = selectionOperator.Select(startedPopulation);
                    var newIndividual = crossoverOperator.ApplyCrossover(parentA, parentB);
                    mutationOperator.Mutate(newIndividual);
                    var penalty = PenaltyFunction.CalculatePenalty(newIndividual.Paths, _pcb, _w1, _w2, _w3, _w4, _w5);
                    if (penalty < minPenalty)
                    {
                        minPenalty = penalty;
                        bestIndividual = newIndividual;
                    }

                    newPopulation.AddIndividual(newIndividual);
                }

                _logger.Log(LogLevel.Debug, $"Generation {i}) - Actual best penalty: {minPenalty}");
                startedPopulation = newPopulation;
            }

            watch.Stop();
            _logger.Log(LogLevel.Info, "----- FINISHED -----");
            var finalLog =
                $"Best penalty: {minPenalty}\nExecution time: {watch.ElapsedMilliseconds.ToString()} ms";
            _logger.Log(LogLevel.Info, finalLog);
            return bestIndividual;
        }

        private Population GetStartedPopulation(int size)
        {
            var randomSearch = new RandomSearch(_pcb);
            var population = new Population();
            for (var i = 0; i < size; i++)
            {
                var individual = randomSearch.FindIndividual();
                population.AddIndividual(individual);
            }

            return population;
        }
    }
}