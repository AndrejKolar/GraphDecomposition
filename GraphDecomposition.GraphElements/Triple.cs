using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphDecomposition.GraphElements
{
    /// <summary>
    /// Structure for modeling a single triple 
    /// </summary>
    public struct Triple
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Triple(int x, int y, int z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

    }
}
