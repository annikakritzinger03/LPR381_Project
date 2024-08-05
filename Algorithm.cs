using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LPR381_Project_GroupV5
{
    internal static class Algorithm
    {
        public static List<Table> PrimalSimplex(Model model)
        {
            var tableList = new List<Table>();

            try
            {
                var tableau = PutModelInCanonicalForm(model);
                model.Results = new List<List<List<double>>>();  // Initialize the Results property
                Solve(model, tableau);
                model.IsSolved = true;

                // Convert results to tableList
                tableList = ConvertResultsToTables(model);

                // Save results to file
                string Path = "C:\\Users\\pathf\\Downloads\\results.txt";
                SaveResultsToFile(model, Path);
                Console.WriteLine($"Content successfully written to file \"{Path}\"");

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return tableList;
        }

        private static List<List<double>> PutModelInCanonicalForm(Model model)
        {
            // Check for any >= constraints and throw an exception
            foreach (var constraint in model.Constraints)
            {
                if (constraint.Operator == ">=")
                {
                    throw new InvalidOperationException("The Primal Simplex method does not support >= constraints.");
                }
            }

            List<List<double>> tableau = new List<List<double>>();
            tableau.Add(new List<double>());

            foreach (var coefficient in model.ObjectiveFunctionCoefficients)
            {
                tableau[0].Add(coefficient * -1);
            }

            for (int i = 0; i < model.Constraints.Count; i++)
            {
                tableau[0].Add(0);
            }
            tableau[0].Add(0); // RHS for the objective function row

            for (int i = 0; i < model.Constraints.Count; i++)
            {
                List<double> constraintValues = new List<double>(model.Constraints[i].CoefficientsList);

                for (int j = 0; j < model.Constraints.Count; j++)
                {
                    if (j == i)
                    {
                        switch (model.Constraints[i].Operator)
                        {
                            case "<=":
                                constraintValues.Add(1); // Slack variable
                                break;
                            case "=":
                                constraintValues.Add(0); // No slack/surplus for equality constraint
                                break;
                        }
                    }
                    else
                    {
                        constraintValues.Add(0);
                    }
                }

                constraintValues.Add(model.Constraints[i].RHS);
                tableau.Add(constraintValues);
            }

            return tableau;
        }

        private static void Solve(Model model, List<List<double>> tableau)
        {
            model.Results.Add(new List<List<double>>(tableau.Select(row => new List<double>(row))));
            Iterate(model, tableau);
        }

        private static bool IsOptimal(Model model, List<List<double>> tableau)
        {
            bool isOptimal = true;

            for (int i = 0; i < tableau[0].Count - 1; i++)
            {
                if (tableau[0][i] < 0)
                {
                    isOptimal = false;
                    break;
                }
            }

            return isOptimal;
        }

        private static void Iterate(Model model, List<List<double>> tableau)
        {
            if (IsOptimal(model, tableau))
                return;

            int pivotColumn = GetPivotColumn(model, tableau);
            int pivotRow = GetPivotRow(model, tableau, pivotColumn);

            if (pivotRow == -1)
                throw new Exception("There is no suitable row to pivot on - the problem is infeasible");

            Pivot(model, tableau, pivotRow, pivotColumn);
            model.Results.Add(new List<List<double>>(tableau.Select(row => new List<double>(row))));
            Iterate(model, tableau);
        }

        private static void Pivot(Model model, List<List<double>> tableau, int pivotRow, int pivotColumn)
        {
            double factor = 1 / tableau[pivotRow][pivotColumn];
            for (int i = 0; i < tableau[pivotRow].Count; i++)
            {
                tableau[pivotRow][i] *= factor;
            }

            for (int i = 0; i < tableau.Count; i++)
            {
                if (i != pivotRow)
                {
                    double pivotColumnValue = tableau[i][pivotColumn];
                    for (int j = 0; j < tableau[i].Count; j++)
                    {
                        tableau[i][j] -= pivotColumnValue * tableau[pivotRow][j];
                    }
                }
            }
        }

        private static int GetPivotColumn(Model model, List<List<double>> tableau)
        {
            int colIndex = -1;
            double mostNegative = 0;

            for (int i = 0; i < tableau[0].Count - 1; i++)
            {
                if (tableau[0][i] < 0 && tableau[0][i] < mostNegative)
                {
                    mostNegative = tableau[0][i];
                    colIndex = i;
                }
            }

            return colIndex;
        }

        private static int GetPivotRow(Model model, List<List<double>> tableau, int pivotColumn)
        {
            int rowIndex = -1;
            double lowestRatio = double.MaxValue;

            for (int i = 1; i < tableau.Count; i++)
            {
                if (tableau[i][pivotColumn] > 0)
                {
                    double ratio = tableau[i][tableau[i].Count - 1] / tableau[i][pivotColumn];
                    if (ratio < lowestRatio && ratio >= 0)
                    {
                        lowestRatio = ratio;
                        rowIndex = i;
                    }
                }
            }

            return rowIndex;
        }

        private static List<Table> ConvertResultsToTables(Model model)
        {
            var tableList = new List<Table>();

            for (int i = 0; i < model.Results.Count; i++)
            {
                var table = new Table
                {
                    ObjectiveFunction = model.Results[i][0],
                    ConstraintsMatrix = model.Results[i].Skip(1).Select(row => row.Take(row.Count - 1).ToList()).ToList(),
                    RightHandSide = model.Results[i].Skip(1).Select(row => row.Last()).ToList(),
                    IsInitial = i == 0,
                    IsOptimal = i == model.Results.Count - 1,
                };

                tableList.Add(table);
            }

            return tableList;
        }

        private static void SaveResultsToFile(Model model, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Solution Steps:");
                foreach (var step in model.Results)
                {
                    foreach (var row in step)
                    {
                        writer.WriteLine(string.Join("\t", row.Select(v => v.ToString("0.##"))));
                    }
                    writer.WriteLine(); // Add a blank line between tables for better readability
                }
            }
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
        private static string displayKnapsack = "";
        public static string Knapsack(Model model)
        {
            //Cases where Knapsack Algorithm would not work for the model
            if (model.Constraints.Count != 1)
            {
                string error = "This problem cannot be solved with Knapsack - there may only be one constraint.";
                Console.WriteLine(error);
                return error;
            }

            foreach(string restriction in model.SignRestrictions)
            {
                if (restriction != "bin")
                {
                    string error = "This problem cannot be solved with Knapsack - decision variables are not binary.";
                    Console.WriteLine(error);
                    return error;
                }
            }

            //Add initial model to the string that will be returned
            displayKnapsack += model.ToString();

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

            //Display Ratio and ranks in tabular format
            DisplayKnapsackRatioRanks(ratios, ranks, weights, values);

            // Call Knapsack method that will implement the algorithm
            double limit = model.Constraints[0].RHS;
            KnapsackAlgorithm("", ranks, new List<List<int>>(), limit, values, weights);

            //d=Display the optimal solution's decision variables and z value
            DisplayOptimalSolution();

            //Return string that contains the model, ratios/ranks, tables, and solution
            return displayKnapsack;
        }


        public static void DisplayKnapsackRatioRanks(List<double> ratioList, List<int> ranksList, double[] OFCoefficients, List<double> Constraintcoefficients )
        {
            double[] weights =Constraintcoefficients.ToArray();
            double[] values = OFCoefficients;
            string[] columnNames = { "Items", "OF", "Constraint", "Ratio", "Rank", "Ordered Items" };

            double[] ratios = ratioList.ToArray();
            int[] ranks = ranksList.ToArray();
            string[] numbers = Enumerable.Range(1, weights.Length).Select(i => "x" + i).ToArray();
            string[] orderedNumbers = ranks.Select(index => numbers[index]).ToArray();

            // Determine column widths
            int[] columnWidths = new int[]
            {
                Math.Max(columnNames[0].Length, numbers.Max(n => n.Length)),
                Math.Max(columnNames[1].Length, values.Max(v => v.ToString().Length)),
                Math.Max(columnNames[2].Length, weights.Max(w => w.ToString().Length)),
                Math.Max(columnNames[3].Length, ratios.Max(r => r.ToString().Length)),
                Math.Max(columnNames[4].Length, ranks.Max(r => r.ToString().Length)),
                Math.Max(columnNames[5].Length, orderedNumbers.Max(o => o.Length))
            };

            // Display the header
            Console.Write("-----------------------------------------------");
            string header = $"\n{columnNames[0].PadRight(columnWidths[0] + 2)}{columnNames[1].PadRight(columnWidths[1] + 2)}{columnNames[2].PadRight(columnWidths[2] + 2)}{columnNames[3].PadRight(columnWidths[3] + 2)}{columnNames[4].PadRight(columnWidths[4] + 2)}{columnNames[5].PadRight(columnWidths[5] + 2)}\n";
            Console.Write(header);
            displayKnapsack += header;

            // Display the rows
            string rows = "";
            for (int i = 0; i < weights.Length; i++)
            {
                string row = $"{numbers[i].PadRight(columnWidths[0] + 2)}{values[i].ToString().PadRight(columnWidths[1] + 2)}{weights[i].ToString().PadRight(columnWidths[2] + 2)}{ratios[i].ToString().PadRight(columnWidths[3] + 2)}{ranks[i].ToString().PadRight(columnWidths[4] + 2)}{orderedNumbers[i].PadRight(columnWidths[5] + 2)}\n";
                Console.Write(row);
                rows += row;
            }
            Console.WriteLine("-----------------------------------------------");
            displayKnapsack += rows;
        }

        private static void KnapsackAlgorithm(
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
                KnapsackAlgorithm(currBranchID, currItems, branchForZero, maxWeight, itemValues, itemWeights);
                KnapsackAlgorithm(currBranchID, currItems, branchForOne, maxWeight, itemValues, itemWeights);
            }
        }

        public static int candidateCount = 0;
        private static void DisplayCalculationTable(
            string branchID,
            string branchString,
            List<string> variables,
            List<double> cumSums,
            List<double> answers,
            int candidateInfeasible,
            double valueTotal = 0)
        {
            // Display the branch information
            string branchName = $"SP{branchID}: {branchString}\n";
            Console.Write(branchName);
            displayKnapsack += "\n" + branchName;

            // Determine column widths
            int varWidth = Math.Max("Variables".Length, variables.Max(v => v.Length));
            int cumSumWidth = Math.Max("Cumulative Sum".Length, cumSums.Max(c => c.ToString().Length));
            int answerWidth = Math.Max("Answer".Length, answers.Max(a => a.ToString().Length));

            // Display the header

            string header = $"{"Variables".PadRight(varWidth)}  {"Cumulative Sum".PadRight(cumSumWidth)}  {"Answer".PadRight(answerWidth)}\n";
            Console.Write(header);
            displayKnapsack += header;

            // Display the rows
            string rows = "";
            for (int i = 0; i < variables.Count; i++)
            {
                string row = $"{variables[i].PadRight(varWidth)}  {cumSums[i].ToString().PadRight(cumSumWidth)}  {Math.Round(answers[i], 3).ToString().PadRight(answerWidth)}\n";
                Console.Write(row);
                rows += row;
            }
            displayKnapsack += rows;

            // Display candidate infeasibility information
            if (candidateInfeasible == 1)
            {
                candidateCount++;
                string optimalCandidate = $"Candidate {candidateCount}: {valueTotal}\n";
                Console.Write(optimalCandidate);
                displayKnapsack += optimalCandidate;
            }
            else if (candidateInfeasible == 2)
            {
                Console.WriteLine("Infeasible");
                displayKnapsack += "Infeasible\n";
            }
            Console.WriteLine();
        }
        private static void DisplayOptimalSolution()
        {
            //Find the best combination
            var maxVal = itemCombinationsValues.Max(c => (double)c[1]);
            var maxIndex = itemCombinationsValues.FindIndex(c => (double)c[1] == maxVal);

            var bestCombinations = (List<List<int>>)itemCombinationsValues[maxIndex][0];
            for (int i = 0; i < bestCombinations.Count; i++)
            {
                bestCombinations[i][0] += 1; // Adjust index
            }

            // Print optimal knapsack items
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Optimal knapsack solution:");
            displayKnapsack += "\nOptimal knapsack solution:\n";

            string combinations = "";
            foreach (var item in bestCombinations)
            {
                string combination = string.Join(" = ", item);
                Console.WriteLine(combination);
                combinations += combination + "\n";
            }
            displayKnapsack += combinations;

            string optimalZ = "Optimal z value: " + maxVal;
            Console.WriteLine(optimalZ);
            displayKnapsack += optimalZ + "\n";
            Console.WriteLine("-----------------------------------------------");
        }
    }
}
