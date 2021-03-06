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
        private (Segment, Point) RandomSegment(Point segmentStartPoint, Point stopEndpoint, Pcb pcb)
        {
            var segment = new Segment(RandDirection(), RandStepSize());
            var lastPathPoint = segmentStartPoint;
            // Check overlapping
            for (int i = 0; i < segment.StepSize; i++)
            {
                var newPathPoint = GetNextPoint(lastPathPoint, segment.Direction);
                //TODO: take into consideration hitting startPoint !
                //TODO: take into consideration no move scenario ! (return null ?)
                
                // if hit stop point
                if (newPathPoint.Equals(stopEndpoint))
                {
                    // remove extra part of segment 
                    segment.StepSize = i + 1;
                    return (segment, newPathPoint);
                }

                // if overlap with other start/stop point
                if (pcb.IsOneOfEndpoints(newPathPoint))
                {
                    // if stepSize can be shorter
                    if (i != 0)
                    {
                        // make shorter to avoid overlapping with endpoint 
                        segment.StepSize = i;
                        return (segment, newPathPoint);
                    }
                    else
                    {
                        //TODO: need to rand new direction
                    }
                }

                lastPathPoint = newPathPoint;
            }
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

        private Direction RandDirection()
        {
            return (Direction) _random.Next(1, 5);
        }

        private int RandStepSize()
        {
            return _random.Next(StepSizeMin, StepSizeMax + 1);
        }
    }
}