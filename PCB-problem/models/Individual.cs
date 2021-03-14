using System.Collections.Generic;
using System.Linq;

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

        private bool Equals(Individual other)
        {
            foreach (var endpoint in _paths.Keys)
            {
                if (other._paths.ContainsKey(endpoint))
                {
                    var path = GetPath(endpoint);
                    var otherPath = other.GetPath(endpoint);
                    if (!otherPath.Equals(path))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Individual) obj);
        }

        public override int GetHashCode()
        {
            return (_paths != null ? _paths.GetHashCode() : 0);
        }
    }
}