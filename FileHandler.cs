using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal static class FileHandler
    {
        public static Model ReadFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            string[] arr = new string[lines.Length];
            int counter = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = Regex.Replace(lines[i], @"(<=|>=|=)(\d)", "$1 $2");
            }
            foreach (var line in lines)
            {
                arr[counter] = line;
                counter++;
            }

            string[] objectiveParts = arr[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            double[] OFC = new double[objectiveParts.Length - 1];
            for (int i = 1; i < objectiveParts.Length; i++)
            {
                OFC[i - 1] = double.Parse(objectiveParts[i]);
            }

            List<Constraint> constraintsList = new List<Constraint>();
            List<string> signRestrictionsList = new List<string>();

            // Parse constraints and constraint restrictions from remaining lines
            for (int i = 1; i < arr.Length - 1; i++)
            {
                string[] parts = arr[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int numberCount = 0;
                int operatorCount = 0;
                string operatorSymbol = "";

                foreach (var part in parts)
                {
                    if (double.TryParse(part, out _))
                    {
                        numberCount++;
                    }
                    else
                    {
                        operatorCount++;
                        operatorSymbol = part;
                    }
                }

                // Check if there is exactly one non-numeric part which is the operator
                if (numberCount == parts.Length - 1 && operatorCount == 1)
                {
                    // Store the entire constraint line as is
                    List<double> coefficientsList = new List<double>();
                    for (int j = 0; j < numberCount - 1; j++)
                    {
                        if (double.TryParse(parts[j], out _))
                        {
                            coefficientsList.Add(double.Parse(parts[j]));
                        }
               
                    }
                    double rhs = double.Parse(parts[parts.Length-1]);
                    constraintsList.Add(new Constraint(coefficientsList, operatorSymbol, rhs));
                }
            }

            // Handle the last line separately
            if (arr.Length > 1)
            {
                string[] parts = arr[arr.Length-1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach(string part in parts)
                {
                    signRestrictionsList.Add(part);
                }
            }

            // Convert list to array for constraint restrictions
            string[] signRestrictions = signRestrictionsList.ToArray();

            Model model = new Model(OFC, constraintsList, signRestrictions);
            return model;
        }

        public static void WriteToFile(string contents)
        {
            string fileName = "results.txt";

            try
            {
                // Create a new file and write the content to it
                using (StreamWriter writer = new StreamWriter(fileName, false))
                {
                    writer.WriteLine(contents);
                }

                Console.WriteLine($"Content successfully written to {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }

        public static void OutputToConsole(string contents)
        {

        }
    }

}
