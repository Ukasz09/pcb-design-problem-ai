using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCB_problem.solutionSearch;

namespace PCB_problem.Test
{
    public class PenaltyFunctionTests
    {
        private Pcb _pcb;
        private IList<Individual> individuals;

        [OneTimeSetUp]
        public void Setup()
        {
            InitPcb();
            InitIndividuals();
        }

        private void InitPcb()
        {
            _pcb = new Pcb(16, 16);
            _pcb.AddEndpoint(new Point(6, 6), new Point(6, 8));
            _pcb.AddEndpoint(new Point(10, 10), new Point(13, 6));
        }

        private void InitIndividuals()
        {
            individuals = new List<Individual>(_pcb.Endpoints.Count);
            var segments = new IList<Segment>[_pcb.Endpoints.Count];
            segments[0] = new List<Segment>()
            {
                new Segment(Direction.Left, 1),
                new Segment(Direction.Up, 1),
                new Segment(Direction.Right, 1),
                new Segment(Direction.Up, 1)
            };
            segments[1] = new List<Segment>()
            {
                new Segment(Direction.Down, 5),
                new Segment(Direction.Right, 1),
                new Segment(Direction.Right, 2),
                new Segment(Direction.Up, 1)
            };
            for (var i = 0; i < _pcb.Endpoints.Count; i++)
            {
                var (startPoint, stopPoint) = _pcb.Endpoints[i];
                var path = new Path(startPoint, stopPoint, segments[i]);
                var individual = new Individual();
                individual.AddPath((startPoint, stopPoint), path);
                individuals.Add(individual);
            }
        }

        [Test]
        [TestCase(1, 1, 1, 1, 1)]
        [TestCase(1, 1, 1, 1, 1)]
        [TestCase(30, 1, 1, 30, 30)]
        [TestCase(10, 1, 1, 8, 8)]
        [TestCase(1, 5, 5, 1, 1)]
        public void WhenGetIndividualWithMinPenaltyCostThenProperIndividual(int w1, int w2, int w3, int w4, int w5)
        {
            // Given
            var fstIndividualPenalty = PenaltyFunction.CalculatePenalty(individuals[0].Paths, _pcb, w1, w2, w3, w4, w5);
            var sndIndividualPenalty = PenaltyFunction.CalculatePenalty(individuals[1].Paths, _pcb, w1, w2, w3, w4, w5);
            var bestCost = Math.Min(fstIndividualPenalty, sndIndividualPenalty);
            var expectedIndividual = fstIndividualPenalty == bestCost ? individuals[0] : individuals[1];

            // When
            var bestIndividual = PenaltyFunction.GetIndividualWithMinPenaltyCost(individuals, _pcb, w1, w2, w3, w4, w5);

            // Then
            Assert.AreEqual(expectedIndividual, bestIndividual);
        }
    }
}