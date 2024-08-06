using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal static class SensitivityAnalysis
    {
        //Keep in mind that these arguments may change, depending on what is needed
        public static void DisplayNBVRange(Table initialTable, Table optimalTable)
        {

        }
        public static void ChangeNBV(Table initialTable, Table optimalTable)
        {

        }
        public static void DisplayBVRange(Table initialTable, Table optimalTable)
        {

        }
        public static void ChangeBV(Table initialTable, Table optimalTable)
        {

        }
        public static void DisplayRHSRange(Table initialTable, Table optimalTable)
        {

        }
        public static void ChangeRHS(Table initialTable, Table optimalTable)
        {

        }
        public static void DisplayNBVColumnRange(Table initialTable, Table optimalTable)
        {

        }
        public static void ChangeNBVColumn(Table initialTable, Table optimalTable)
        {

        }
        public static void AddActivity(Table initialTable, Table optimalTable)
        {

        }
        public static void AddConstraint(Table initialTable, Table optimalTable)
        {

        }
        public static void DisplayShadowPrices(Table initialTable, Table optimalTable)
        {

        }

        public static Model ApplyDuality(Model model)
        {
            Model dualModel = new Model();

            //Primary Max/Min z becomes Dual Min/Max w
            dualModel.MinMax = (model.MinMax == "max") ? "min" : "max";
            dualModel.ZW = "w";

            // Initialize the constraints in the dual model
            dualModel.Constraints = new List<Constraint>();
            for (int i = 0; i < model.ObjectiveFunctionCoefficients.Length; i++)
            {
                dualModel.Constraints.Add(new Constraint
                {
                    CoefficientsList = new List<double>(new double[model.Constraints.Count])
                });
            }
            // Initialize the sign restrictions in the dual model
            var dualModelSignRestrictions = new List<String>();
            var dualModelObjectiveFunctionCoefficients = new List<Double>();

            //Objective function Coefficients become the Constraint RHS values (and vice versa)
            for (int i=0; i<model.ObjectiveFunctionCoefficients.Length; i++)
            {
                dualModel.Constraints[i].RHS = model.ObjectiveFunctionCoefficients[i];
            }
            for(int i=0; i < model.Constraints.Count; i++)
            {
                dualModelObjectiveFunctionCoefficients.Add(model.Constraints[i].RHS);
            }
            dualModel.ObjectiveFunctionCoefficients = dualModelObjectiveFunctionCoefficients.ToArray();

            //Constraint rows and columns are transposed
            for (int i = 0; i < model.Constraints.Count; i++)
            {
                for (int j = 0; j < model.Constraints[i].CoefficientsList.Count; j++)
                {
                    dualModel.Constraints[j].CoefficientsList[i] = model.Constraints[i].CoefficientsList[j];
                }
            }

            //Primal Constraint signs to Dual sign restrictions and vice versa
            for (int i=0; i<model.Constraints.Count; i++)
            {
                var primalConstraint = model.Constraints[i];
                string primalMinMax = model.MinMax;
                var primalSignRestriction = model.SignRestrictions[i];
                var dualConstraint = dualModel.Constraints[i];
                string dualMinMax = dualModel.MinMax;          

                //Primal sign restrictions to Dual constraint signs
                if (primalSignRestriction == "-")
                {
                    if (dualMinMax == "max")
                    {
                        dualConstraint.Operator = ">=";
                    }
                    else if (dualMinMax == "min")
                    {
                        dualConstraint.Operator = "<=";
                    }
                }
                else if (primalSignRestriction == "+")
                {
                    if (dualMinMax == "max")
                    {
                        dualConstraint.Operator = "<=";
                    }
                    else if (dualMinMax == "min")
                    {
                        dualConstraint.Operator = ">=";
                    }
                }
                else if (primalSignRestriction == "urs")
                {
                    dualConstraint.Operator = "=";
                }

                //Primal constraint signs to Dual sign restrictions
                if (primalConstraint.Operator == ">=")
                {
                    if (primalMinMax == "max")
                    {
                        dualModelSignRestrictions.Add("-");
                    }
                    else if (primalMinMax == "min")
                    {
                        dualModelSignRestrictions.Add("+");
                    }
                }
                else if (primalConstraint.Operator == "<=")
                {
                    if (primalMinMax == "max")
                    {
                        dualModelSignRestrictions.Add("+");
                    }
                    else if (primalMinMax == "min")
                    {
                        dualModelSignRestrictions.Add("-");
                    }
                }
                else if (primalConstraint.Operator == "=")
                {
                    dualModelSignRestrictions.Add("urs");
                }
            }

            dualModel.SignRestrictions = dualModelSignRestrictions.ToArray();

            return dualModel;
        }

        public static double SolveDuality(Model dualModel)
        {
            //Calculate and display Duality model with Primal Simplex
            List<Table> tableList = Algorithm.PrimalSimplex(dualModel);

            Console.WriteLine("-----------------------------------------------\n");
            foreach (Table table in tableList)
            {
                Console.WriteLine(table.ToString());
            }
            Console.WriteLine("-----------------------------------------------");

            //Retrieve and display Duality w value
            double dualWValue = Math.Round(tableList[tableList.Count - 1].ObjectiveFunction[tableList[tableList.Count - 1].ObjectiveFunction.Count - 1], 3);
            Console.WriteLine("Duality optimal solution w = " + dualWValue);

            return dualWValue;
        }

        public static void StrongWeakDuality(double dualWValuel, Table optimalTable)
        {
            bool hasStrongDuality = true;

            //Retrieve and display Primal z value
            double primalZValue = Math.Round(optimalTable.ObjectiveFunction[optimalTable.ObjectiveFunction.Count - 1], 3);
            Console.WriteLine("Primal optimal solution z = " + primalZValue);

            //Determine whether model has a weak or string duality
            double dualityGap = primalZValue - dualWValuel;
            if (dualityGap != 0)
            {
                hasStrongDuality = false;
            }

            if (hasStrongDuality)
            {
                Console.WriteLine("This model has a strong duality (The dual model is a mirrored image of the primal model).\n" +
                    "Thus, it can be used for further calculations.");
            }
            else
            {
                Console.WriteLine("This model has a weak duality.\n" +
                    "Thus, it cannot be used for further calculations.");
            }
        }
    }
}
