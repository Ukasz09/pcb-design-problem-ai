using System;

namespace PCB_problem
{
    public class Segment
    {
        public Direction Direction { get; }
        public int StepSize { get; set; }

        public Segment(Direction direction, int stepSize)
        {
            Direction = direction;
            StepSize = stepSize;
        }

        public override string ToString()
        {
            return $"[{((int) Direction).ToString()},{StepSize.ToString()}]";
        }

        private bool Equals(Segment other)
        {
            return Direction == other.Direction && StepSize == other.StepSize;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Segment) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Direction, StepSize);
        }
    }
}