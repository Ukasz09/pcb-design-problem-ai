using System;
using System.Collections.Generic;

namespace PCB_problem.solutionSearch
{
    public class RandomSearch : PcbPathSolution
    {
        private readonly Random _random = new Random();

        private const int StepSizeMin = 1;
        private const int StepSizeMax = 5;

        private Dictionary<Direction, int> directionIncValue = new Dictionary<Direction, int>
        {
            {Direction.Left, -1},
            {Direction.Up, 1},
            {Direction.Right, 1},
            {Direction.Down, -1}
        };

        public Path[] FindSolution(Pcb pcb)
        {
            throw new NotImplementedException();
            // while (expression)
            // {
            //    
            // }
        }

        private Path FindPath(Point startPoint, Point endPoint, Pcb pcb)
        {
            Path path = new Path(startPoint, endPoint);

            // var direction = RandDirection();
            // var stepSize = _random.Next(StepSizeMin, StepSizeMax + 1);
            // // Check overlapping
            // var lastPathPoint = startPoint;
            // for (int i = 0; i < stepSize; i++)
            // {
            //     var newPathPoint = GetNextPoint(lastPathPoint, direction);
            //     if (newPathPoint.Equals(endPoint))
            //     {
            //         path.AddSegment(direction, stepSize);
            //         return path;
            //     }
            //
            //     if (pcb.IsOneOfEndpoints(newPathPoint))
            //     {
            //         return null;
            //     }
            // }
            path.AddSegment(direction, stepSize);
        }

        /**
         * @return: (segment, new last path point)
         */
        private (Segment, Point) RandSegment(Point segmentStartPoint, Point stopPoint, Pcb pcb,
            IList<Direction> availableDirections)
        {
            var direction = RandDirection(availableDirections);
            // To make sure that next time we don't get the same direction
            availableDirections.Remove(direction);

            var segment = new Segment(direction, RandStepSize());
            var lastPathPoint = segmentStartPoint;

            // Check overlapping
            for (int i = 0; i < segment.StepSize; i++)
            {
                var newPathPoint = GetNextPoint(lastPathPoint, segment.Direction);

                // if hit stop point
                if (newPathPoint.Equals(stopPoint))
                {
                    // remove extra part of segment 
                    segment.StepSize = i + 1;
                    return (segment, newPathPoint);
                }

                // if overlap with other endpoints point or self startPoint 
                if (pcb.IsOneOfEndpoints(newPathPoint))
                {
                    // if stepSize can be shorter
                    if (i != 0)
                    {
                        // make shorter to avoid overlapping with endpoint 
                        segment.StepSize = i;
                        return (segment, newPathPoint);
                    }

                    // there is no other move
                    if (availableDirections.Count == 0)
                    {
                        return (null, segmentStartPoint);
                    }

                    // change direction
                    return RandSegment(segmentStartPoint, stopPoint, pcb, availableDirections);
                }

                lastPathPoint = newPathPoint;
            }

            return (segment, lastPathPoint)
        }

        private Point GetNextPoint(Point lastPoint, Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                {
                    return new Point(lastPoint.X, lastPoint.Y - 1);
                }
                case Direction.Up:
                {
                    return new Point(lastPoint.X, lastPoint.Y + 1);
                }
                case Direction.Left:
                {
                    return new Point(lastPoint.X - 1, lastPoint.Y);
                }
                default:
                {
                    return new Point(lastPoint.X + 1, lastPoint.Y);
                }
            }
        }

        private Direction RandDirection(IList<Direction> availableDirections)
        {
            int randIndex = _random.Next(availableDirections.Count);
            return availableDirections[randIndex];
        }

        private int RandStepSize()
        {
            return _random.Next(StepSizeMin, StepSizeMax + 1);
        }
    }
}