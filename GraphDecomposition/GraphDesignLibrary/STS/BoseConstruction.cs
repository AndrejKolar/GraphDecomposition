using System;
using System.Collections.Generic;
using System.Text;

namespace GraphDesignLibrary
{
    public class BoseConstruction : SteinerTripleSystem
    {
        public BoseConstruction(int numVertices)
        {
            this.OrderV = numVertices;
            this.OrderN = (OrderV - 3) / 6;
            
            TripleList = new List<Triple>();

            CreateQuasigroup(2 * OrderN + 1);

            CreateTypeOne();
            CreateTypeTwo();
        }


        private void CreateTypeOne()   //creating Type 1 triples
        {
            for (int i = 1; i <= 2 * OrderN + 1; i++)
            {
                TripleList.Add(new Triple() 
                {
                    FirstPair = CreatePair(i, 1),
                    SecondPair = CreatePair(i, 2),
                    ThirdPair = CreatePair(i, 3)
                });
            }
        }

        private void CreateTypeTwo()
        {
            for (int i = 1; i <= 2 * OrderN + 1; i++)
            {
                for (int j = i + 1; j <= 2 * OrderN + 1; j++)
                {
                    TripleList.Add(new Triple()
                    {
                        FirstPair = CreatePair(i, 1),
                        SecondPair = CreatePair(j, 1),
                        ThirdPair = CreatePair(GetElement(i, j), 2)
                    });

                    TripleList.Add(new Triple()
                    {
                        FirstPair = CreatePair(i, 2),
                        SecondPair = CreatePair(j, 2),
                        ThirdPair = CreatePair(GetElement(i, j), 3)  
                    });

                    TripleList.Add(new Triple()
                    {
                        FirstPair = CreatePair(i, 3),
                        SecondPair = CreatePair(j, 3),
                        ThirdPair = CreatePair(GetElement(i, j), 1)
                    });
                    
                }
            }
        }




    }
}
