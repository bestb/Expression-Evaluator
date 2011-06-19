﻿using Vanderbilt.Biostatistics.Wfccm2;

namespace ExpressionEvaluator.Procedures
{
    class Equal : Operator
    {
        public Equal(int precedance) : base("==", precedance, 2)
        {
            _name2 = "Equal";
            DoubleDoubleBool = (x, y) => x == y;
            BoolBoolBool = (x, y) => x == y;
        }
    }
}