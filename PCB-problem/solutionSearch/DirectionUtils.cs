namespace PCB_problem.solutionSearch
{
    public class DirectionUtils
    {
        public static Direction GetOppositeDirection(Direction direction)
        {
            var directionNumber = ((int) direction + 2) % 4;
            return (Direction) directionNumber;
        }
    }
}