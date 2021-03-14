using System;
using System.Collections.Generic;
using System.Linq;

namespace PCB_problem.solutionSearch
{
    public static class PenaltyFunction
    {
        public static int CalculatePenalty(IEnumerable<Path> paths, Pcb pcb, int w1, int w2, int w3, int w4, int w5)
        {
            var listOfPointsForEachPath = FlatAllPathsToListOfPoints(paths);
            var listOfAllPoints = listOfPointsForEachPath.SelectMany(i => i).ToList();
            var k1 = GetNumberOfCrosses(listOfAllPoints);
            var k2 = GetTotalPathsLength(listOfAllPoints);
            var k3 = GetTotalSegmentsQty(paths);
            var pointsOutsideBoard = GetPointsOutsideBoard(pcb.Width, pcb.Height, listOfAllPoints);
            var k4 = GetNumberOfPathsOutsideBoard(listOfPointsForEachPath, pointsOutsideBoard);
            var k5 = pointsOutsideBoard.Count();
            return w1 * k1 + w2 * k2 + w3 * k3 + w4 * k4 + w5 * k5;
        }

        private static int GetNumberOfCrosses(IList<Point> listOfPoints)
        {
            var allPointsQty = listOfPoints.Count();
            var distinctQty = listOfPoints.Distinct().Count();
            return allPointsQty - distinctQty;
        }

        private static int GetTotalPathsLength(IEnumerable<Point> listOfPoints)
        {
            return listOfPoints.Count();
        }

        private static int GetTotalSegmentsQty(IEnumerable<Path> paths)
        {
            return paths.Sum((path => path.Segments.Count()));
        }

        private static int GetNumberOfPathsOutsideBoard(IEnumerable<List<Point>> pointsForEachSegment,
            ICollection<Point> pointsOutsideBoard)
        {
            return pointsForEachSegment.Count(points => points.Any(pointsOutsideBoard.Contains));
        }

        private static List<Point> GetPointsOutsideBoard(int boardWidth, int boardHeight,
            IEnumerable<Point> listOfPoints)
        {
            return listOfPoints.Where(point => PointIsOutsideBoard(boardWidth, boardHeight, point)).ToList();
        }

        private static bool PointIsOutsideBoard(int boardWidth, int boardHeight, Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X >= boardWidth || point.Y >= boardHeight;
        }

        private static List<List<Point>> FlatAllPathsToListOfPoints(IEnumerable<Path> paths)
        {
            var listOfPoints = paths.Select(ParsePathToPoints).ToList();
            return listOfPoints;
        }

        private static List<Point> ParsePathToPoints(Path path)
        {
            var points = new List<Point> {path.startPoint};
            var lastPoint = path.startPoint;
            foreach (var segment in path.Segments)
            {
                var (xInc, yInc) = GetPositionIncValue(segment.Direction);
                for (var i = 0; i < segment.StepSize; i++)
                {
                    var newPoint = new Point(lastPoint.X + xInc, lastPoint.Y + yInc);
                    points.Add(newPoint);
                    lastPoint = newPoint;
                }
            }

            return points;
        }

        public static Individual GetIndividualWithMinPenaltyCost(IEnumerable<Individual> individuals, Pcb pcb, int w1,
            int w2,
            int w3,
            int w4, int w5)
        {
            var minPenalty = int.MaxValue;
            Individual bestIndividual = null;
            foreach (var individual in individuals)
            {
                var penalty = CalculatePenalty(individual.Paths, pcb, w1, w2, w3, w4, w5);
                if (penalty < minPenalty)
                {
                    minPenalty = penalty;
                    bestIndividual = individual;
                }
            }

            return bestIndividual;
        }

        private static (int, int) GetPositionIncValue(Direction direction)
        {
            return direction switch
            {
                Direction.Left => (-1, 0),
                Direction.Up => (0, 1),
                Direction.Right => (1, 0),
                Direction.Down => (0, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}