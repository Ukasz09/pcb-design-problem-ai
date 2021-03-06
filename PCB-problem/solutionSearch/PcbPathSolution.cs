using System.Collections.Generic;

namespace PCB_problem.solutionSearch
{
    public interface PcbPathSolution
    {
        List<Path> FindSolution(Pcb pcb);
    }
}