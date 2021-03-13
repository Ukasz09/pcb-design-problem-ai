using System.Collections.Generic;

namespace PCB_problem
{
    public class Individual
    {
        private readonly Dictionary<(Point, Point), Path> _paths; // <(startPoint,stopPoint), path>

        public Dictionary<(Point, Point), Path>.ValueCollection Paths => _paths.Values;
        public Dictionary<(Point, Point), Path>.KeyCollection StartPoints => _paths.Keys;


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

        public Path GetPath((Point, Point) endpoint)
        {
            return _paths[endpoint];
        }
    }
}