using System.Collections.Generic;

namespace PCB_problem.solutionSearch
{
    public interface IPcbSolution
    {
        Individual FindIndividual();
        Individual FindBestIndividual(int individualsQty);
    }
}