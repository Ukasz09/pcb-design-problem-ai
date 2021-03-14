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
            var penaltyForIndividuals = new Dictionary<Individual, int>(population.Individuals.Count);
            foreach (var individual in population.Individuals)
            {
                var penalty = PenaltyFunction.CalculatePenalty(individual.Paths, _pcb, _w1, _w2, _w3, _w4, _w5);
                penaltyForIndividuals.Add(individual, penalty);
            }

            var rouletteAxisX = GetRouletteAxisX(penaltyForIndividuals);
            var randRouletteChoice = _random.NextDouble();
            foreach (var (individual, (startX, stopX)) in rouletteAxisX)
            {
                if (randRouletteChoice >= startX && randRouletteChoice <= stopX)
                {
                    return individual;
                }
            }

            throw new ArithmeticException(
                $"Incorrect roulette choice or axis: \n Choice: {randRouletteChoice}, \n Axis: {string.Join(",", rouletteAxisX.Values)}");
        }

        // <individual, <startX,stopX>
        private Dictionary<Individual, (int, int)> GetRouletteAxisX(
            Dictionary<Individual, int> penaltyForIndividuals)
        {
            // example x axis for roulette: x1 - - - - - x2 - - x3 - - - - - - - - - - x4 - - - x5 - x6
            var xAxisValues =
                new Dictionary<Individual, (int, int)>(penaltyForIndividuals.Count); // <individual, <startX,stopX>
            var minPenalty = penaltyForIndividuals.Values.Min();
            var lastXValue = 0;
            foreach (var (individual, penalty) in penaltyForIndividuals)
            {
                var inverselyPenalty = minPenalty / penalty;
                var newXValue = lastXValue + inverselyPenalty;
                xAxisValues.Add(individual, (lastXValue, newXValue));
                lastXValue = newXValue;
            }

            // Scaling x axis values to make it within range 0.0 - 1.0
            var maxXValue = lastXValue;
            foreach (var individual in xAxisValues.Keys)
            {
                var scaledStartX = xAxisValues[individual].Item1 / maxXValue;
                var scaledStopX = xAxisValues[individual].Item2 / maxXValue;
                xAxisValues[individual] = (scaledStartX, scaledStopX);
            }

            return xAxisValues;
        }
    }
}