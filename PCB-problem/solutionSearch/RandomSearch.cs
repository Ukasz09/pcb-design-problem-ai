using System;
using System.Collections.Generic;

namespace PCB_problem.solutionSearch
{
    public class RandomSearch : IPcbSolution
    {
        private const int MinStepSize = 1;
        private const int MaxStepSize = 5;
        private const int MaxBendsQty = 10; // To avoid getting into inf loop of bad results

        private readonly Random _random = new Random();

        public Dictionary<(Point,Point), Path> FindSolution(Pcb pcb)
        {
            var solution = new Dictionary<(Point,Point), Path>();
            foreach (var (startPoint, stopPoint) in pcb.Endpoints)
            {
                var path = FindPath(startPoint, stopPoint, pcb);
                solution.Add((startPoint,stopPoint),path);
                Console.WriteLine($"Found solution for: ({startPoint},{stopPoint})");
            }

            return solution;
        }

        private Path FindPath(Point startPoint, Point stopPoint, Pcb pcb)
        {
            var path = new Path(startPoint, stopPoint);
            var lastPathPoint = startPoint;
            var bendsQty = 0;
            // IMPORTANT: if there is no answer it will run forever
            do
            {
                var availableDirections = GetListOfAllDirection();
                var direction = RandDirection(availableDirections);
                // To make sure that next time we don't get opposite direction
                availableDirections.Remove(GetOppositeDirection(direction));

                var (segment, point) = RandSegment(lastPathPoint, stopPoint, pcb, availableDirections);
                // If need to clear path
                if (bendsQty >= MaxBendsQty || segment == null )
                {
                    path = new Path(startPoint, stopPoint);
                    lastPathPoint = startPoint;
                    bendsQty = 0;
                }
                else
                {
                    path.AddSegment(segment);
                    lastPathPoint = point;
                    bendsQty++;
                }
            } while (!lastPathPoint.Equals(stopPoint));

            return path;
        }

        /**
         * @return: (segment or null, new last path point)
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
                        newPathPoint = GetNextPoint(newPathPoint, GetOppositeDirection(segment.Direction));
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

             return (segment, lastPathPoint);
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
            return _random.Next(MinStepSize, MaxStepSize + 1);
        }

        private Direction GetOppositeDirection(Direction direction)
        {
            var directionNumber = ((int) direction + 2) % 4;
            return (Direction) directionNumber;
        }

        private List<Direction> GetListOfAllDirection()
        {
            return new List<Direction> {Direction.Left, Direction.Up, Direction.Right, Direction.Down};
        }
    }
}