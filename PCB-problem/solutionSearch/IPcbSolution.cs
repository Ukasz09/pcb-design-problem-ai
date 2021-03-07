using System.Collections.Generic;

namespace PCB_problem.solutionSearch
{
    public interface IPcbSolution
    {
        Dictionary<(Point, Point), Path> FindSolution(Pcb pcb);
        Dictionary<(Point, Point), Path> FindBestSolution(Pcb pcb, int individualsQty);
    }
}