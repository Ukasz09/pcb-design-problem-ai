using System;
using System.Collections.Generic;
using System.Linq;

namespace PCB_problem.solutionSearch
{
    public static class PenaltyFunction
    {
        public static int CalculatePenalty(IEnumerable<Path> paths, Pcb pcb, int w1, int w2, int w3, int w4, int w5)
        {
            var listOfAllPoints = FlatAllPathsToListOfPoints(paths).ToList();
            var pointsOutsideBoard = GetPointsOutsideBoard(pcb.Width, pcb.Height, listOfAllPoints);
            var k1 = GetNumberOfCrosses(listOfAllPoints);
            var k2 = GetTotalPathsLength(listOfAllPoints);
            var k3 = GetTotalSegmentsQty(paths);
            var k4 = GetNumberOfPathsOutsideBoard(paths, pointsOutsideBoard);
            var k5 = pointsOutsideBoard.Count();
            // Console.WriteLine($"k1:{k1.ToString()}, k2:{k2.ToString()}, k3:{k3.ToString()}, k4:{k4.ToString()}, k5:{k5.ToString()}");
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

        private static int GetNumberOfPathsOutsideBoard(IEnumerable<Path> paths, ICollection<Point> pointsOutsideBoard)
        {
            return paths.Count(path => PathIsOutsideBoard(path, pointsOutsideBoard));
        }

        private static bool PathIsOutsideBoard(Path path, ICollection<Point> pointsOutsideBoard)
        {
            var pathPoints = ParsePathToPoints(path);
            return pathPoints.Any(pointsOutsideBoard.Contains);
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

        private static IEnumerable<Point> FlatAllPathsToListOfPoints(IEnumerable<Path> paths)
        {
            var listOfPoints = new List<Point>();
            listOfPoints = paths.Select(ParsePathToPoints)
                .Aggregate(listOfPoints, (current, pathPoints) => current.Concat(pathPoints).ToList());
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