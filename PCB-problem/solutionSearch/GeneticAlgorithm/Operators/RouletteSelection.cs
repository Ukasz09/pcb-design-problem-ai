using System;
using System.Collections.Generic;

namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public class RouletteSelection : ISelection
    {
        private readonly Pcb _pcb;
        private readonly Random _random;
        private readonly int _w1;
        private readonly int _w2;
        private readonly int _w3;
        private readonly int _w4;
        private readonly int _w5;

        public RouletteSelection(Pcb pcb, int w1, int w2, int w3, int w4, int w5)
        {
            _pcb = pcb;
            _w1 = w1;
            _w2 = w2;
            _w3 = w3;
            _w4 = w4;
            _w5 = w5;
            _random = new Random();
        }

        public Individual Select(Population population)
        {
            var penalties = new List<int>(population.Individuals.Count);
            var penaltySum = 0;
            foreach (var individual in population.Individuals)
            {
                var penalty = PenaltyFunction.CalculatePenalty(individual.Paths, _pcb, _w1, _w2, _w3, _w4, _w5);
                penalties.Add(penalty);
                penaltySum += penalty;
            }

            var randRouletteChoice = _random.Next(0, penaltySum + 1);
            var buffer = 0;
            for (var i = 0; i < penalties.Count; i++)
            {
                buffer += penalties[i];
                if (buffer >= randRouletteChoice)
                {
                    return population.Individuals[i];
                }
            }

            return population.Individuals[penalties.Count - 1];
        }
    }
}