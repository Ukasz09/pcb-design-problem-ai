using System.Collections.Generic;

namespace PCB_problem
{
    public class Population
    {
        private IList<Individual> _individuals = new List<Individual>();

        public void AddIndividual(Individual individual)
        {
            _individuals.Add(individual);
        }
    }
}