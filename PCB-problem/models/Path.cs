using System;
using System.Collections.Generic;
using System.Linq;

namespace PCB_problem
{
    public class Path
    {
        public Point startPoint { get; }
        public Point stopPoint { get; }

        public IList<Segment> Segments { get; }

        public Path(Point startPoint, Point stopPoint)
        {
            this.startPoint = startPoint;
            this.stopPoint = stopPoint;
            Segments = new List<Segment>();
        }

        private Path(Point startPoint, Point stopPoint, IList<Segment> segments)
        {
            this.startPoint = startPoint;
            this.stopPoint = stopPoint;
            Segments = segments;
        }

        public void AddSegment(Segment segment)
        {
            Segments.Add(segment);
        }

        public override string ToString()
        {
            return string.Join(",", Segments);
        }

        public Path Clone()
        {
            var segmentsCopy = Segments.Select(segment => new Segment(segment.Direction, segment.StepSize)).ToList();
            var startPointCopy = new Point(startPoint.X, startPoint.Y);
            var stopPointCopy = new Point(stopPoint.X, stopPoint.Y);
            return new Path(startPointCopy, stopPointCopy, segmentsCopy);
        }

        private bool Equals(Path other)
        {
            if (!startPoint.Equals(other.startPoint) || !stopPoint.Equals(other.stopPoint))
            {
                return false;
            }

            return Segments.All(segment => other.Segments.Contains(segment));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Path) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(startPoint, stopPoint, Segments);
        }
    }
}