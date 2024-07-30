using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal class Constraint
    {
        //xi, si, ei, ai,...
        public List<String> VariableList { get; set; }
        //Coefficients of variables
        public List<Double> CoefficientsList { get; set; }
        //<=, >= or =
        public string Operator { get; set; }
        //Righthandside
        public double RHS { get; set; }

        public Constraint(List<String> variableList, List<Double> coefficientsList, string ctrtOperator, double rhs)
        {
            VariableList = variableList;
            CoefficientsList = coefficientsList;
            Operator = ctrtOperator;
            RHS = rhs;
        }
    }
}
