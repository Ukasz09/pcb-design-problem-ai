using System;
using System.Collections.Generic;

namespace PCB_problem.solutionSearch.GeneticAlgorithm
{
    public class MutationA
    {
        private readonly double _mutationProbability;
        private readonly Random _random;

        public MutationA(double mutationProbability)
        {
            _mutationProbability = mutationProbability;
            _random = new Random();
        }

        public void Mutate(Individual individual)
        {
            foreach (var path in individual.Paths)
            {
                var randValue = _random.NextDouble();
                if (randValue <= _mutationProbability)
                {
                    MutatePath(path);
                }
            }
        }

        private void MutatePath(Path path)
        {
            // rand segment
            var drawnIndex = _random.Next(path.Segments.Count);
            var segment = path.Segments[drawnIndex];

            // rand direction
            var randDomain = (segment.Direction == Direction.Left || segment.Direction == Direction.Right)
                ? new[] {Direction.Down, Direction.Up}
                : new[] {Direction.Left, Direction.Right};
            var moveDirection = randDomain[_random.Next(1)];

            // Previous segment
            if (drawnIndex == 0)
            {
                InsertNewPrevSegment(path.Segments, moveDirection, drawnIndex);
                drawnIndex++; // index of actual segment moved about 1
            }
            else
            {
                var previousSegment = path.Segments[drawnIndex - 1];
                if (previousSegment.Direction == moveDirection)
                {
                    previousSegment.StepSize += 1;
                }
                else if (previousSegment.Direction == DirectionUtils.GetOppositeDirection(moveDirection))
                {
                    previousSegment.StepSize -= 1;
                    if (previousSegment.StepSize <= 0)
                    {
                        path.Segments.RemoveAt(drawnIndex - 1);
                        drawnIndex--;
                    }
                }
                else
                {
                    InsertNewPrevSegment(path.Segments, moveDirection, drawnIndex);
                    drawnIndex++; // index of actual segment moved about 1
                }
            }


            // Next segment
            if (drawnIndex == path.Segments.Count - 1)
            {
                InsertNewNextSegment(path.Segments, moveDirection, drawnIndex);
            }
            else
            {
                var nextSegment = path.Segments[drawnIndex + 1];
                if (nextSegment.Direction == moveDirection)
                {
                    nextSegment.StepSize -= 1;
                    if (nextSegment.StepSize <= 0)
                    {
                        path.Segments.RemoveAt(drawnIndex + 1);
                    }
                }
                else if (nextSegment.Direction == DirectionUtils.GetOppositeDirection(moveDirection))
                {
                    nextSegment.StepSize += 1;
                }
                else
                {
                    InsertNewNextSegment(path.Segments, moveDirection, drawnIndex);
                }
            }
        }

        private void InsertNewPrevSegment(IList<Segment> segments, Direction moveDirection, int actualSegmentIndex)
        {
            var newExtraPrevSegment = new Segment(moveDirection, 1);
            segments.Insert(actualSegmentIndex, newExtraPrevSegment);
            }

        private void InsertNewNextSegment(IList<Segment> segments, Direction moveDirection, int actualSegmentIndex)
        {
            var newExtraNextSegment = new Segment(DirectionUtils.GetOppositeDirection(moveDirection), 1);
            segments.Insert(actualSegmentIndex + 1, newExtraNextSegment);
        }
    }
}