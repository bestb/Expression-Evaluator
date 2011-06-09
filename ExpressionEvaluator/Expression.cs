/*****************************************************************************************
* Jeremy Roberts                                                           Expression.cs *
* 2004-07-23                                                                     Wfccm2 *
*****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Vanderbilt.Biostatistics.Wfccm2
{
    public interface IVariable
    {
        string Name { get; }
        Type Type { get; }
    }

    /// <summary>
    /// Evaluatable mathmatical function.
    /// </summary>
    /// <remarks><pre>
    /// 2004-07-19 - Jeremy Roberts
    /// Takes a string and allow a user to add variables. Evaluates the string as a mathmatical function.
    /// 2006-01-11 - Will Gray
    ///  - Added generic collection implementation.
    /// </pre></remarks>
    [Serializable()]
    public class Expression : MarshalByRefObject
    {
        /// <summary>
        /// Dynaamic function type.
        /// </summary>
        /// <remarks><pre>
        /// 2005-12-20 - Jeremy Roberts
        /// </pre></remarks>
        [Serializable()]
        public abstract class DynamicFunction
        {
            public abstract double EvaluateD(Dictionary<string, double> variables);
            public abstract bool EvaluateB(Dictionary<string, double> variables);
        }
        protected DynamicFunction _dynamicFunction;
        //protected AppDomain NewAppDomain;


        #region Member data
        
        /// <summary>
        /// The function.
        /// </summary>
        protected InfixExpression _inFunction;
        protected PostFixExpression _postFunction;
        protected Dictionary<string, IVariable> _variables = new Dictionary<string, IVariable>();
        protected const double TRUE = 1;
        protected const double FALSE = 0;
        protected string[] _splitPostFunction;
        protected List<Token> _tokens = new List<Token>();

        #endregion

        #region Constructors
        /// <summary>
        /// Creation constructor.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-20 - Jeremy Roberts
        /// </pre></remarks>
        public Expression()
        {
        }

        /// <summary>
        /// Creation constructor.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-19 - Jeremy Roberts
        /// </pre></remarks>
        /// <param name="function">The function to be evaluated.</param>
        public Expression(string function)
        {
            Function = function;
        }

        #endregion

        #region properties
        /// <summary>
        /// InFix property
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-19 - Jeremy Roberts
        /// </pre></remarks>
        public string Function
        {
            get{return _inFunction.Original;}
            set{
                _inFunction = new InfixExpression(value);
                _postFunction = new PostFixExpression(_inFunction);
                ClearVariables();
                BuildTokens();
            }
        }

        /// <summary>
        /// PostFix property
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-19 - Jeremy Roberts
        /// </pre></remarks>
        public string PostFix
        {
            get { return _postFunction.Expression; }
        }

        /// <summary>
        /// PostFix property
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-19 - Jeremy Roberts
        /// </pre></remarks>
        public string InFix
        {
            get { return _inFunction.Expression; }
        }

        #endregion

        public void AddSetVariable(string name, double val)
        {
            AddSetVariable<double>(name, val);
        }

        public void AddSetVariable(string name, DateTime val)
        {
            AddSetVariable<DateTime>(name, val);
        }

        public void AddSetVariable(string name, bool val)
        {
            AddSetVariable<double>(name, val ? TRUE : FALSE);
        }



        private void AddSetVariable<T>(string name, T val)
        {
            if (!_variables.ContainsKey(name))
            {
                var v = new Variable<T>(name);
                _variables[name] = v;
            }

            ((Variable<T>)_variables[name]).Value = val;
        }

        /// <summary>
        /// Clears the variable information.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-19 - Jeremy Roberts
        /// </pre></remarks>
        public void ClearVariables()
        {
            _variables.Clear();
        }

        /// <summary>
        /// Clears all information.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-19 - Jeremy Roberts
        /// </pre></remarks>
        public void Clear()
        {
            _inFunction = null;
            _postFunction = null;
            _variables.Clear();
        }

        private void BuildTokens()
        {
            _tokens = new List<Token>();

            foreach (var t in _postFunction.Tokens)
            {
                if (IsVariable(t))
                {
                    Variable<double> v;
                    if (!_variables.ContainsKey(t))
                    {
                        v = new Variable<double>(t, double.NaN);
                        _variables.Add(t, v);
                    }
                    else
                    {
                        v = (Variable<double>)_variables[t];
                    }
                    _tokens.Add(v);
                    continue;
                }

                if (ExpressionKeywords.Keywords.Where(x => x.Name == t).Count() == 1)
                {
                    var v = ExpressionKeywords.Keywords.Where(x => x.Name == t).Single();
                    _tokens.Add(v);
                }

                if (IsNumber(t))
                {
                    var v = (Operand<double>)ConvertToOperand(t);
                    _tokens.Add(v);
                }

                if (t == "true" || t == "false")
                {
                    var v = (Operand<double>)ConvertToOperand(t);
                    _tokens.Add(v);
                }
            }
        }

        /// <summary>
        /// Checks to see if a string is a variable.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-20 - Jeremy Roberts
        /// 2004-08-11 - Jeremy Roberts
        ///  Changed to check to see if it is a number.
        /// </pre></remarks>
        /// <param name="token">String to check.</param>
        /// <returns></returns>
        protected bool IsVariable(string token)
        {
            if (token == "true" || token == "false")
                return false;

            if (!ExpressionKeywords.IsOperand(token))
                return false;

            if (IsNumber(token))
                return false;

            return true;
        }

        public ReadOnlyCollection<string> FunctionVariables
        {
            get
            {
                if (_inFunction == null)
                    throw new ExpressionException("Function does not exist");

                // The arraylist to return
                List<string> retVal = new List<string>();

                // Check each token to see if its a variable.
                foreach (string token in _inFunction.Tokens)
                {
                    if (IsVariable(token))
                        retVal.Add(token);
                }

                return retVal.AsReadOnly();
            }
        }

        /// <summary>
        /// Returns a variable's value.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-20 - Jeremy Roberts
        /// </pre></remarks>
        /// <param name="token">Variable to return.</param>
        /// <returns></returns>
        public double GetVariableValue(string token)
        {
            try
            {
                return ((Operand<double>)_variables[token]).Value;
            }
            catch
            {
                return double.NaN;                
            }
        }

        /// <summary>
        /// Evaluates the function as a double.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-19 - Jeremy Roberts
        /// </pre></remarks>
        /// <returns></returns>
        public double EvaluateNumeric()
        {
            // TODO! Check to see that we have the variable that we need.

            // Create a temporary vector to hold the secondary stack.
            //return ConvertToOperand(Evaluate());
            return ((Operand<double>)ConvertToOperand(Evaluate())).Value;
        }

        /// <summary>
        /// Evaluates the function given as a boolean expression.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-20 - Jeremy Roberts
        /// </pre></remarks>
        public bool EvaluateBoolean()
        {
            string result = Evaluate();

            if (((Operand<double>)ConvertToOperand(result)).Value == TRUE)
                return true;
            return false;
        }

        private string Evaluate()
        {
            var workstack = new Stack<Token>();
            //string sLeft = "";
            //string sRight = "";
            Operand<double> op1 = null;
            Operand<double> op2 = null;
            //string sResult = "";
            double result = double.NaN;
            int currentConditionalDepth = 0;

            // loop through the postfix vector
            foreach (var token in _tokens)
            {
                // If the current string is an operator
                //if (!ExpressionKeywords.IsOperator(token))
                if (!(token is Keyword))
                {
                    // push the string on the workstack
                    workstack.Push(token);
                    continue;
                }

                var op = token as Keyword; 

                // Single operand operators. 
                if (op.Name == "abs" ||
                    op.Name == "neg" ||
                    op.Name == "ln" ||
                    op.Name == "sign" ||
                    op.Name == "else")
                {
                    //sLeft = workstack.Pop();
                    op1 = (Operand<double>)workstack.Pop();

                    // Convert the operands
                    //op1 = (Operand<double>) ConvertToOperand(sLeft);
                }
                    // Double operand operators
                else
                {
                    //sRight = workstack.Pop();
                    //sLeft = workstack.Pop();
                    op2 = (Operand<double>)workstack.Pop();
                    op1 = (Operand<double>)workstack.Pop();

                    // Convert the operands
                    //op1 = (Operand<double>) ConvertToOperand(sLeft);
                    //op2 = (Operand<double>) ConvertToOperand(sRight);
                }

                // call the operator 
                switch (op.Name)
                {
                    case "+":
                        //sResult = (op1.Value + op2.Value).ToString();
                        result = op1.Value + op2.Value;
                        break;

                    case "-":
                        //sResult = (op1.Value - op2.Value).ToString();
                        result = op1.Value - op2.Value;
                        break;

                    case "*":
                        //sResult = (op1.Value*op2.Value).ToString();
                        result = op1.Value * op2.Value;
                        break;

                    case "/":
                        //sResult = op2.Value == 0 ? (double.NaN).ToString() : (op1.Value/op2.Value).ToString();
                        result = op2.Value == 0 ? double.NaN : op1.Value / op2.Value;
                        break;

                    case "^":
                        //sResult = Math.Pow(op1.Value, op2.Value).ToString();
                        result = Math.Pow(op1.Value, op2.Value);
                        break;

                    case "sign":
                        //sResult = (op1.Value >= 0 ? 1 : -1).ToString();
                        result = op1.Value >= 0 ? 1 : -1;
                        break;

                    case "abs":
                        //sResult = Math.Abs(op1.Value).ToString();
                        result = Math.Abs(op1.Value);
                        break;

                    case "neg":
                        //sResult = (-1*op1.Value).ToString();
                        result = -1 * op1.Value;
                        break;

                    case "ln":
                        //sResult = Math.Log(op1.Value).ToString();
                        result = Math.Log(op1.Value);
                        break;

                    case "<=":
                        //sResult = op1.Value <= op2.Value ? "true" : "false";
                        result = op1.Value <= op2.Value ? TRUE : FALSE;
                        break;

                    case "<":
                        //sResult = op1.Value < op2.Value ? "true" : "false";
                        result = op1.Value < op2.Value ? TRUE : FALSE;
                        break;

                    case ">=":
                        //sResult = op1.Value >= op2.Value ? "true" : "false";
                        result = op1.Value >= op2.Value ? TRUE : FALSE;
                        break;

                    case ">":
                        //sResult = op1.Value > op2.Value ? "true" : "false";
                        result = op1.Value > op2.Value ? TRUE : FALSE;
                        break;

                    case "==":
                        //sResult = op1.Value == op2.Value ? "true" : "false";
                        result = op1.Value == op2.Value ? TRUE : FALSE;
                        break;

                    case "!=":
                        //sResult = op1.Value != op2.Value ? "true" : "false";
                        result = op1.Value != op2.Value ? TRUE : FALSE;
                        break;

                    case "||":
                        //sResult = op2.Value == TRUE || op1.Value == TRUE ? "true" : "false";
                        result = op2.Value == TRUE || op1.Value == TRUE ? TRUE : FALSE;
                        break;

                    case "&&":
                        //sResult = op2.Value == TRUE && op1.Value == TRUE ? "true" : "false";
                        result = op2.Value == TRUE && op1.Value == TRUE ? TRUE : FALSE;
                        break;

                    case "elseif":
                        if (currentConditionalDepth > 0)
                        {
                            // Eat the result.
                            continue;
                        }
                        goto case "if";

                    case "if":
                        if (op1.Value != TRUE && op1.Value != FALSE)
                        {
                            throw new ExpressionException("variable type error.");
                        }
                        if (op1.Value == TRUE)
                        {
                            result = op2.Value;
                            currentConditionalDepth++;
                        }
                        else
                        {
                            // Eat the result.
                            continue;
                        }
                        break;

                    case "else":
                        if (currentConditionalDepth > 0)
                        {
                            currentConditionalDepth--;
                            // Eat the result.
                            continue;
                        }
                        result = op1.Value;
                        break;
                }

                // Push the result on the stack
                workstack.Push(new Operand<double>(result));
            }

            return ((Operand<double>)workstack.Peek()).Value.ToString();
        }

        /// <summary>
        /// Converts a string to its value representation.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-20 - Jeremy Roberts
        /// </pre></remarks>
        /// <param name="token">The string to check.</param>
        /// <returns></returns>
        protected object ConvertToOperand(string token)
        {
            try
            {
                return _variables[token];
            }
            catch
            {
                if (token == "true")
                    return new Operand<double>(TRUE);
                
                if (token == "false")
                    return new Operand<double>(FALSE);
                
                // Convert the operand
                return new Operand<double>(double.Parse(token));
            }
        }

        /// <summary>
        /// Overloads the ToString method.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-20 - Jeremy Roberts
        /// </pre></remarks>
        /// <param name="token">The string to check.</param>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(_inFunction.Original);
            int count = 0;
            foreach (var keyval in _variables)
            {
                if (count++ == 0)
                    ret.Append("; ");
                else
                    ret.Append(", ");
                ret.Append(keyval.Value);
            }
            return ret.ToString();
        }

        /// <summary>
        /// Checks to see if a string is a number.
        /// </summary>
        /// <remarks><pre>
        /// 2004-08-11 - Jeremy Roberts
        /// </pre></remarks>
        /// <param name="token">The string to check.</param>
        /// <returns>True if is a number, false otherwise.</returns>
        protected bool IsNumber(string token)
        {
            try 
            {
                double.Parse(token);
                return true;
            }
            catch 
            {
                return false;
            }

        }

        #region compilation

        ///// <summary>
        ///// Compiles the functions.
        ///// </summary>
        ///// <remarks><pre>
        ///// 2005-12-20 - Jeremy Roberts
        ///// </pre></remarks>
        //protected void compile() 
        //{
        //    // Code to set up the object.

        //    // Create a new AppDomain.
        //    // Set up assembly.
        //    //
        //    //NewAppDomain = System.AppDomain.CreateDomain("NewApplicationDomain");
        //    //NewAppDomain = appDomain;

        //    AssemblyName assemblyName = new AssemblyName();
        //    assemblyName.Name = "EmittedAssembly";
        //    AssemblyBuilder assembly = Thread.GetDomain().DefineDynamicAssembly(
        //    //AssemblyBuilder assembly = NewAppDomain.DefineDynamicAssembly(
        //        assemblyName,
        //        //AssemblyBuilderAccess.Save);
        //        AssemblyBuilderAccess.Run);
        //        //AssemblyBuilderAccess.RunAndSave);

        //    // Add Dynamic Module
        //    //
        //    ModuleBuilder module;
        //    module = assembly.DefineDynamicModule("EmittedModule");
        //    TypeBuilder dynamicFunctionClass = module.DefineType(
        //        "DynamicFunction",
        //        TypeAttributes.Public,
        //        typeof(DynamicFunction));

        //    // Define class constructor
        //    //
        //    Type objType = Type.GetType("System.Object");
        //    ConstructorInfo objConstructor = objType.GetConstructor(new Type[0]);
        //    Type[] constructorParams = { };
        //    ConstructorBuilder constructor = dynamicFunctionClass.DefineConstructor(
        //        MethodAttributes.Public,
        //        CallingConventions.Standard,
        //        constructorParams);

        //    // Emit the class constructor.
        //    //
        //    ILGenerator constructorIL = constructor.GetILGenerator();
        //    constructorIL.Emit(OpCodes.Ldarg_0);
        //    constructorIL.Emit(OpCodes.Call, objConstructor);
        //    constructorIL.Emit(OpCodes.Ret);

        //    // Define "EvaluateD" function.
        //    //
        //    Type[] args = { typeof(Dictionary<string, double>) };
        //    MethodBuilder evalMethodD = dynamicFunctionClass.DefineMethod(
        //        "EvaluateD",
        //        MethodAttributes.Public | MethodAttributes.Virtual,
        //        typeof(double),
        //        args);
        //    ILGenerator methodILD;
        //    methodILD = evalMethodD.GetILGenerator();
        //    emitFunction(this.PostFix, methodILD);

        //    // Define "EvaluateB" function.
        //    //
        //    MethodBuilder evalMethodB = dynamicFunctionClass.DefineMethod(
        //        "EvaluateB",
        //        MethodAttributes.Public | MethodAttributes.Virtual,
        //        typeof(bool),
        //        args);
        //    ILGenerator methodILB;
        //    methodILB = evalMethodB.GetILGenerator();
        //    emitFunction(this.PostFix, methodILB);

        //    // Create an object to use.
        //    //
        //    Type dt = dynamicFunctionClass.CreateType();
        //    //assembly.Save("assem.dll");
        //    //assembly.Save("x.exe");
        //    //return (function)Activator.CreateInstance(dt, new Object[] { });
        //    this.dynamicFunction = (DynamicFunction)Activator.CreateInstance(dt, new Object[] { });
        //}

        
        //protected void emitFunction(string function, ILGenerator ilGen) 
        //{
        //    string[] splitFunction = function.Split(new char[] { ' ' });

        //    // Set up two double variables.
        //    ilGen.DeclareLocal(typeof(System.Double));
        //    ilGen.DeclareLocal(typeof(System.Double));


        //    foreach (string token in splitFunction)
        //    {
        //        // If the current string is an operator
        //        if (this.IsOperator(token))
        //        {
        //            // call the operator 
        //            switch (token)
        //            {
        //            case "+":
        //                {
        //                    // Add the operands
        //                    ilGen.Emit(OpCodes.Add);
        //                    break;
        //                }

        //            case "-":
        //                {
        //                    // Subtract the operands
        //                    ilGen.Emit(OpCodes.Sub);
        //                    break;
        //                }

        //            case "*":
        //                {
        //                    // Multiply the operands
        //                    ilGen.Emit(OpCodes.Mul);
        //                    break;
        //                }

        //            case "/":
        //                {
        //                    // Divide the operands
        //                    System.Reflection.Emit.Label pushNaN = ilGen.DefineLabel();
        //                    System.Reflection.Emit.Label exit = ilGen.DefineLabel();

        //                    // Store the two variables.
        //                    ilGen.Emit(OpCodes.Stloc_0); // store b in 0
        //                    ilGen.Emit(OpCodes.Stloc_1); // store a in 1

        //                    // Load the denominator and see if its 0.
        //                    ilGen.Emit(OpCodes.Ldloc_0);
        //                    ilGen.Emit(OpCodes.Ldc_R8, 0.0);
        //                    ilGen.Emit(OpCodes.Ceq);
        //                    ilGen.Emit(OpCodes.Brtrue_S, pushNaN);

        //                    // It is not zero, do the division.
        //                    ilGen.Emit(OpCodes.Ldloc_1);
        //                    ilGen.Emit(OpCodes.Ldloc_0);
        //                    ilGen.Emit(OpCodes.Div);
        //                    ilGen.Emit(OpCodes.Br_S, exit);

        //                    // Push NaN
        //                    ilGen.MarkLabel(pushNaN);
        //                    ilGen.Emit(OpCodes.Ldc_R8, double.NaN);

        //                    ilGen.MarkLabel(exit);

        //                    break;
        //                }

        //            case "^":
        //                {
        //                    // Raise the number to the power.
        //                    ilGen.EmitCall(OpCodes.Callvirt,
        //                        typeof(System.Math).GetMethod("Pow"),
        //                        null);
        //                    break;
        //                }

        //            case "sign":
        //                {
        //                    // Get the sign.
        //                    System.Reflection.Emit.Label pushNeg = ilGen.DefineLabel();
        //                    System.Reflection.Emit.Label exit = ilGen.DefineLabel();

        //                    // Compare to see if the value is less then 0
        //                    ilGen.Emit(OpCodes.Stloc_0); // store
        //                    ilGen.Emit(OpCodes.Ldloc_0);
        //                    ilGen.Emit(OpCodes.Ldc_R8, 0.0);
        //                    ilGen.Emit(OpCodes.Clt);
        //                    ilGen.Emit(OpCodes.Brtrue_S, pushNeg);

        //                    // Push 1
        //                    ilGen.Emit(OpCodes.Ldc_R8, 1.0);
        //                    ilGen.Emit(OpCodes.Br_S, exit);

        //                    // Push Neg
        //                    ilGen.MarkLabel(pushNeg);
        //                    ilGen.Emit(OpCodes.Ldc_R8, -1.0);

        //                    ilGen.MarkLabel(exit);

        //                    break;
        //                }

        //            case "abs":
        //                {
        //                    // Convert to postive.
        //                    Type[] absArgs = { typeof(System.Double) };
        //                    ilGen.EmitCall(OpCodes.Callvirt,
        //                        typeof(System.Math).GetMethod("Abs", absArgs),
        //                        null);
        //                    break;
        //                }

        //            case "neg":
        //                {
        //                    // Change the sign.
        //                    ilGen.Emit(OpCodes.Ldc_R8, -1.0);
        //                    ilGen.Emit(OpCodes.Mul);
        //                    break;
        //                }

        //            case "<=":
        //                {
        //                    // Make the comparison.
        //                    System.Reflection.Emit.Label pushFalse = ilGen.DefineLabel();
        //                    System.Reflection.Emit.Label exit = ilGen.DefineLabel();

        //                    // Compare the two values.
        //                    ilGen.Emit(OpCodes.Cgt);
        //                    ilGen.Emit(OpCodes.Brtrue_S, pushFalse);

        //                    // Otherwise its true
        //                    ilGen.Emit(OpCodes.Ldc_R8, TRUE);
        //                    ilGen.Emit(OpCodes.Br_S, exit);

        //                    // Push NaN
        //                    ilGen.MarkLabel(pushFalse);
        //                    ilGen.Emit(OpCodes.Ldc_R8, FALSE);

        //                    ilGen.MarkLabel(exit);
        //                    break;
        //                }

        //            case "<":
        //                {
        //                    // Make the comparison.
        //                    // Compare the two values.
        //                    ilGen.Emit(OpCodes.Clt);
        //                    break;
        //                }

        //            case ">=":
        //                {
        //                    // Make the comparison.
        //                    System.Reflection.Emit.Label pushFalse = ilGen.DefineLabel();
        //                    System.Reflection.Emit.Label exit = ilGen.DefineLabel();

        //                    // Compare the two values.
        //                    ilGen.Emit(OpCodes.Clt);
        //                    ilGen.Emit(OpCodes.Brtrue_S, pushFalse);

        //                    // Otherwise its true
        //                    ilGen.Emit(OpCodes.Ldc_R8, TRUE);
        //                    ilGen.Emit(OpCodes.Br_S, exit);

        //                    // Push NaN
        //                    ilGen.MarkLabel(pushFalse);
        //                    ilGen.Emit(OpCodes.Ldc_R8, FALSE);

        //                    ilGen.MarkLabel(exit);
        //                    break;
        //                }

        //            case ">":
        //                {
        //                    // Make the comparison.
        //                    ilGen.Emit(OpCodes.Cgt);
        //                    break;
        //                }

        //            case "==":
        //                {
        //                    // Make the comparison.
        //                    ilGen.Emit(OpCodes.Ceq);
        //                    break;
        //                }

        //            case "!=":
        //                {
        //                    // Make the comparison.
        //                    System.Reflection.Emit.Label pushFalse = ilGen.DefineLabel();
        //                    System.Reflection.Emit.Label exit = ilGen.DefineLabel();

        //                    // Compare the two values.
        //                    ilGen.Emit(OpCodes.Ceq);
        //                    ilGen.Emit(OpCodes.Brtrue_S, pushFalse);

        //                    // Otherwise its true
        //                    ilGen.Emit(OpCodes.Ldc_R8, TRUE);
        //                    ilGen.Emit(OpCodes.Br_S, exit);

        //                    // Push NaN
        //                    ilGen.MarkLabel(pushFalse);
        //                    ilGen.Emit(OpCodes.Ldc_R8, FALSE);

        //                    ilGen.MarkLabel(exit);

        //                    break;
        //                }

        //            case "||":
        //                {
        //                    // Make the comparison.
        //                    System.Reflection.Emit.Label pushTrue = ilGen.DefineLabel();
        //                    System.Reflection.Emit.Label exit = ilGen.DefineLabel();

        //                    // Store the two variables.
        //                    ilGen.Emit(OpCodes.Stloc_0);
        //                    ilGen.Emit(OpCodes.Stloc_1);

        //                    // Compare the two values.
        //                    ilGen.Emit(OpCodes.Ldloc_0);
        //                    ilGen.Emit(OpCodes.Brtrue_S, pushTrue);
        //                    ilGen.Emit(OpCodes.Ldloc_1);
        //                    ilGen.Emit(OpCodes.Brtrue_S, pushTrue);

        //                    // Otherwise its false
        //                    ilGen.Emit(OpCodes.Ldc_R8, FALSE);
        //                    ilGen.Emit(OpCodes.Br_S, exit);

        //                    // Push NaN
        //                    ilGen.MarkLabel(pushTrue);
        //                    ilGen.Emit(OpCodes.Ldc_R8, TRUE);

        //                    ilGen.MarkLabel(exit);
        //                    break;
        //                }

        //            case "&&":
        //                {
        //                    // Make the comparison.
        //                    System.Reflection.Emit.Label pushFalse = ilGen.DefineLabel();
        //                    System.Reflection.Emit.Label exit = ilGen.DefineLabel();

        //                    // Store the two variables.
        //                    ilGen.Emit(OpCodes.Stloc_0);
        //                    ilGen.Emit(OpCodes.Stloc_1);

        //                    // Compare the two values.
        //                    ilGen.Emit(OpCodes.Ldloc_0);
        //                    ilGen.Emit(OpCodes.Brfalse_S, pushFalse);
        //                    ilGen.Emit(OpCodes.Ldloc_1);
        //                    ilGen.Emit(OpCodes.Brfalse_S, pushFalse);

        //                    // Otherwise its true
        //                    ilGen.Emit(OpCodes.Ldc_R8, TRUE);
        //                    ilGen.Emit(OpCodes.Br_S, exit);

        //                    // Push NaN
        //                    ilGen.MarkLabel(pushFalse);
        //                    ilGen.Emit(OpCodes.Ldc_R8, FALSE);

        //                    ilGen.MarkLabel(exit);
        //                    break;
        //                }
        //            }

        //        }
        //        else if (IsVariable(token))
        //        {
        //            // push the string on the workstack
        //            ilGen.Emit(OpCodes.Ldarg_1);
        //            ilGen.Emit(OpCodes.Ldstr, token);
        //            ilGen.EmitCall(OpCodes.Callvirt,
        //                typeof(System.Collections.Generic.Dictionary<string, double>).GetMethod("get_Item"),
        //                null);
        //            //ilGen.Emit(OpCodes.Unbox_Any, typeof(System.Double));
        //        }
        //        else if (token.Equals("true"))
        //        {
        //            ilGen.Emit(OpCodes.Ldc_R8, TRUE);
        //        }
        //        else if (token.Equals("false"))
        //        {
        //            ilGen.Emit(OpCodes.Ldc_R8, FALSE);
        //        }
        //        else
        //        {
        //            // Parse the number.
        //            ilGen.Emit(OpCodes.Ldc_R8, double.Parse(token));
        //        }
        //    }
        //    ilGen.Emit(OpCodes.Ret);
        //}

        #endregion

    }
}
