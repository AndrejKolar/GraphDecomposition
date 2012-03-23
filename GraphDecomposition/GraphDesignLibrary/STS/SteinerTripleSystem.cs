using System;
using System.Collections.Generic;
using System.Text;

namespace GraphDesignLibrary
{
    public abstract class SteinerTripleSystem
    {
        private List<Triple> tripleList;
        private int orderV;
        private int orderN;
        private int[,] quasigroup;

        public int[,] Quasigroup
        {
            get { return quasigroup; }
            set { quasigroup = value; }
        }

        public int OrderN
        {
            get { return orderN; }
            set { orderN = value; }
        }

        public int OrderV
        {
            get { return orderV; }
            set { orderV = value; }
        }

        public List<Triple> TripleList
        {
            get { return tripleList; }
            set { tripleList = value; }
        }

        public void CreateQuasigroup(int order)
        {          

            Quasigroup = new Int32[order, order];

            int elem = 1;      //prvi red
            for (int i = 0; i < order; i++)
            {
                if (i % 2 == 0)
                {
                    Quasigroup[0, i] = elem;
                    elem++;
                }
            }
            for (int i = 0; i < order; i++)
            {
                if (i % 2 == 1)
                {
                    Quasigroup[0, i] = elem;
                    elem++;
                }

            }

            for (int i = 1; i < order; i++)  //ostali redovi
            {
                for (int j = 0; j < order; j++)  //shiftanje redova za jedan u desno
                {
                    Quasigroup[i, j] = Quasigroup[i - 1, (j + 1) % order];

                }
            }
        }

        public int GetElement(int i, int j)
        {
            return Quasigroup[i - 1, j - 1];
        }

        public string CreatePair(int first, int second)
        {
            return "(" + first.ToString() + "," + second.ToString() + ")";
        }
    }
}
