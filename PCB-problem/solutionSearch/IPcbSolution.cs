using System.Collections.Generic;

namespace PCB_problem.solutionSearch
{
    public interface IPcbSolution
    {
        Dictionary<(Point, Point), Path> FindSolution();
        Dictionary<(Point, Point), Path> FindBestSolution(int individualsQty);
    }
}