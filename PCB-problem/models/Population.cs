using System.Collections.Generic;

namespace PCB_problem
{
    public class Population
    {
        public IList<Individual> Individuals { get; } = new List<Individual>();

        public void AddIndividual(Individual individual)
        {
            Individuals.Add(individual);
        }
    }
}