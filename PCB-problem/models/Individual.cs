using System.Collections.Generic;

namespace PCB_problem
{
    public class Individual
    {
        private readonly Dictionary<(Point, Point), Path> _paths;

        public Dictionary<(Point, Point), Path>.ValueCollection Paths => _paths.Values;

        public Individual() : this(new Dictionary<(Point, Point), Path>())
        {
        }

        public Individual(Dictionary<(Point, Point), Path> paths)
        {
            _paths = paths;
        }

        public override string ToString()
        {
            return _paths.ToString();
        }

        public void AddPath((Point, Point) startedPoint, Path path)
        {
            _paths.Add(startedPoint, path);
        }
    }
}