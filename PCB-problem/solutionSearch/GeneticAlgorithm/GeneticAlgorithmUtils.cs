using System;
using System.Collections.Generic;
using System.Linq;

namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public static class GeneticAlgorithmUtils
    {
        public static Population GetStartedPopulation(Pcb pcb, int size, int? seed = null)
        {
            var randomSearch = new RandomSearch(pcb, seed);
            var population = new Population();
            for (var i = 0; i < size; i++)
            {
                var individual = randomSearch.FindIndividual();
                population.AddIndividual(individual);
            }

            return population;
        }

        public static IEnumerable<Individual> DrawIndividualsWithoutRepeating(Population population, int qty,
            Random randomGenerator)
        {
            var possibleIndexesToDraw = Enumerable.Range(0, population.IndividualsQty).ToList();
            var individuals = new List<Individual>(qty);
            for (var i = 0; i < qty; i++)
            {
                var randIndex = possibleIndexesToDraw[randomGenerator.Next(possibleIndexesToDraw.Count)];
                possibleIndexesToDraw.Remove(randIndex);
                var individual = population.Individuals[randIndex];
                individuals.Add(individual);
            }

            return individuals;
        }
    }
}