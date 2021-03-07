using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PCB_problem
{
    public class DataUtils
    {
        public static string[] ReadDataFromFile(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);
            return lines;
        }

        // TODO: add data validation
        public static Pcb ConvertDataToPcb(string[] textLines, string separator)
        {
            var sizeData = textLines[0].Split(separator);
            var pcb = generatePcb(sizeData);
            for (var i = 1; i < textLines.Length; i++)
            {
                var endpointsData = textLines[i].Split(separator);
                var (startPoint, endPoint) = parseEndpointData(endpointsData);
                pcb.AddEndpoint(startPoint, endPoint);
            }

            return pcb;
        }

        /**
         * Return: (startPoint, endPoint)
         */
        private static (Point, Point) parseEndpointData(string[] endpointsData)
        {
            int.TryParse(endpointsData[0], out var startPointX);
            int.TryParse(endpointsData[1], out var startPointY);
            int.TryParse(endpointsData[2], out var endPointX);
            int.TryParse(endpointsData[3], out var endPointY);
            var startPoint = new Point(startPointX, startPointY);
            var endPoint = new Point(endPointX, endPointY);
            return (startPoint, endPoint);
        }

        private static Pcb generatePcb(string[] sizeData)
        {
            int.TryParse(sizeData[0], out var pcbWidth);
            int.TryParse(sizeData[1], out var pcbHeight);
            var pcb = new Pcb(pcbWidth, pcbHeight);
            return pcb;
        }

        public static void SaveSolution(Dictionary<(Point, Point), Path> solution, string filePath)
        {
            var lines = new string[solution.Count];
            int i = 0;
            foreach (var entry in solution)
            {
                var path = entry.Value;
                var pathStringArr = path.Segments.Select(segment => segment.ToString()).ToArray();
                lines[i] = $"{path.startPoint},[{string.Join(",", pathStringArr)}]";
                i++;
            }

            // var textLines = solution.Select(path =>
            //         path.Segments.Select(segment => segment.ToString()).ToArray())
            //     .ToArray();
            // File.WriteAllText(filePath, "");
            // foreach (var lines in textLines)
            // {
                // File.AppendAllLines(filePath, lines);
                File.WriteAllLines(filePath, lines);
            // }

            Console.WriteLine($"Solution correct saved in file: {filePath}");
        }
    }
}