using NLog;

namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private readonly Pcb _pcb;
        private readonly int _w1;
        private readonly int _w2;
        private readonly int _w3;
        private readonly int _w4;
        private readonly int _w5;

        public GeneticAlgorithm(Pcb pcb, int w1, int w2, int w3, int w4, int w5)
        {
            _pcb = pcb;
            _w1 = w1;
            _w2 = w2;
            _w3 = w3;
            _w4 = w4;
            _w5 = w5;
        }

        /**
         * @return (bestIndividual, penaltyForEachEpoch, execTime)
         */
        public (Individual, int[], long) FindBestIndividual
        (Population startedPopulation, int epochsQty, ISelection selectionOperator, ICrossover crossoverOperator,
            IMutation mutationOperator)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var bestPenaltiesForEpoch = new int[epochsQty];

            var minPenalty = PenaltyFunction.CalculatePenalty(startedPopulation.Individuals[0].Paths, _pcb, _w1, _w2,
                _w3, _w4, _w5);
            var bestIndividual = startedPopulation.Individuals[0];
            for (var i = 0; i < epochsQty; i++)
            {
                var newPopulation = new Population();

                // Always store the best individual
                newPopulation.AddIndividual(bestIndividual);

                var bestPenaltyForEpoch = int.MaxValue;
                Individual bestIndividualForEpoch = null;
                while (newPopulation.IndividualsQty < startedPopulation.IndividualsQty)
                {
                    var parentA = selectionOperator.Select(startedPopulation);
                    var parentB = selectionOperator.Select(startedPopulation);
                    var newIndividual = crossoverOperator.ApplyCrossover(parentA, parentB);
                    mutationOperator.Mutate(newIndividual);
                    var penalty = PenaltyFunction.CalculatePenalty(newIndividual.Paths, _pcb, _w1, _w2, _w3, _w4, _w5);
                    if (penalty < bestPenaltyForEpoch)
                    {
                        bestPenaltyForEpoch = penalty;
                        bestIndividualForEpoch = newIndividual;
                    }

                    newPopulation.AddIndividual(newIndividual);
                }

                bestPenaltiesForEpoch[i] = bestPenaltyForEpoch;
                if (bestPenaltyForEpoch < minPenalty)
                {
                    minPenalty = bestPenaltyForEpoch;
                    bestIndividual = bestIndividualForEpoch;
                }

                startedPopulation = newPopulation;
            }

            watch.Stop();
            return (bestIndividual, bestPenaltiesForEpoch, watch.ElapsedMilliseconds);
        }
    }
}