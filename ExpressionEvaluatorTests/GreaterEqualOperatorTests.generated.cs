﻿// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Vanderbilt.Biostatistics.Wfccm2;

namespace ExpressionEvaluatorTests
{
    [TestFixture]
    public class GreaterEqualOperatorTests
    {
        Expression func;

        [SetUp]
        public void init()
        { this.func = new Expression(""); }

        [TearDown]
        public void clear()
        { func.Clear(); }
        
        [Test]
        public void GreaterEqualOperator_PositiveWholeWithPositiveWhole_IsCorrect()
        {
            func.Function = "2 >= 2";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithPositiveFraction_IsCorrect()
        {
            func.Function = "2 >= 0.5";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithNegativeWhole_IsCorrect()
        {
            func.Function = "2 >= -2";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithNegativeFraction_IsCorrect()
        {
            func.Function = "2 >= -0.5";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithPositiveWhole_IsCorrect()
        {
            func.Function = "0.5 >= 2";
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithPositiveFraction_IsCorrect()
        {
            func.Function = "0.5 >= 0.5";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithNegativeWhole_IsCorrect()
        {
            func.Function = "0.5 >= -2";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithNegativeFraction_IsCorrect()
        {
            func.Function = "0.5 >= -0.5";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithPositiveWhole_IsCorrect()
        {
            func.Function = "-2 >= 2";
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithPositiveFraction_IsCorrect()
        {
            func.Function = "-2 >= 0.5";
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithNegativeWhole_IsCorrect()
        {
            func.Function = "-2 >= -2";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithNegativeFraction_IsCorrect()
        {
            func.Function = "-2 >= -0.5";
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithPositiveWhole_IsCorrect()
        {
            func.Function = "-0.5 >= 2";
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithPositiveFraction_IsCorrect()
        {
            func.Function = "-0.5 >= 0.5";
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithNegativeWhole_IsCorrect()
        {
            func.Function = "-0.5 >= -2";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithNegativeFraction_IsCorrect()
        {
            func.Function = "-0.5 >= -0.5";
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithPositiveWholeWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= 2";
			func.AddSetVariable("a", 2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithPositiveFractionWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= 0.5";
			func.AddSetVariable("a", 2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithNegativeWholeWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= -2";
			func.AddSetVariable("a", 2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithNegativeFractionWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= -0.5";
			func.AddSetVariable("a", 2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithPositiveWholeWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= 2";
			func.AddSetVariable("a", 0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithPositiveFractionWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= 0.5";
			func.AddSetVariable("a", 0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithNegativeWholeWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= -2";
			func.AddSetVariable("a", 0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithNegativeFractionWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= -0.5";
			func.AddSetVariable("a", 0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithPositiveWholeWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= 2";
			func.AddSetVariable("a", -2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithPositiveFractionWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= 0.5";
			func.AddSetVariable("a", -2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithNegativeWholeWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= -2";
			func.AddSetVariable("a", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithNegativeFractionWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= -0.5";
			func.AddSetVariable("a", -2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithPositiveWholeWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= 2";
			func.AddSetVariable("a", -0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithPositiveFractionWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= 0.5";
			func.AddSetVariable("a", -0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithNegativeWholeWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= -2";
			func.AddSetVariable("a", -0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithNegativeFractionWithLeftVariable_IsCorrect()
        {
            func.Function = "a >= -0.5";
			func.AddSetVariable("a", -0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithPositiveWholeWithRightVariable_IsCorrect()
        {
            func.Function = "2 >= a";
			func.AddSetVariable("a", 2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithPositiveFractionWithRightVariable_IsCorrect()
        {
            func.Function = "2 >= a";
			func.AddSetVariable("a", 0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithNegativeWholeWithRightVariable_IsCorrect()
        {
            func.Function = "2 >= a";
			func.AddSetVariable("a", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithNegativeFractionWithRightVariable_IsCorrect()
        {
            func.Function = "2 >= a";
			func.AddSetVariable("a", -0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithPositiveWholeWithRightVariable_IsCorrect()
        {
            func.Function = "0.5 >= a";
			func.AddSetVariable("a", 2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithPositiveFractionWithRightVariable_IsCorrect()
        {
            func.Function = "0.5 >= a";
			func.AddSetVariable("a", 0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithNegativeWholeWithRightVariable_IsCorrect()
        {
            func.Function = "0.5 >= a";
			func.AddSetVariable("a", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithNegativeFractionWithRightVariable_IsCorrect()
        {
            func.Function = "0.5 >= a";
			func.AddSetVariable("a", -0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithPositiveWholeWithRightVariable_IsCorrect()
        {
            func.Function = "-2 >= a";
			func.AddSetVariable("a", 2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithPositiveFractionWithRightVariable_IsCorrect()
        {
            func.Function = "-2 >= a";
			func.AddSetVariable("a", 0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithNegativeWholeWithRightVariable_IsCorrect()
        {
            func.Function = "-2 >= a";
			func.AddSetVariable("a", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithNegativeFractionWithRightVariable_IsCorrect()
        {
            func.Function = "-2 >= a";
			func.AddSetVariable("a", -0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithPositiveWholeWithRightVariable_IsCorrect()
        {
            func.Function = "-0.5 >= a";
			func.AddSetVariable("a", 2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithPositiveFractionWithRightVariable_IsCorrect()
        {
            func.Function = "-0.5 >= a";
			func.AddSetVariable("a", 0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithNegativeWholeWithRightVariable_IsCorrect()
        {
            func.Function = "-0.5 >= a";
			func.AddSetVariable("a", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithNegativeFractionWithRightVariable_IsCorrect()
        {
            func.Function = "-0.5 >= a";
			func.AddSetVariable("a", -0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithPositiveWholeWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", 2d);
			func.AddSetVariable("b", 2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithPositiveFractionWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", 2d);
			func.AddSetVariable("b", 0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithNegativeWholeWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", 2d);
			func.AddSetVariable("b", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveWholeWithNegativeFractionWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", 2d);
			func.AddSetVariable("b", -0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithPositiveWholeWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", 0.5d);
			func.AddSetVariable("b", 2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithPositiveFractionWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", 0.5d);
			func.AddSetVariable("b", 0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithNegativeWholeWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", 0.5d);
			func.AddSetVariable("b", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_PositiveFractionWithNegativeFractionWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", 0.5d);
			func.AddSetVariable("b", -0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithPositiveWholeWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", -2d);
			func.AddSetVariable("b", 2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithPositiveFractionWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", -2d);
			func.AddSetVariable("b", 0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithNegativeWholeWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", -2d);
			func.AddSetVariable("b", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeWholeWithNegativeFractionWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", -2d);
			func.AddSetVariable("b", -0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithPositiveWholeWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", -0.5d);
			func.AddSetVariable("b", 2d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithPositiveFractionWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", -0.5d);
			func.AddSetVariable("b", 0.5d);
            Assert.AreEqual(false, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithNegativeWholeWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", -0.5d);
			func.AddSetVariable("b", -2d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        public void GreaterEqualOperator_NegativeFractionWithNegativeFractionWithVariable_IsCorrect()
        {
            func.Function = "a >= b";
			func.AddSetVariable("a", -0.5d);
			func.AddSetVariable("b", -0.5d);
            Assert.AreEqual(true, func.EvaluateBoolean());
        }

        [Test]
        [ExpectedException(typeof(ExpressionException), ExpectedMessage = "Operator error", MatchType = MessageMatch.Contains)]
        public void GreaterEqualOperator_MalformedExpressionPositiveWholeLeftOfOperator_ThrowsException()
        {
            func.Function = "2 >=";
        }

        [Test]
        [ExpectedException(typeof(ExpressionException), ExpectedMessage = "Operator error", MatchType = MessageMatch.Contains)]
        public void GreaterEqualOperator_MalformedExpressionPositiveFractionLeftOfOperator_ThrowsException()
        {
            func.Function = "0.5 >=";
        }

        [Test]
        [ExpectedException(typeof(ExpressionException), ExpectedMessage = "Operator error", MatchType = MessageMatch.Contains)]
        public void GreaterEqualOperator_MalformedExpressionNegativeWholeLeftOfOperator_ThrowsException()
        {
            func.Function = "-2 >=";
        }

        [Test]
        [ExpectedException(typeof(ExpressionException), ExpectedMessage = "Operator error", MatchType = MessageMatch.Contains)]
        public void GreaterEqualOperator_MalformedExpressionNegativeFractionLeftOfOperator_ThrowsException()
        {
            func.Function = "-0.5 >=";
        }

        [Test]
        [ExpectedException(typeof(ExpressionException), ExpectedMessage = "Operator error", MatchType = MessageMatch.Contains)]
        public void GreaterEqualOperator_MalformedExpressionPositiveWholeRightOfOperator_ThrowsException()
        {
            func.Function = ">= 2";
        }

        [Test]
        [ExpectedException(typeof(ExpressionException), ExpectedMessage = "Operator error", MatchType = MessageMatch.Contains)]
        public void GreaterEqualOperator_MalformedExpressionPositiveFractionRightOfOperator_ThrowsException()
        {
            func.Function = ">= 0.5";
        }

        [Test]
        [ExpectedException(typeof(ExpressionException), ExpectedMessage = "Operator error", MatchType = MessageMatch.Contains)]
        public void GreaterEqualOperator_MalformedExpressionNegativeWholeRightOfOperator_ThrowsException()
        {
            func.Function = ">= -2";
        }

        [Test]
        [ExpectedException(typeof(ExpressionException), ExpectedMessage = "Operator error", MatchType = MessageMatch.Contains)]
        public void GreaterEqualOperator_MalformedExpressionNegativeFractionRightOfOperator_ThrowsException()
        {
            func.Function = ">= -0.5";
        }

    }
}
