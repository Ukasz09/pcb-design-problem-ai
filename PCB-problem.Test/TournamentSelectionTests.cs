using System;
using System.Collections.Generic;
using NUnit.Framework;
using PCB_problem.solutionSearch.GeneticAlgorithm;
using PCB_problem.solutionSearch;

namespace PCB_problem.Test
{
    public class TournamentSelectionTests
    {
        private Pcb _pcb;

        [OneTimeSetUp]
        public void Setup()
        {
            _pcb = new Pcb(16, 16);
            _pcb.AddEndpoint(new Point(2, 7), new Point(9, 7));
            _pcb.AddEndpoint(new Point(3, 8), new Point(7, 6));
            _pcb.AddEndpoint(new Point(4, 4), new Point(5, 13));
            _pcb.AddEndpoint(new Point(5, 2), new Point(10, 12));
            _pcb.AddEndpoint(new Point(6, 6), new Point(6, 8));
            _pcb.AddEndpoint(new Point(7, 10), new Point(13, 10));
            _pcb.AddEndpoint(new Point(8, 2), new Point(8, 15));
            _pcb.AddEndpoint(new Point(10, 10), new Point(13, 6));
        }

        [Test]
        [TestCaseSource(nameof(GetTournamentTestData))]
        public void GivenStartedPopulationAndTournamentSizeWhenSelectionThenIndividualWithMinCostSelected
            (int w1, int w2, int w3, int w4, int w5, int tournamentPercentIntFormat)
        {
            // Given
            const int populationSize = 5;
            const int seed = 987654;
            var startedPopulation = GeneticAlgorithmUtils.GetStartedPopulation(_pcb, populationSize, seed);
            var tournamentSizePercent = ((double) tournamentPercentIntFormat) / 100;
            var tournamentSelection = new TournamentSelection(_pcb, tournamentSizePercent, w1, w2, w3, w4, w5, seed);
            var randomGenerator = new Random(seed);
            var qtyToDraw = (int) Math.Ceiling(tournamentSizePercent * startedPopulation.IndividualsQty);
            var populationSubset =
                GeneticAlgorithmUtils.DrawIndividualsWithoutRepeating(startedPopulation, qtyToDraw, randomGenerator);
            var expectedIndividual =
                PenaltyFunction.GetIndividualWithMinPenaltyCost(populationSubset, _pcb, w1, w2, w3, w4, w5);

            // When
            var selectedIndividual = tournamentSelection.Select(startedPopulation);

            // Then
            Assert.AreEqual(expectedIndividual, selectedIndividual);
        }

        private static IEnumerable<int[]> GetTournamentTestData()
        {
            for (var i = 0; i < 10; i++)
            {
                var tournamentSizePercentIntFormat = (i + 1) * 10;
                yield return new[] {1, 1, 1, 1, 1, tournamentSizePercentIntFormat};
                yield return new[] {1, 1, 1, 1, 1, tournamentSizePercentIntFormat};
                yield return new[] {30, 1, 1, 30, 30, tournamentSizePercentIntFormat};
                yield return new[] {10, 1, 1, 8, 8, tournamentSizePercentIntFormat};
                yield return new[] {1, 5, 5, 1, 1, tournamentSizePercentIntFormat};
            }
        }
    }
}