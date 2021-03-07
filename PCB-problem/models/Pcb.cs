using System.Collections.Generic;
using System.Linq;

namespace PCB_problem
{
    public class Pcb
    {
        public int Width { get; }
        public int Height { get; }
        public IList<(Point, Point)> Endpoints { get; } = new List<(Point, Point)>();

        public Pcb(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void AddEndpoint(Point startPoint, Point endPoint)
        {
            Endpoints.Add((startPoint, endPoint));
        }

        public bool IsOneOfEndpoints(Point point)
        {
            var result = Endpoints.FirstOrDefault((pair) => pair.Item1.Equals(point) || pair.Item2.Equals(point));
            return result != default;
        }

        public override string ToString()
        {
            return $"{Width.ToString()}x{Height.ToString()}\n{string.Join("\n", Endpoints.ToArray())}";
        }
    }
}