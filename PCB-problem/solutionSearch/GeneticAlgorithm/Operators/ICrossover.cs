namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public interface ICrossover
    {
        Individual ApplyCrossover(Individual parentA, Individual parentB);
    }
}