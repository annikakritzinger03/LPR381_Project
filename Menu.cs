using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal static class Menu
    {
        public static List<Model> modelsList = new List<Model>();
        public static List<Constraint> constraintList = new List<Constraint>();
        public static List<Table> tableList = new List<Table>();

        public static void DisplayMenu()
        {
            Console.WriteLine("----------Linear and Integer Programming Solver----------\n\n" +
                "\t1. Solve Linear/Integer Programming Model\n" +
                //Implement afterwards?
                "\t2. View All Solved Models\n" +
                "\t3. Exit Program\n");

            Console.Write("Please enter 1, 2 or 3: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int userResponse))
            {
                switch (userResponse)
                {
                    case 1:
                        Console.Clear();
                        SolveModel();
                        ReturnToMenu();
                        break;
                    case 2:
                        Console.Clear();
                        ViewSolvedModels();
                        ReturnToMenu();
                        break;
                    case 3:
                        Exit();
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine($"\"{userResponse}\" is not a valid option...\n" +
                            $"Please enter a number that is either 1, 2 or 3");
                        DisplayMenu();
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"\"{input}\" is not a valid option...\n" +
                    $"Please enter a number that is either 1, 2 or 3");
                DisplayMenu();
            }

        }

        static void ReturnToMenu()
        {
            Thread.Sleep(2000);
            Console.Write("Do you want to return to the main menu? (Y/N): ");
            string menu = Console.ReadLine();

            if (menu.ToUpper() == "Y")
            {
                Console.Clear();
                DisplayMenu();
            }
            else if (menu.ToUpper() == "N")
            {
                Exit();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"\"{menu}\" is not \"Y/N\".\n" +
                    $"Please enter either Y or N");
                ReturnToMenu();
            }
        }

        private static void SolveModel()
        {
            Model model = ReadTextFile();

            DisplayAlgorithmMenu();
            Console.Write("Please enter 1, 2, 3, 4 or 5: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int userResponse))
            {
                switch (userResponse)
                {
                    case 1:
                        Console.Clear();
                        //Primal Simplex algorithm implementation (canonical form, solve, display, output to .txt)
                        tableList = Algorithm.PrimalSimplex(model);
                        break;
                    case 2:
                        Console.Clear();
                        //Primal Simplex Revised algorithm implementation
                        tableList = Algorithm.RevisedPrimalSimplex(model);
                        break;
                    case 3:
                        Console.Clear();
                        //Branch and Bound Simplex algorithm implementation
                        tableList = Algorithm.BranchBound(model);
                        break;
                    case 4:
                        Console.Clear();
                        //Cutting Plane algorithm implementation
                        tableList = Algorithm.CuttingPlane(model);
                        break;
                    case 5:
                        Console.Clear();
                        //Branch and Bound Knapsack algorithm implementation
                        tableList = Algorithm.Knapsack(model);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine($"\"{userResponse}\" is not a valid option...\n" +
                            $"Please enter a number that is either 1, 2, 3, 4 or 5");
                        SolveModel();
                        break;
                }

                Table initialTable = new Table(), optimalTable = new Table();

                for (int i = 0; i < tableList.Count; i++)
                {
                    if (tableList[i].IsInitial == true)
                    {
                        initialTable = tableList[i];
                    }
                    else if (tableList[i].IsOptimal == true)
                    {
                        optimalTable = tableList[i];
                    }
                }

                //Implement Sensitivity analysis
                ConductSensitivityAnalysis(initialTable, optimalTable);
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"\"{input}\" is not a valid option...\n" +
                    $"Please enter a number that is either 1, 2 or 3");
                DisplayMenu();
            }

        }

        private static Model ReadTextFile()
        {
            Console.Write("Please provide the file path to the text file containing the LP/IP model: ");
            string filePath = Console.ReadLine();

            //Read text file contents and add to list of models
            Model model = FileHandler.ReadFromFile(filePath);
            modelsList.Add(model);

            return model;
        }

        static void DisplayAlgorithmMenu()
        {
            Console.Write("Please select the algorithm that will be implemented to solve the LP/IP model:\n\n" +
                "\t1. Primal Simplex\n" +
                "\t2. Primal Simplex Revised\n" +
                "\t3. Branch and Bound Simplex\n" +
                "\t4. Cutting Plane\n" +
                "\t5. Branch and Bound Knapsack\n");
        }

        private static void ConductSensitivityAnalysis(Table initialTable, Table optimalTable)
        {
            bool applyAnalysis = false;

            Thread.Sleep(2000);
            Console.Write("Do you want to apply sensitivity analysis to the solution? (Y/N): ");
            string sensitivityAnalysis = Console.ReadLine();

            if (sensitivityAnalysis.ToUpper() == "Y")
            {
                Console.Clear();
                applyAnalysis = true;
            }
            else if (sensitivityAnalysis.ToUpper() == "N")
            {
                Console.Clear();
                Console.WriteLine("You have selected to NOT apply sensitivity analysis to the solution.");

                ReturnToMenu();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"\"{sensitivityAnalysis}\" is not \"Y/N\".\n" +
                   $"Please enter either Y or N");
                ConductSensitivityAnalysis(initialTable, optimalTable);
            }

            if (applyAnalysis)
            {
                DisplaySensitivityAnalysisMenu();
                Console.Write("Please enter a number between 1 and 12 (both included): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int userResponse))
                {
                    switch (userResponse)
                    {
                        case 1:
                            Console.Clear();
                            //Display NBV Range
                            SensitivityAnalysis.DisplayNBVRange(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 2:
                            Console.Clear();
                            //Change NBV
                            SensitivityAnalysis.ChangeNBV(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 3:
                            Console.Clear();
                            //Display BV Range
                            SensitivityAnalysis.DisplayBVRange(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 4:
                            Console.Clear();
                            //Change BV
                            SensitivityAnalysis.ChangeBV(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 5:
                            Console.Clear();
                            //Display RHS Range
                            SensitivityAnalysis.DisplayRHSRange(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 6:
                            Console.Clear();
                            //Change RHS
                            SensitivityAnalysis.ChangeRHS(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 7:
                            Console.Clear();
                            //Display NBV Column Range
                            SensitivityAnalysis.DisplayNBVColumnRange(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 8:
                            Console.Clear();
                            //Change NBV Column
                            SensitivityAnalysis.ChangeNBVColumn(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 9:
                            Console.Clear();
                            //Add Activity
                            SensitivityAnalysis.AddActivity(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 10:
                            Console.Clear();
                            //Add Constraint
                            SensitivityAnalysis.AddConstraint(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 11:
                            Console.Clear();
                            //Display Shadow Prices
                            SensitivityAnalysis.DisplayShadowPrices(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        case 12:
                            Console.Clear();
                            //Apply and Solve Duality
                            SensitivityAnalysis.ApplyDuality(initialTable, optimalTable);
                            SensitivityAnalysis.SolveDuality(initialTable, optimalTable);
                            SensitivityAnalysis.StrongWeakDuality(initialTable, optimalTable);
                            ReturnToSensitivityAnalysisMenu();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine($"\"{userResponse}\" is not a valid option...\n" +
                                $"Please enter a number that is between 1 and 12 (both included)");
                            DisplaySensitivityAnalysisMenu();
                            break;
                    }
                }
            }
        }

        private static void ReturnToSensitivityAnalysisMenu()
        {
            Thread.Sleep(2000);
            Console.Write("Do you want to return to the sensitivity analysis menu? (Y/N): ");
            string menu = Console.ReadLine();

            if (menu.ToUpper() == "Y")
            {
                Console.Clear();
                DisplaySensitivityAnalysisMenu();
            }
            else if (menu.ToUpper() == "N")
            {
                ReturnToMenu();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"\"{menu}\" is not \"Y/N\".\n" +
                    $"Please enter either Y or N");
                ReturnToSensitivityAnalysisMenu();
            }
        }

        private static void DisplaySensitivityAnalysisMenu()
        {
            Console.Write("Please select what you would like to be displayed/changed in the solution:\n" +
                "\t(NBV = Non-Basic Variable, BV = Basic Variable, RHS = Right Hand Side)\n\n" +

                "\t1. Display NBV Range\n" +
                "\t2. Change NBV\n" +
                "\t3. Display BV Range\n" +
                "\t4. Change BV\n" +
                "\t5. Display RHS Range\n" +
                "\t6. Change RHS\n" +
                "\t7. Display NBV Column Range\n" +
                "\t8. Change NBV Column\n" +
                "\t9. Add an Activity (COlumn)\n" +
                "\t10. Add a COnstraint (Row)\n" +
                "\t11. Display Shadow Prices\n" +
                //Display in the end whether duality is weak or string)
                "\t12. Apply and Solve Duality\n");
        }

        private static void ViewSolvedModels()
        {
            throw new NotImplementedException();
        }

        static void Exit()
        {
            Console.Clear();
            Console.WriteLine("Thank you for checking out our program! :)");
            Environment.Exit(0);
        }
    }
}
