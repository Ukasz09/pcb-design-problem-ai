using System.Collections.Generic;
using System.Linq;

namespace PCB_problem
{
    public class Pcb
    {
        public int Width { get; set; }
        public int Height { get; set; }
        private IList<(Point, Point)> _endpoints = new List<(Point, Point)>();

        public Pcb(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void AddEndpoint(Point startPoint, Point endPoint)
        {
            _endpoints.Add((startPoint, endPoint));
        }

        public override string ToString()
        {
            return $"{Width.ToString()}x{Height.ToString()}\n{string.Join("\n", _endpoints.ToArray())}";
        }
    }
}