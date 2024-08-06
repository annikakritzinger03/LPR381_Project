﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal class Constraint
    {
        //Coefficients of variables
        public List<Double> CoefficientsList { get; set; }
        //<=, >= or =
        public string Operator { get; set; }
        //Righthandside
        public double RHS { get; set; }


        public Constraint(){ }

        public Constraint(List<Double> coefficientsList, string ctrtOperator, double rhs)
        {
            CoefficientsList = coefficientsList;
            Operator = ctrtOperator;
            RHS = rhs;
        }

        public override string ToString()
        {
            string coefficients = "";
            foreach (var coefficient in CoefficientsList)
            {
                coefficients += "+" + coefficient.ToString();
            }

            string display = $"{coefficients} {Operator} {RHS}\n";
            return display;
        }
    }
}
