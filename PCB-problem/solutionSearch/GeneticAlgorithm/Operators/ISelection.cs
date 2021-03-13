namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public interface ISelection
    {
        Individual Select(Population population);
    }
}