using System;
using System.Collections.Generic;
using System.Linq;

namespace PCB_problem.solutionSearch
{
    public class RandomSearch : IPcbSolution
    {
        private const int MinStepSize = 1;
        private const int MaxBendsQty = 10; // To avoid getting into inf loop of bad results

        private readonly Random _random = new Random();

        public Dictionary<(Point, Point), Path> FindBestSolution(Pcb pcb, int individualsQty)
        {
            if (individualsQty < 1)
            {
                throw new ArgumentException("Amount of individuals must cannot be less than 1", nameof(individualsQty));
            }

            var bestIndividual = FindSolution(pcb);
            var minPenalty = PenaltyFunction.CalculatePenalty(bestIndividual.Values, pcb);
            for (var i = 0; i < individualsQty - 1; i++)
            {
                var individual = FindSolution(pcb);
                var penalty = PenaltyFunction.CalculatePenalty(individual.Values, pcb);
                if (penalty < minPenalty)
                {
                    minPenalty = penalty;
                    bestIndividual = individual;
                }
            }

            return bestIndividual;
        }

        public Dictionary<(Point, Point), Path> FindSolution(Pcb pcb)
        {
            var solution = new Dictionary<(Point, Point), Path>();
            foreach (var (startPoint, stopPoint) in pcb.Endpoints)
            {
                var path = FindPath(startPoint, stopPoint, pcb);
                solution.Add((startPoint, stopPoint), path);
                Console.WriteLine($"Found solution for: ({startPoint},{stopPoint})");
            }

            return solution;
        }

        private Path FindPath(Point startPoint, Point stopPoint, Pcb pcb, int maxStepSize = 5)
        {
            var path = new Path(startPoint, stopPoint);
            var lastPathPoint = startPoint;
            var bendsQty = 0;
            // IMPORTANT: if there is no answer it will run forever
            var lastDirection = RandDirection(GetListOfAllDirection());
            do
            {
                var availableDirections = GetListOfAllDirection();
                // To make sure that next time we don't get opposite direction
                availableDirections.Remove(GetOppositeDirection(lastDirection));
                var (segment, point, direction) =
                    RandSegment(lastPathPoint, stopPoint, pcb, availableDirections, maxStepSize);
                var wasBend = lastDirection != direction;
                lastDirection = direction;

                // If need to clear path
                if (bendsQty >= MaxBendsQty || segment == null)
                {
                    path = new Path(startPoint, stopPoint);
                    lastPathPoint = startPoint;
                    bendsQty = 0;
                }
                else
                {
                    path.AddSegment(segment);
                    lastPathPoint = point;
                    if (wasBend)
                    {
                        bendsQty++;
                    }
                }
            } while (!lastPathPoint.Equals(stopPoint));

            return path;
        }

        /**
         * @return: (segment or null, new last path point)
         */
        private (Segment, Point, Direction) RandSegment(Point segmentStartPoint, Point stopPoint, Pcb pcb,
            IList<Direction> availableDirections, int maxStepSize)
        {
            var direction = RandDirection(availableDirections);
            var segment = new Segment(direction, RandStepSize(maxStepSize));
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
                    return (segment, newPathPoint, direction);
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
                        return (segment, newPathPoint, direction);
                    }

                    // To make sure that next time we don't get the same direction
                    availableDirections.Remove(direction);
                    // To not allow turning back
                    var oppositeDirection = GetOppositeDirection(direction);
                    if (availableDirections.Contains(oppositeDirection))
                    {
                        availableDirections.Remove(oppositeDirection);
                    }

                    // there is no other move
                    if (availableDirections.Count == 0)
                    {
                        return (null, segmentStartPoint, direction);
                    }

                    // change direction
                    return RandSegment(segmentStartPoint, stopPoint, pcb, availableDirections, maxStepSize);
                }

                lastPathPoint = newPathPoint;
            }

            return (segment, lastPathPoint, direction);
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

        private int RandStepSize(int stepSize)
        {
            return _random.Next(MinStepSize, stepSize + 1);
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