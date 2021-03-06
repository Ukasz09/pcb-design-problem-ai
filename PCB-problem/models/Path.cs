using System.Collections.Generic;

namespace PCB_problem
{
    public class Path
    {
        public Point startPoint { get; }
        public Point endPoint { get; }

        private readonly IList<Segment> _segments = new List<Segment>();

        public Path(Point startPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public void AddSegment(Segment segment)
        {
            _segments.Add(segment);
        }
    }
}