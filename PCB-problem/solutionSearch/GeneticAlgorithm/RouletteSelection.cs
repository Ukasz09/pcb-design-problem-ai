using System;
using System.Collections.Generic;
using System.Linq;

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
            // example x axis for roulette: x1 - - - - - x2 - - x3 - - - - - - - - - - x4 - - - x5 - x6
            var rouletteAxisX =
                new Dictionary<Individual, (int, int)>(population.Individuals.Count); // <individual, <startX,stopX>
            var minPenalty = int.MaxValue;
            var lastXValue = 0;
            var randRouletteChoice = _random.NextDouble() * (lastXValue);
            foreach (var individual in population.Individuals)
            {
                var penalty = PenaltyFunction.CalculatePenalty(individual.Paths, _pcb, _w1, _w2, _w3, _w4, _w5);
                var inverselyPenalty = minPenalty / penalty;
                var newXValue = lastXValue + inverselyPenalty;
                rouletteAxisX.Add(individual, (lastXValue, newXValue));
                if (randRouletteChoice >= lastXValue && randRouletteChoice <= newXValue)
                {
                    return individual;
                }

                lastXValue = newXValue;
            }

            throw new ArithmeticException(
                $"Incorrect roulette choice or axis: \n Choice: {randRouletteChoice}, \n Axis: {string.Join(",", rouletteAxisX.Values)}");
        }
    }
}