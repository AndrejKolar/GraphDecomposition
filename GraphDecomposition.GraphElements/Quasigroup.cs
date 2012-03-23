using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphDecomposition.GraphElements
{
    public class Quasigroup
    {
        /// <summary>
        /// Private field that stores the values of the quasigroup
        /// </summary>
        private int[,] quasigroupArray;

        /// <summary>
        /// Constructor creates a quasigroup matrix of the specified size
        /// </summary>
        /// <param name="order">Size of the quasigroup</param>
        public Quasigroup(int order)
        {
            quasigroupArray = new Int32[order, order];

            int elem = 1;
            for (int i = 0; i < order; i++)
            {
                if (i % 2 == 0)
                {
                    quasigroupArray[0, i] = elem;
                    elem++;
                }
            }

            for (int i = 0; i < order; i++)
            {
                if (i % 2 == 1)
                {
                    quasigroupArray[0, i] = elem;
                    elem++;
                }

            }

            for (int i = 1; i < order; i++)
            {
                for (int j = 0; j < order; j++)  
                {
                    quasigroupArray[i, j] = quasigroupArray[i - 1, (j + 1) % order];

                }
            }
        }

        /// <summary>
        /// Gets the element from the specified row and column
        /// </summary>
        /// <param name="i">Row number</param>
        /// <param name="j">Column number</param>
        /// <returns>Element of the quasigroup matrix</returns>
        public int GetElement(int i, int j)
        {
            return quasigroupArray[i - 1, j - 1];
        }
    }
}
