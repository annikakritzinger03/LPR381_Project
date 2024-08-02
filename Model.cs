using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    //Base class: What is firstly extracted from the text file
    internal class Model
    {
        public double[] ObjectiveFunctionCoefficients { get; set; }
        public List<Constraint> Constraints { get; set; }
        public string[] SignRestrictions { get; set; }

        public Model()
        {
        }

        public Model(double[] objectiveFunctionCoefficients, List<Constraint> constraints, string[] signRetrictions)
        {
            ObjectiveFunctionCoefficients = objectiveFunctionCoefficients;
            Constraints = constraints;
            SignRestrictions = signRetrictions;
        }

        public override string ToString()
        {
            string objectiveFunction = "";
            foreach (double coefficient in ObjectiveFunctionCoefficients)
            {
                objectiveFunction += " " + coefficient;
            }

            string constraints = "";
            foreach (Constraint constraint in Constraints)
            {
                constraints += constraint.ToString();
            }

            string restrictions = "";
            foreach (string retriction in SignRestrictions)
            {
                restrictions += " " + retriction;
            }

            string display = $"Objective function: {objectiveFunction}\n\n" +
                $"Constraints: \n{constraints}\n" +
                $"Sign restrictions: {restrictions.ToString()}\n";
            return display;
        }

    }
}
