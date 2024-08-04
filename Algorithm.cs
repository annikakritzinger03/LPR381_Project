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

        //Return type may change
        public static List<Table> Knapsack(Model model)
        {
            List<Table> tableList = new List<Table>();
            
            //Table initialTable = new Table(model, true, false, false, false, false)

            //Determine the rank of variables
            double[] ranks = new double[model.ObjectiveFunctionCoefficients.Length];


            for(int i=0; i<model.ObjectiveFunctionCoefficients.Length; i++)
            {
                ranks[i] = model.ObjectiveFunctionCoefficients[i] / model.Constraints[0].CoefficientsList[i];
                Console.WriteLine(ranks[i]);
            }


            return tableList;
        }
    }
}
