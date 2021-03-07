using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

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
        public static Pcb ConvertDataToPcb(string[] textLines, string delimiter)
        {
            var sizeData = textLines[0].Split(delimiter);
            var pcb = generatePcb(sizeData);
            for (var i = 1; i < textLines.Length; i++)
            {
                var endpointsData = textLines[i].Split(delimiter);
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
            var lines = new string[solution.Count + 2];
            lines[0] = "[";
            int i = 1;
            foreach (var entry in solution)
            {
                var path = entry.Value;
                var pathStringArr = path.Segments.Select(segment => segment.ToString()).ToArray();
                lines[i] = $"[{path.startPoint},[{string.Join(",", pathStringArr)}]],";
                // If last line of result then remove comma
                if (i == solution.Count)
                {
                    lines[i] = lines[i].Remove(lines[i].Length - 1, 1);
                }

                i++;
            }

            lines[^1] = "]";
            File.WriteAllLines(filePath, lines);
            Console.WriteLine($"Solution correct saved in file: {filePath}");
        }

        public static void ParseEndpointsDataForUi(string[] data, string delimiter, string filePath)
        {
            var textLines = new string[data.Length + 1];
            textLines[0] = "[";
            // omit fst line with board size
            for (var i = 1; i < data.Length; i++)
            {
                var joinedArrText = string.Join(",", data[i].Split(delimiter));
                var arrSeparator = i != data.Length - 1 ? "," : "";
                textLines[i] = $"[{joinedArrText}]{arrSeparator}";
            }

            textLines[^1] = "]";
            File.WriteAllLines(filePath, textLines);
            Console.WriteLine($"Endpoints data correct parsed and saved in file: {filePath}");
        }
    }
}