using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphDecomposition.Interfaces
{
    public interface IDecompositionAlgorithm
    {
        GraphDecomposition.GraphElements.SteinerTripleSystem StartAlgorithm(int v);
    }
}
