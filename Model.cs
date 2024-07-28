using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal class Model
    {

        public double[] ObjectiveFunctionCoefficients { get; set; }
        public List<Constraint> Constraints { get; set; }
        public string[] ConstraintRetrictions { get; set; }

        public Model(double[] objectiveFunctionCoefficients, List<Constraint> constraints, string[] constraintRetrictions)
        {
            ObjectiveFunctionCoefficients = objectiveFunctionCoefficients;
            Constraints = constraints;
            ConstraintRetrictions = constraintRetrictions;
        }
    }
}
