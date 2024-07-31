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

            List<Constraint> constraints = new List<Constraint>();
            List<string> constraintRestrictionsList = new List<string>();

            // Parse constraints and constraint restrictions from remaining lines
            for (int i = 1; i < arr.Length; i++)
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
                    double[] coefficients = new double[numberCount - 1];
                    for (int j = 0; j < numberCount - 1; j++)
                    {
                        coefficients[j] = double.Parse(parts[j]);
                    }
                    double rhs = double.Parse(parts[numberCount]);
                    constraints.Add(new Constraint(coefficients, operatorSymbol, rhs));
                }
                else
                {
                    // Store in constraint restrictions
                    constraintRestrictionsList.Add(arr[i]);
                }
            }

            // Convert list to array for constraint restrictions
            string[] constraintRestrictions = constraintRestrictionsList.ToArray();

            Model model = new Model(OFC, constraints, constraintRestrictions);

            return model;
        }

        public static void WriteToFile(string contents)
        {

        }

        public static void OutputToConsole(string contents)
        {

        }
    }

}
