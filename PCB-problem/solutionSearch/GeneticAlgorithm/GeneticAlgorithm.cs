using System;

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

        public GeneticAlgorithm(Pcb pcb, int w3, int w2, int w1, int w4, int w5)
        {
            _pcb = pcb;
            _w3 = w3;
            _w2 = w2;
            _w1 = w1;
            _w4 = w4;
            _w5 = w5;
        }

        public Individual FindBestIndividual(int populationSize, int generationsQty, ISelection selectionMethod,
            UniformCrossover crossover)
        {
            if (populationSize < 1)
            {
                throw new ArgumentException("Started population size must be greater than 1");
            }

            var startedPopulation = GetStartedPopulation(populationSize);
            var minPenalty = int.MaxValue;
            Individual bestIndividual = null;
            for (var i = 0; i < generationsQty; i++)
            {
                var newPopulation = new Population();
                while (newPopulation.IndividualsQty < populationSize)
                {
                    var parentA = selectionMethod.Select(startedPopulation);
                    var parentB = selectionMethod.Select(startedPopulation);
                    var newIndividual = crossover.ApplyCrossover(parentA, parentB);
                    //TODO: add mutation
                    var penalty = PenaltyFunction.CalculatePenalty(newIndividual.Paths, _pcb, _w1, _w2, _w3, _w4, _w5);
                    if (penalty < minPenalty)
                    {
                        minPenalty = penalty;
                        bestIndividual = newIndividual;
                    }

                    newPopulation.AddIndividual(newIndividual);
                }
            }

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