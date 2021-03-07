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
    }
}