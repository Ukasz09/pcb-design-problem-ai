namespace PCB_problem
{
    public class DataUtils
    {
        public static string[] ReadDataFromFile(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            return lines;
        }

        // TODO: add data validation
        public static Pcb ConvertDataToPcb(string[] textLines, string separator)
        {
            var sizeData = textLines[0].Split(separator);
            int.TryParse(sizeData[0], out var pcbWidth);
            int.TryParse(sizeData[1], out var pcbHeight);
            var pcb = new Pcb(pcbWidth, pcbHeight);
            for (var i = 1; i < textLines.Length; i++)
            {
                var endpointsData = textLines[i].Split(separator);
                int.TryParse(endpointsData[0], out var startPointX);
                int.TryParse(endpointsData[1], out var startPointY);
                int.TryParse(endpointsData[2], out var endPointX);
                int.TryParse(endpointsData[3], out var endPointY);
                var startPoint = new Point(startPointX, startPointY);
                var endPoint = new Point(endPointX, endPointY);
                pcb.AddEndpoint(startPoint, endPoint);
            }

            return pcb;
        }
    }
}