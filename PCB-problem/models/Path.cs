using System.Collections.Generic;
using System.Linq;

namespace PCB_problem
{
    public class Path
    {
        public Point startPoint { get; }
        public Point endPoint { get; }

        // /**
        //  * Return last point of path line draw made from segments or startPoint if segments empty 
        //  */
        // public Point LastEdgePoint
        // {
        //     get
        //     {
        //         if (_segments.Count == 0)
        //         {
        //             return startPoint;
        //         }
        //
        //         return _lastEdgePoint;
        //     }
        // }
        //
        // private Point _lastEdgePoint = null;

        private readonly IList<(Direction, int)> _segments = new List<(Direction, int)>(); // <direction, stepSize>

        public Path(Point startPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public void AddSegment(Direction direction, int stepSize)
        {
            _segments.Add((direction, stepSize));
        }
    }
}