using System;

namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public class UniformCrossover
    {
        private readonly double _crossOverProbability; // <0,1>
        private readonly Random _random = new Random();

        public UniformCrossover(double crossOverProbability)
        {
            _crossOverProbability = crossOverProbability;
        }

        public Individual ApplyCrossover(Individual parentA, Individual parentB)
        {
            var newIndividual = new Individual();
            foreach (var endpoint in parentA.StartPoints)
            {
                var pathFromA = parentA.GetPath(endpoint);
                var pathFromB = parentB.GetPath(endpoint);
                var randProbability = _random.NextDouble();
                // if rand <=crossoverProbability, take from A, otherwise from B
                var pathCopy = randProbability <= _crossOverProbability ? pathFromA.Clone() : pathFromB.Clone();
                var startPointCopy = new Point(endpoint.Item1.X, endpoint.Item1.Y);
                var stopPointCopy = new Point(endpoint.Item2.X, endpoint.Item2.Y);
                newIndividual.AddPath((startPointCopy, stopPointCopy), pathCopy);
            }

            return newIndividual;
        }
    }
}