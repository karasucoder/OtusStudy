using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaExpressionsLearning
{
    public class Planet
    {
        public string Name { get; set; }

        public int SequenceNumber { get; set; }

        public double EquatorLength { get; set; }

        public Planet? PreviousPlanet { get; set; }
    }

}
