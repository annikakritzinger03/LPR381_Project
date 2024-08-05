using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPR381_Project_GroupV5
{
    internal class Table : Model
    {
        public bool IsInitial { get; set; }
        public bool IsOptimal { get; set; }
        public bool IsInfeasible { get; set; }
        public bool IsUnbounded { get; set; }
        public bool IsCandidate { get; set; }
        public List<double> ObjectiveFunction { get; set; }
        public List<List<double>> ConstraintsMatrix { get; set; }
        public List<double> RightHandSide { get; set; }

        public Table() : base()
        {
            ObjectiveFunction = new List<double>();
            ConstraintsMatrix = new List<List<double>>();
            RightHandSide = new List<double>();
            Results = new List<List<List<double>>>();  // Ensure Results is initialized
        }

        public Table(Model model, bool isInitial, bool isOptimal, bool isInfeasible, bool isUnbounded, bool isCandidate) : base(model.ObjectiveFunctionCoefficients, model.Constraints, model.SignRestrictions)
        {
            IsInitial = isInitial;
            IsOptimal = isOptimal;
            IsInfeasible = isInfeasible;
            IsUnbounded = isUnbounded;
            IsCandidate = isCandidate;

            ObjectiveFunction = model.ObjectiveFunctionCoefficients.ToList();
            ConstraintsMatrix = model.Constraints.Select(c => c.CoefficientsList.ToList()).ToList();
            RightHandSide = model.Constraints.Select(c => c.RHS).ToList();

            Results = new List<List<List<double>>>();  // Ensure Results is initialized

            ConvertToCanonical();
        }

        private void ConvertToCanonical()
        {
            for (int i = 0; i < Constraints.Count; i++)
            {
                if (Constraints[i].Operator == "<=")
                {
                    // Add slack variable
                    ConstraintsMatrix[i].Add(1);
                }
                else if (Constraints[i].Operator == ">=")
                {
                    // Add surplus variable and multiply constraint by -1
                    for (int j = 0; j < ConstraintsMatrix[i].Count; j++)
                    {
                        ConstraintsMatrix[i][j] *= -1;
                    }
                    RightHandSide[i] *= -1;
                    ConstraintsMatrix[i].Add(1);
                }
                else
                {
                    // Add zero variable
                    ConstraintsMatrix[i].Add(0);
                }
            }

            // Add zeros to the objective function for slack/surplus variables
            for (int i = 0; i < Constraints.Count; i++)
            {
                ObjectiveFunction.Add(0);
            }
        }

        public void AddResultStep(List<List<double>> step)
        {
            Results.Add(step);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Join("\t", ObjectiveFunction.Select(c => c.ToString("0.##"))));

            for (int i = 0; i < ConstraintsMatrix.Count; i++)
            {
                sb.AppendLine(string.Join("\t", ConstraintsMatrix[i].Select(c => c.ToString("0.##"))) + "\t" + RightHandSide[i].ToString("0.##"));
            }

            return sb.ToString();
        }
    }
}
