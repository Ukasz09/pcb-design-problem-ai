using System;
using System.Collections.Generic;
using NLog;

namespace PCB_problem.solutionSearch
{
    public class RandomSearch
    {
        private const int MinStepSize = 1;
        private readonly int _maxStepSize;
        private const int MaxBendsQty = 20; // To avoid getting into inf loop of bad results
        private readonly Pcb _pcb;
        private readonly Logger _logger;

        private readonly Random _random;

        public RandomSearch(Pcb pcb, int? seed = null)
        {
            _pcb = pcb;
            _logger = LogManager.GetCurrentClassLogger();
            _random = seed != null ? new Random(seed.Value) : new Random();
            _maxStepSize = Math.Min(_pcb.Width, _pcb.Height); 
        }

        public Individual FindBestIndividual(int individualsQty, int w1, int w2, int w3, int w4, int w5)
        {
            if (individualsQty < 1)
            {
                throw new ArgumentException("Amount of individuals must cannot be less than 1", nameof(individualsQty));
            }

            _logger.Log(LogLevel.Info, "Searching best individual ...");
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var bestIndividual = FindIndividual();
            var minPenalty = PenaltyFunction.CalculatePenalty(bestIndividual.Paths, _pcb, w1, w2, w3, w4, w5);
            for (var i = 0; i < individualsQty - 1; i++)
            {
                var individual = FindIndividual();
                var penalty = PenaltyFunction.CalculatePenalty(individual.Paths, _pcb, w1, w2, w3, w4, w5);
                if (penalty < minPenalty)
                {
                    minPenalty = penalty;
                    bestIndividual = individual;
                }
            }

            watch.Stop();
            _logger.Log(
                LogLevel.Info,
                $"Best penalty: {minPenalty}, Execution time: {watch.ElapsedMilliseconds.ToString()} ms"
            );
            return bestIndividual;
        }

        public Individual FindIndividual()
        {
            var individual = new Individual();
            foreach (var (startPoint, stopPoint) in _pcb.Endpoints)
            {
                var path = FindPath(startPoint, stopPoint, _pcb, _maxStepSize);
                individual.AddPath((startPoint, stopPoint), path);
            }

            return individual;
        }

        private Path FindPath(Point startPoint, Point stopPoint, Pcb pcb, int maxStepSize)
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
                availableDirections.Remove(DirectionUtils.GetOppositeDirection(lastDirection));
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
            for (var i = 0; i < segment.StepSize; i++)
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
                        newPathPoint = GetNextPoint(newPathPoint,
                            DirectionUtils.GetOppositeDirection(segment.Direction));
                        return (segment, newPathPoint, direction);
                    }

                    // To make sure that next time we don't get the same direction
                    availableDirections.Remove(direction);
                    // To not allow turning back
                    var oppositeDirection = DirectionUtils.GetOppositeDirection(direction);
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
            return direction switch
            {
                Direction.Down => new Point(lastPoint.X, lastPoint.Y - 1),
                Direction.Up => new Point(lastPoint.X, lastPoint.Y + 1),
                Direction.Left => new Point(lastPoint.X - 1, lastPoint.Y),
                _ => new Point(lastPoint.X + 1, lastPoint.Y)
            };
        }

        private Direction RandDirection(IList<Direction> availableDirections)
        {
            var randIndex = _random.Next(availableDirections.Count);
            return availableDirections[randIndex];
        }

        private int RandStepSize(int stepSize)
        {
            return _random.Next(MinStepSize, stepSize + 1);
        }

        private List<Direction> GetListOfAllDirection()
        {
            return new List<Direction> {Direction.Left, Direction.Up, Direction.Right, Direction.Down};
        }
    }
}