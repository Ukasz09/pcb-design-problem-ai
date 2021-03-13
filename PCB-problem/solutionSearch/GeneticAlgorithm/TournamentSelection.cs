using System;
using System.Collections.Generic;
using System.Linq;

namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public class TournamentSelection : ISelection
    {
        private readonly Random _random;
        private readonly Pcb _pcb;
        private readonly int _w1;
        private readonly int _w2;
        private readonly int _w3;
        private readonly int _w4;
        private readonly int _w5;
        private readonly int _tournamentSize;

        public TournamentSelection(Pcb pcb, int tournamentSize, int w1, int w2, int w3, int w4, int w5)
        {
            _random = new Random();
            _pcb = pcb;
            _tournamentSize = tournamentSize;
            _w1 = w1;
            _w2 = w2;
            _w3 = w3;
            _w4 = w4;
            _w5 = w5;
        }

        public TournamentSelection(int seed, Pcb pcb, int tournamentSize, int w1, int w2, int w3, int w4, int w5)
        {
            _random = new Random(seed);
            _pcb = pcb;
            _tournamentSize = tournamentSize;
            _w1 = w1;
            _w2 = w2;
            _w3 = w3;
            _w4 = w4;
            _w5 = w5;
        }

        public Individual Select(Population population)
        {
            if (_tournamentSize > population.Individuals.Count)
            {
                throw new ArgumentException(
                    "Quantity of individuals to select from population cannot be less than actual population size");
            }

            var individuals = DrawIndividualsWithoutRepeating(population, _tournamentSize);
            var bestIndividual =
                PenaltyFunction.GetIndividualWithMinPenaltyCost(individuals, _pcb, _w1, _w2, _w3, _w4, _w5);
            return bestIndividual;
        }

        private IEnumerable<Individual> DrawIndividualsWithoutRepeating(Population population, int qty)
        {
            var possibleIndexesToDraw = Enumerable.Range(0, qty).ToList();
            var individuals = new List<Individual>(qty);
            for (var i = 0; i < qty; i++)
            {
                var randIndex = possibleIndexesToDraw[_random.Next(possibleIndexesToDraw.Count)];
                possibleIndexesToDraw.Remove(randIndex);
                var individual = population.Individuals[randIndex];
                individuals.Add(individual);
            }

            return individuals;
        }
    }
}