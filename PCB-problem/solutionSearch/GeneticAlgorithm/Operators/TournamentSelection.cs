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
        private readonly double _tournamentSizePercent;

        public TournamentSelection(Pcb pcb, double tournamentSizePercent, int w1, int w2, int w3, int w4, int w5,
            int? seed = null)
        {
            _random = seed != null ? new Random(seed.Value) : new Random();
            _pcb = pcb;
            _tournamentSizePercent = tournamentSizePercent;
            _w1 = w1;
            _w2 = w2;
            _w3 = w3;
            _w4 = w4;
            _w5 = w5;
        }

        public Individual Select(Population population)
        {
            var qtyToDraw = (int)Math.Ceiling(_tournamentSizePercent * population.IndividualsQty);
            var individuals = GeneticAlgorithmUtils.DrawIndividualsWithoutRepeating(population, qtyToDraw, _random);
            var bestIndividual =
                PenaltyFunction.GetIndividualWithMinPenaltyCost(individuals, _pcb, _w1, _w2, _w3, _w4, _w5);
            return bestIndividual;
        }
    }
}