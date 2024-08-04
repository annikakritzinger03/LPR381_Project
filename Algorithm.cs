using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal static class Algorithm
    {
        public static List<Table> PrimalSimplex(Model model)
        {
            List<Table> tableList = new List<Table>();
            return tableList;
        }
        public static List<Table> RevisedPrimalSimplex(Model model)
        {
            List<Table> tableList = new List<Table>();
            return tableList;
        }
        public static List<Table> BranchBound(Model model)
        {
            List<Table> tableList = new List<Table>();
            return tableList;
        }
        public static List<Table> CuttingPlane(Model model)
        {
            List<Table> tableList = new List<Table>();
            return tableList;
        }

        public static List<KnapsackTable> Knapsack(Model model)
        {
            //Cases where Knapsack Algorithm would not work
            if (model.Constraints.Count != 1)
            {
                Console.WriteLine("This problem cannot be solved with Knapsack - there may only be one constraint.");
                return null;
            }
            
            foreach(string restriction in model.SignRestrictions)
            {
                if (restriction != "bin")
                {
                    Console.WriteLine("This problem cannot be solved with Knapsack - decision variables are not binary.");
                    return null;
                }
            }

            List<KnapsackTable> tableList = new List<KnapsackTable>();          

            //Determine the rank of variables and display relevant information
            List<double> ratios = new List<double>();
            List<int> ranks = new List<int>();


            for(int i=0; i<model.ObjectiveFunctionCoefficients.Length; i++)
            {
                ratios.Add(Math.Round(model.ObjectiveFunctionCoefficients[i] / model.Constraints[0].CoefficientsList[i],3));
            }

            var indexedRatios = ratios.Select((value, index) => new { Value = value, Index = index }).ToList();
            var sortedIndexedRatios = indexedRatios.OrderByDescending(x => x.Value).ToList();
           
            for (int i = 0; i < sortedIndexedRatios.Count; i++)
            {
                ranks.Add(sortedIndexedRatios[i].Index + 1);
            }

            PrintKnapsackRatioRanks(ratios, ranks, model.ObjectiveFunctionCoefficients, model.Constraints[0].CoefficientsList);

            //Create an intial Knapsack Table
            KnapsackTable initialTable = new KnapsackTable(ranks);
            initialTable.IsInitial = true;
           
            return tableList; 
        }

        public static void PrintKnapsackRatioRanks(List<double> ratioList, List<int> ranksList, double[] OFCoefficients, List<double> Constraintcoefficients )
        {
            double[] constraints =Constraintcoefficients.ToArray();
            double[] values = OFCoefficients;
            string[] columnNames = { "Items", "OF", "Constraint", "Ratio", "Rank", "Ordered Items" };

            double[] ratios = ratioList.ToArray();
            int[] ranks = ranksList.ToArray();
            string[] numbers = Enumerable.Range(1, constraints.Length).Select(i => "x" + i).ToArray();
            string[] orderedNumbers = ranks.Select(index => numbers[index-1]).ToArray();

            string[][] table = new string[constraints.Length + 1][];
            table[0] = columnNames;
            for (int i = 0; i < constraints.Length; i++)
            {
                table[i + 1] = new string[]{
                    numbers[i],
                    values[i].ToString(),
                    constraints[i].ToString(),
                    ratios[i].ToString(),
                    ranks[i].ToString(),
                    orderedNumbers[i]
                };
            }

            // Print the table with the needed spacing to ensure neat formatting
            int[] columnWidths = new int[6];
            for (int i = 0; i < table[0].Length; i++)
            {
                columnWidths[i] = table.Max(row => row[i].Length);
            }

            for (int i = 0; i < table.Length; i++)
            {
                for (int j = 0; j < table[i].Length; j++)
                {
                    Console.Write(table[i][j].PadRight(columnWidths[j] + 2));
                }
                Console.WriteLine();
            }
        }
    }
}
