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

        private static List<List<object>> itemCombinationsValues = new List<List<object>>();
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
                ranks.Add(sortedIndexedRatios[i].Index);
            }

            List<double> values = model.ObjectiveFunctionCoefficients.ToList();
            double[] weights = model.Constraints[0].CoefficientsList.ToArray();

            PrintKnapsackRatioRanks(ratios, ranks, weights, values);

            // Call Knapsack method
            double limit = model.Constraints[0].RHS;
            Knapsack("", ranks, new List<List<int>>(), limit, values, weights);

            //Find the best combination
            var maxVal = itemCombinationsValues.Max(c => (double)c[1]);
            var maxIndex = itemCombinationsValues.FindIndex(c => (double)c[1] == maxVal);

            var bestCombinations = (List<List<int>>)itemCombinationsValues[maxIndex][0];
            for (int i = 0; i < bestCombinations.Count; i++)
            {
                bestCombinations[i][0] += 1; // Adjust index
            }

            // Print optimal knapsack items
            Console.WriteLine("Optimal knapsack items:");
            foreach (var item in bestCombinations)
            {
                Console.WriteLine(string.Join(", ", item));
            }
            Console.WriteLine("Total value: " + maxVal);




            //-------------------------------------------------------------------------------------
            //Create an intial Knapsack Table
            KnapsackTable initialTable = new KnapsackTable(ranks);
            initialTable.IsInitial = true;
           
            return tableList; 
        }

        public static void PrintKnapsackRatioRanks(List<double> ratioList, List<int> ranksList, double[] OFCoefficients, List<double> Constraintcoefficients )
        {
            double[] weights =Constraintcoefficients.ToArray();
            double[] values = OFCoefficients;
            string[] columnNames = { "Items", "OF", "Constraint", "Ratio", "Rank", "Ordered Items" };

            double[] ratios = ratioList.ToArray();
            int[] ranks = ranksList.ToArray();
            string[] numbers = Enumerable.Range(1, weights.Length).Select(i => "x" + i).ToArray();
            string[] orderedNumbers = ranks.Select(index => numbers[index]).ToArray();

            string[][] table = new string[weights.Length + 1][];
            table[0] = columnNames;
            for (int i = 0; i < weights.Length; i++)
            {
                table[i + 1] = new string[]{
                    numbers[i],
                    values[i].ToString(),
                    weights[i].ToString(),
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

        private static void Knapsack(
            string currBranchID,
            List<int> currItems,
            List<List<int>> markedItemsAssumedValues,
            double maxWeight,
            List<double> itemValues,
            double[] itemWeights)
        {
            // Initialization
            var answers = new List<double>();
            var cumSums = new List<double>();
            var variables = new List<string>();
            double prevWeightAvailable = maxWeight;
            int infeasible = 0;
            currBranchID = "";
            if (markedItemsAssumedValues.Count == 0)
            {
                currBranchID = "0";
            }

            double valuesTotal = 0;
            foreach (var itemPair in markedItemsAssumedValues) // Check all assumed values and their sums
            {
                if (itemPair.Equals(markedItemsAssumedValues.First()))
                {
                    currBranchID += (itemPair[1] + 1).ToString();
                }
                else
                {
                    currBranchID += "." + (itemPair[1] + 1).ToString();
                }
                variables.Add("x" + (itemPair[0] + 1).ToString() + "*");
                valuesTotal += itemValues[itemPair[0]] * itemPair[1];
                prevWeightAvailable -= itemWeights[itemPair[0]] * itemPair[1];
                cumSums.Add(prevWeightAvailable);
                answers.Add(itemPair[1]);
            }

            List<int> nextItems;
            if (markedItemsAssumedValues.Count == 0)
            {
                nextItems = currItems;
            }
            else
            {
                nextItems = currItems.Where(i => !markedItemsAssumedValues.Select(m => m[0]).Contains(i)).ToList();
            }

            if (prevWeightAvailable < 0)
            {
                infeasible = 2;
            }

            if (nextItems.Count == 0) // All values considered, label as infeasible or candidate
            {
                string branchString = markedItemsAssumedValues.Count == 0 ? "" : "x" + (markedItemsAssumedValues.Last()[0] + 1).ToString() + "=" + markedItemsAssumedValues.Last()[1].ToString();

                if (prevWeightAvailable < 0)
                {
                    infeasible = 2;
                }
                else
                {
                    infeasible = 1; //candidate
                    itemCombinationsValues.Add(new List<object> { markedItemsAssumedValues, valuesTotal });
                }

                DisplayCalculationTable(currBranchID, branchString, variables, cumSums, answers, infeasible, valuesTotal);
                return;
            }

            var branchForZero = new List<List<int>>();
            var branchForOne = new List<List<int>>();

            foreach (var item in nextItems)
            {
                variables.Add("x" + (item + 1).ToString());
                double initialWeightAvailable = prevWeightAvailable;
                prevWeightAvailable -= itemWeights[item];
                valuesTotal += itemValues[item];
                cumSums.Add(prevWeightAvailable);

                if (prevWeightAvailable < 0)
                {
                    if (answers.Last() == 1 || item == nextItems.First())
                    {
                        answers.Add(initialWeightAvailable / itemWeights[item]);
                        if (infeasible!=2)
                        {
                            branchForZero = new List<List<int>>(markedItemsAssumedValues) { new List<int> { item, 0 } };
                            branchForOne = new List<List<int>>(markedItemsAssumedValues) { new List<int> { item, 1 } };
                        }
                    }
                    else
                    {
                        answers.Add(0);
                    }
                }
                else
                {
                    answers.Add(1);
                }
            }

            if (prevWeightAvailable >= 0)
            {
                infeasible = 1;
                branchForOne.Clear();
                branchForZero.Clear();
            }

            string branchStr = markedItemsAssumedValues.Count == 0 ? "" : "x" + (markedItemsAssumedValues.Last()[0] + 1).ToString() + "=" + markedItemsAssumedValues.Last()[1].ToString();

            DisplayCalculationTable(currBranchID, branchStr, variables, cumSums, answers, infeasible, valuesTotal);

            if (infeasible==1)
            {
                foreach (var item in nextItems)
                {
                    markedItemsAssumedValues.Add(new List<int> { item, 1 });
                }
                itemCombinationsValues.Add(new List<object> { markedItemsAssumedValues, valuesTotal });
            }

            if (branchForZero.Count > 0)
            {
                Knapsack(currBranchID, currItems, branchForZero, maxWeight, itemValues, itemWeights);
                Knapsack(currBranchID, currItems, branchForOne, maxWeight, itemValues, itemWeights);
            }
        }

        private static void DisplayCalculationTable(
            string branchID,
            string branchString,
            List<string> variables,
            List<double> cumSums,
            List<double> answers,
            int candidateInfeasible,
            double valueTotal = 0)
        {
            // Display the calculation table for the branch
            Console.WriteLine("SP" + branchID + ": " + branchString);
            Console.WriteLine("Variables\tCumulative Sum\tAnswer");
            for (int i = 0; i < variables.Count; i++)
            {
                Console.WriteLine(variables[i] + "\t" + cumSums[i] + "\t" + answers[i]);
            }
            if (candidateInfeasible == 1)
            {
                Console.WriteLine("Candidate: " + valueTotal);
            }
            else if (candidateInfeasible == 2)
            {
                Console.WriteLine("Infeasible");
            }
            Console.WriteLine();
        }


    }
}
