using System;

namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public class UniformCrossover : ICrossover
    {
        private readonly double _crossoverProbability; // <0,1>
        private readonly Random _random = new Random();

        /**
         * crossOverProbability: probability within range 0.0 and 1.0 
         */
        public UniformCrossover(double crossoverProbability)
        {
            _crossoverProbability = crossoverProbability;
        }

        public Individual ApplyCrossover(Individual parentA, Individual parentB)
        {
            var newIndividual = new Individual();
            foreach (var endpoint in parentA.EndPoints)
            {
                var pathFromA = parentA.GetPath(endpoint);
                var pathFromB = parentB.GetPath(endpoint);
                var randProbability = _random.NextDouble();
                // if rand <=crossoverProbability, take from A, otherwise from B
                var pathCopy = randProbability <= _crossoverProbability ? pathFromA.Clone() : pathFromB.Clone();
                var startPointCopy = new Point(endpoint.Item1.X, endpoint.Item1.Y);
                var stopPointCopy = new Point(endpoint.Item2.X, endpoint.Item2.Y);
                newIndividual.AddPath((startPointCopy, stopPointCopy), pathCopy);
            }

            return newIndividual;
        }
    }
}