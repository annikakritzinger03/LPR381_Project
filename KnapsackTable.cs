using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project_GroupV5
{
    internal class KnapsackTable
    {
        public string TableNum { get; set; }
        public List<int> VariablesOrder { get; set; }
        public List<int> SubtractedAnswer { get; set; }
        public List<double> Results { get; set; }
        public bool IsInitial { get; set; }
        public bool IsOptimal { get; set; }
        public bool IsInfeasible { get; set; }
        public bool IsCandidate { get; set; }


        //Create a table instance with only the rank
        public KnapsackTable(List<int> variablesOrder) {
            VariablesOrder = variablesOrder;
        }

        public override string ToString()
        {
            string display = "";

            display += $"SP{TableNum}\tSub Results\tResults\n";
            for (int i = 0; i < VariablesOrder.Count - 1; i++)
            {
                display += $"x{VariablesOrder[i]}\t{SubtractedAnswer[i]}\t{Results[i]}";
        
            }

            return display;
        }
    }
}
