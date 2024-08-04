using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal class Table : Model
    {
        public bool IsInitial { get; set; }
        public bool IsOptimal { get; set; }
        public bool IsInfeasible { get; set; }
        public bool IsUnbounded { get; set; }
        public bool IsCandidate { get; set; }

        //Initialise a blank table
        public Table() : base()
        {
        }

        //Initialise a table with a model's attributes
        public Table (
            Model model,
            bool isInitial,
            bool isOptimal,
            bool isInfeasible,
            bool isUnbounded,
            bool isCandidate

        ) : base(model.ObjectiveFunctionCoefficients, model.Constraints, model.SignRestrictions)
        {
            IsInitial = isInitial;
            IsOptimal = isOptimal;
            IsInfeasible = isInfeasible;
            IsUnbounded = isUnbounded;
            IsCandidate = isCandidate;

            //Convert table to canonical form so that it can be solved
            ConvertToCanonical();
        }


        private void ConvertToCanonical()
        {
            for (int i = 0; i < Constraints.Count; i++)
            {
                if (Constraints[i].Operator == "<=")
                {
                    // For "<=" constraints, add an s variable

                    
                }
                else if (Constraints[i].Operator == ">=")
                {
                    // For ">=" constraints, add an "e" variable and multiply the constraint by -1
                    for (int j = 0; j < Constraints[i].CoefficientsList.Count; j++)
                    {
                        Constraints[i].CoefficientsList[j] *= -1;
                    }
                    Constraints[i].RHS *= -1;
                }
            }
        }

        public override string ToString()
        {
            //Display the given table in an appropriate manner for console output (as well as for  the .txt file)

            string tableDisplay = "";
            return tableDisplay;
        }
    }
}
