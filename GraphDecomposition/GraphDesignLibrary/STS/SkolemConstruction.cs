using System;
using System.Collections.Generic;
using System.Text;

namespace GraphDesignLibrary
{
    public class SkolemConstruction : SteinerTripleSystem
    {
        public SkolemConstruction(int NumVertices)
        {
            this.OrderV = NumVertices;
            this.OrderN = (OrderV - 1) / 6;
            TripleList = new List<Triple>();

            CreateQuasigroup(2 * OrderN);            

            CreateTypeOne();

            CreateTypeTwo();

            CreateTypeThree();

        }

        private void CreateTypeOne()
        {
            for (int i = 1; i <= OrderN; i++)
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
            for (int i = 1; i <= OrderN; i++)
            {
                TripleList.Add(new Triple()
                {
                    FirstPair = CreatePair(0, 0),
                    SecondPair = CreatePair(OrderN + i, 1),
                    ThirdPair = CreatePair(i, 2)
                });

                TripleList.Add(new Triple()
                {
                    FirstPair = CreatePair(0, 0),
                    SecondPair = CreatePair(OrderN + i, 2),
                    ThirdPair = CreatePair(i, 3)
                });

                TripleList.Add(new Triple()
                {
                    FirstPair = CreatePair(0, 0),
                    SecondPair = CreatePair(OrderN + i, 3),
                    ThirdPair = CreatePair(i, 1)
                });  
                
            }
        }

        private void CreateTypeThree()
        {
            for (int i = 1; i <= 2 * OrderN; i++)
            {
                for (int j = i + 1; j <= 2 * OrderN; j++)
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
