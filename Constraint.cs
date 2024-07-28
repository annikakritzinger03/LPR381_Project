using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal class Constraint
    {
        public double[] Coefficients { get; set; }
        public string Operator { get; set; }
        public double RHS { get; set; }

        public Constraint(double[] coefficients, string ctrtOperator, double rhs)
        {
            Coefficients = coefficients;
            Operator = ctrtOperator;
            RHS = rhs;
        }
    }
}
