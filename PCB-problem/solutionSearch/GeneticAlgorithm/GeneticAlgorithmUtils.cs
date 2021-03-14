namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public static class GeneticAlgorithmUtils
    {
        public static Population GetStartedPopulation(Pcb pcb, int size)
        {
            var randomSearch = new RandomSearch(pcb);
            var population = new Population();
            for (var i = 0; i < size; i++)
            {
                var individual = randomSearch.FindIndividual();
                population.AddIndividual(individual);
            }

            return population;
        }
    }
}