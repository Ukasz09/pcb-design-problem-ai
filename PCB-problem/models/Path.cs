using System;
using System.Collections.Generic;

namespace PCB_problem
{
    public class Path
    {
        public Point startPoint { get; }
        public Point endPoint { get; }

        public IList<Segment> Segments { get; } = new List<Segment>();

        public Path(Point startPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public void AddSegment(Segment segment)
        {
            Segments.Add(segment);
        }

        public override string ToString()
        {
            return string.Join(",", Segments);
        }
    }
}