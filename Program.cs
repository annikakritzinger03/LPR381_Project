namespace LPR381_Project_GroupV5
{
    internal class Program
    {
        public static List<Model> modelsList = new List<Model>();
        public static List<Constraint> constraintList = new List<Constraint>();

        static void Main(string[] args)
        {
            DisplayMenu();
            Console.ReadKey();
        }

        static void DisplayMenu()
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

            if(menu.ToUpper() == "Y")
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
                Console.WriteLine($"\"{menu}\" is not a number.\n" +
                    $"Please enter a number that is either 1, 2 or 3");
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
                        Algorithm.PrimalSimplex(model);
                        break;
                    case 2:
                        Console.Clear();
                        //Primal Simplex Revised algorithm implementation
                        Algorithm.RevisedPrimalSimplex(model);
                        break;
                    case 3:
                        Console.Clear();
                        //Branch and Bound Simplex algorithm implementation
                        Algorithm.BranchBound(model);
                        break;
                    case 4:
                        Console.Clear();
                        //Cutting Plane algorithm implementation
                        Algorithm.CuttingPlane(model);
                        break;
                    case 5:
                        Console.Clear();
                        //Branch and Bound Knapsack algorithm implementation
                        Algorithm.Knapsack(model);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine($"\"{userResponse}\" is not a valid option...\n" +
                            $"Please enter a number that is either 1, 2, 3, 4 or 5");
                        SolveModel();
                        break;
                }

                //Implement Sensitivity analysis
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