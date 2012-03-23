using System;
using System.Collections.Generic;
using System.Text;

namespace GraphDesignLibrary
{
    public struct Triple
    {
        private string firstPair;

        public string FirstPair
        {
            get { return firstPair; }
            set { firstPair = value; }
        }
        private string secondPair;

        public string SecondPair
        {
            get { return secondPair; }
            set { secondPair = value; }
        }
        private string thirdPair;

        public string ThirdPair
        {
            get { return thirdPair; }
            set { thirdPair = value; }
        }
    }
}
