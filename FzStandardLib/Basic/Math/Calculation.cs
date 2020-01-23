/*
该类的思想最于2014年12月思考的到，最初写在易语言程序中。
后再2017年11月移植到C#。
*/


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using M = System.Math;

namespace FzLib.Basic.Math
{
    public class Calculation
    {

        /// <summary>
        /// 分块函数
        /// </summary>
        private string[] function;
        /// <summary>
        /// 要进行函数计算之前的初始化
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <param name="arg">参数</param>
        /// <returns></returns>
        public bool InitializeFunction(string exp, string arg)
        {
            try
            {
                //替换(1+2)(3+4)
                exp = exp.Replace(")(", ")*(");
                for (int i = 0; i < functionList.Length; i++)
                {
                    exp = exp.Replace(functionList[i], $"##{i}#");
                }
                //替换(1/x)sinx
                exp = exp.Replace(")##", ")*##");

                function = exp.Split(new string[] { arg }, StringSplitOptions.None);
                //替换2x、sinx、sin(x)
                char[] beforeArg = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ')' };

                for (int i = 0; i < function.Length - 1; i++)
                {
                    foreach (var c in beforeArg)
                    {
                        if (function[i].EndsWith(c.ToString()))
                        {
                            function[i] += "*";
                        }
                    }
                }
                //替换 xsinx、x(2+3)
                char[] afterArg = { '(', '#' };
                for (int i = 1; i < function.Length; i++)
                {
                    foreach (var c in afterArg)
                    {
                        if (function[i].StartsWith(c.ToString()))
                        {
                            function[i] = "*" + function[i];
                        }
                    }
                }

                //每一个 分块把函数替换回来
                for (int i = 0; i < function.Length; i++)
                {
                    for (int j = 0; j < functionList.Length; j++)
                    {
                        function[i] = function[i].Replace($"##{j}#", functionList[j]);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public CalcResult F(double x)
        {
            string exp = string.Join(x.ToString(), function);
            return Calc(exp);
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <param name="outExp">最后一步的表达式</param>
        /// <returns>若返回double.NaN，则代表计算出错。否则返回结果。</returns>
        public CalcResult Calc(string exp)
        {
            stopCalculate = false;
            steps.Clear();
            expression = exp;
            Standardize();
            Calc();
            return new CalcResult(expression, steps);
        }
        private const string rstrNumber1 = @"(?<num1>-?(\d+\.?(\d+)?)(e\+?-?\d+)?)";
        private const string rstrPlusNumber1 = @"(?<num1>(\d+\.?(\d+)?)(e\+?-?\d+)?)";
        private const string rstrNumber2 = @"(?<num2>-?(\d+\.?(\d+)?)(e\+?-?\d+)?)";
        private const string rstrNumber3 = @"(?<num3>-?(\d+\.?(\d+)?)(e\+?-?\d+)?)";
        private const string rstrNumber = @"(?<num>-?(\d+\.?(\d+)?)(e\+?-?\d+)?)";// @"(?<num>-?(\d+\.?(\d+)?))";
        private const string strFunc1 = "sin|cos|tan|sqrt|lg|ln|abs|asin|acos|atan|sinh|cosh|tanh|floor|round|sign|deg|rad";
        private const string strFunc2 = "max|min|log|round|mod|pow";
        private const string rstrFunc1 = @"(?<symbol>" + strFunc1 + ")";
        private const string rstrFunc2 = @"(?<symbol>" + strFunc2 + ")";
        private const string rstrFunc = @"(?<symbol>" + strFunc1 + "|" + strFunc2 + ")";

        private static readonly string[] functionList = (strFunc1 + "|" + strFunc2).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

        private static readonly Regex rAddAndSub = new Regex(rstrNumber1 + @"(?<sign>\+|-)" + rstrNumber2, RegexOptions.Compiled);
        private static readonly Regex rMultiAndDiv = new Regex(rstrPlusNumber1 + @"(?<sign>\*|/)" + rstrNumber2, RegexOptions.Compiled);
        private static readonly Regex rBrckets = new Regex(@"\((?<exp>[0-9a-zA-Z\.\+\-\*\/°""']+)\)", RegexOptions.Compiled);
        private static readonly Regex rPower = new Regex(rstrNumber1 + @"\^" + rstrNumber2, RegexOptions.Compiled);
        private static readonly Regex rDegree = new Regex($"({rstrNumber1}°)?({rstrNumber2}')?({rstrNumber3}\")?", RegexOptions.Compiled);
        private static readonly Regex rFactorial = new Regex(rstrNumber + @"!", RegexOptions.Compiled);

        private static readonly Regex rFunc = new Regex(rstrFunc + @"\(?" + rstrNumber1 + @"(," + rstrNumber2 + @"\))?", RegexOptions.Compiled);
        private static readonly Regex rFunc1 = new Regex(rstrFunc1 + rstrNumber, RegexOptions.Compiled);
        private static readonly Regex rFunc2 = new Regex(rstrFunc2 + @"\(" + rstrNumber1 + "," + rstrNumber2 + @"\)", RegexOptions.Compiled);

        private string beforeBracket;
        private string afterBracket;
        private string expression;

        private static readonly Dictionary<string, Func<double, double>> strToFunc1 = new Dictionary<string, Func<double, double>>()
        {
            ["sin"] = M.Sin,
            ["cos"] = M.Cos,
            ["tan"] = M.Tan,
            ["lg"] = M.Log10,
            ["sqrt"] = M.Sqrt,
            ["ln"] = M.Log,
            ["abs"] = M.Abs,
            ["asin"] = M.Asin,
            ["acos"] = M.Acos,
            ["atan"] = M.Atan,
            ["sinh"] = M.Sinh,
            ["cosh"] = M.Cosh,
            ["tanh"] = M.Tanh,
            ["floor"] = M.Floor,
            ["round"] = M.Round,
            ["sign"] = p => M.Sign(p),
            ["deg"] = p => p*180/M.PI,
            ["rad"]=p=>p*M.PI/180,
        }; 
        private static readonly Dictionary<string, Func<double, double, double>> strToFunc2 = new Dictionary<string, Func<double, double, double>>()
        {
            ["max"] = (num1, num2) => M.Max(num1, num2),
            ["min"] = (num1, num2) => M.Min(num1, num2),
            ["log"] = (num1, num2) => M.Log(num2, num1),
            ["round"] = (num1, num2) =>
            {
                if ((int)num2 == num2)
                {
                    return M.Round(num1, (int)num2);
                }
                else
                {
                    return double.NaN;
                }
            },
            ["mod"] = (num1, num2) => M.IEEERemainder(num1, num2),
            ["pow"] = (num1, num2) => M.Pow(num1, num2),

        };

        bool stopCalculate = false;
        private List<string> steps;

        public Calculation()
        {
            steps = new List<string>();
        }


        private void AddAndSub()
        {
            while (rAddAndSub.IsMatch(expression) && !stopCalculate)
            {
                Match result = rAddAndSub.Match(expression);
                double num;
                if (result.Groups["sign"].Value == "+")
                {
                    num = double.Parse(result.Groups["num1"].Value) + double.Parse(result.Groups["num2"].Value);
                }
                else
                {
                    num = double.Parse(result.Groups["num1"].Value) - double.Parse(result.Groups["num2"].Value);
                }
                expression = expression.Replace(result.Value, num.ToString());
                steps.Add(beforeBracket + expression + afterBracket);
            }
        }

        private void MultiAndDiv()
        {
            while (rMultiAndDiv.IsMatch(expression) && !stopCalculate)
            {
                Match result = rMultiAndDiv.Match(expression);
                double num;
                if (result.Groups["sign"].Value == "*")
                {
                    num = double.Parse(result.Groups["num1"].Value) * double.Parse(result.Groups["num2"].Value);
                }
                else
                {
                    num = double.Parse(result.Groups["num1"].Value) / double.Parse(result.Groups["num2"].Value);
                }
                expression = expression.Replace(result.Value, num.ToString());
                StandardizePlusAndMinusSigns();
                steps.Add(beforeBracket + expression + afterBracket);
            }
            //AddAndSub(ref exp);
        }

        private string Degree(Match match)
        {
            if(string.IsNullOrEmpty(match.Value))
            {
                return "";
            }
            double value = 0;
            if(match.Groups["num1"].Success)
            {
                value += double.Parse(match.Groups["num1"].Value);
            }
            if(match.Groups["num2"].Success)
            {
                value += double.Parse(match.Groups["num2"].Value)/60;
            }  
            if(match.Groups["num3"].Success)
            {
                value += double.Parse(match.Groups["num3"].Value)/3600;
            }
            return value.ToString();
        }
        private void Power()
        {
            while (rPower.IsMatch(expression))
            {
                Match result = rPower.Match(expression);
                double num;
                try
                {
                    num = M.Pow(double.Parse(result.Groups["num1"].Value), double.Parse(result.Groups["num2"].Value));
                }
                catch
                {
                    stopCalculate = true;
                    return;
                }
                expression = expression.Replace(result.Value, num.ToString());
                steps.Add(beforeBracket + expression + afterBracket);
            }
        }
        private void Factorial()
        {
            while (rFactorial.IsMatch(expression))
            {
                double result = 1;
                var match = rFactorial.Match(expression);
                try
                {
                    double num = double.Parse(rFactorial.Match(expression).Groups["num"].Value);
                    if ((int)num == num && num > 0)
                    {
                        for (int i = 2; i <= (int)num; i++)
                        {
                            result *= i;
                        }
                    }
                    else
                    {
                        throw new Exception("试图求非整数或非正数的阶乘");
                    }
                }
                catch
                {
                    stopCalculate = true;
                    return;
                }
                expression = expression.Replace(match.Value, result.ToString());
                steps.Add(beforeBracket + expression + afterBracket);
            }
        }

        private void Func()
        {
            while (rFunc.IsMatch(expression) && !stopCalculate)
            {
                string temp = expression;
                while (rFunc1.IsMatch(expression))
                {
                    Match result = rFunc1.Match(expression);
                    double num = 0;

                    num = strToFunc1[result.Groups["symbol"].Value](double.Parse(result.Groups["num"].Value));
                    expression = expression.Replace(result.Value, num.ToString());
                    steps.Add(beforeBracket + expression + afterBracket);
                }
                while (rFunc2.IsMatch(expression))
                {
                    Match result = rFunc2.Match(expression);
                    double num = 0;
                    double num1 = double.Parse(result.Groups["num1"].Value);
                    double num2 = double.Parse(result.Groups["num2"].Value);
                    num = strToFunc2[result.Groups["symbol"].Value](num1, num2);
                    expression = expression.Replace(result.Value, num.ToString());
                    steps.Add(beforeBracket + expression + afterBracket);
                }
                if (expression == temp)
                {
                    break;
                }
            }

        }
        /// <summary>
        /// 括号
        /// </summary>
        /// <param name="exp"></param>
        private void Brackets()
        {
            while (rBrckets.IsMatch(expression) && !stopCalculate)
            {
                Match result = rBrckets.Match(expression);
                string num = result.Groups["exp"].Value;
                beforeBracket = expression.Split(new string[] { num }, StringSplitOptions.RemoveEmptyEntries)[0];
                afterBracket = expression.Split(new string[] { num }, StringSplitOptions.RemoveEmptyEntries)[1];
                expression = num;
                Calc();
                //Power(ref num);
                //expression = expression.Replace(result.Value, num.ToString());
                expression = beforeBracket.Substring(0,beforeBracket.Length-1) + expression + afterBracket.Substring(1);
                beforeBracket = "";
                afterBracket = "";
                steps.Add(beforeBracket + expression + afterBracket);
            }
        }
        private void StandardizePlusAndMinusSigns()
        {
            expression=expression.Replace("--", "+").Replace("++", "+").Replace("*+", "*").TrimStart('+');
        }
        private void Standardize()
        {
            string temp = expression;
            expression = expression.ToLower();
            expression = expression.Replace("“", "\"").Replace("”", "\"").Replace("‘", "'").Replace("’", "'").Replace("（","(").Replace("）",")");
            expression = expression.Replace(")(", ")*(");
            expression = expression.Replace(" ", "");
            string temp2;
            do
            {
                temp2 = expression;
                StandardizePlusAndMinusSigns();
            } while (temp2 != expression);

            //替换常数e
            expression = expression.Replace("#e", M.E.ToString());
            //替换常数π
            expression = expression.Replace("#pi", M.PI.ToString());
            //补全括号
            int countOfLeftBrackets = expression.Length - expression.Replace("(", "").Length;
            int countOfRightBrackets = expression.Length - expression.Replace(")", "").Length;
            //补全右侧括号
            if (countOfLeftBrackets > countOfRightBrackets)
            {
                expression += new string(')', countOfLeftBrackets - countOfRightBrackets);
            }
            //补全左侧括号
            else if (countOfLeftBrackets < countOfRightBrackets)
            {
                expression = new string('(', countOfRightBrackets - countOfLeftBrackets) + expression;
            }
            if(rDegree.IsMatch(expression))
            {
                expression= rDegree.Replace(expression, m => Degree(m));
            }


            //替换a(b)和(a)b为a*(b)和(a)*b
            for (int n = 0; n < 10; n++)
            {
                expression = expression.Replace($"{n}(", $"{n}*(");
                expression = expression.Replace($"){n}", $")*{n}");
                //替换类似2sin为2*sin
                //for (char c = 'a'; c <= 'z'; c++)
                //{
                //    exp = exp.Replace($"{n}{c}", $"{n}*{c}");
                //} 
                foreach (var func in functionList)
                {
                    expression = expression.Replace($"{n}{func}", $"{n}*{func}");
                }


                //for (char c = 'a'; c <= 'z'; c++)
                //{
                //    exp = exp.Replace($"{n}{c}", $"{n}*{c}");
                //}
                string str = "##";
                expression = expression.Replace($"{n}{str}", $"{n}*{str}");
            }
            foreach (var func in functionList)
            {
                expression = expression.Replace($"){func}", $")*{func}");
            }
            if (expression != temp)
            {
                steps.Add(expression);
            }
        }

        private void Calc()
        {
            string temp = expression;
            do
            {
                temp = expression;
                do
                {
                    temp = expression;
                    do
                    {
                        temp = expression;
                        do
                        {
                            temp = expression;
                            do
                            {
                                temp = expression;
                                do
                                {
                                    temp = expression;
                                    Brackets();
                                } while (temp != expression && !stopCalculate);

                                Func();
                            } while (temp != expression && !stopCalculate);
                            Factorial();
                        } while (temp != expression && !stopCalculate);
                        Power();
                    } while (temp != expression && !stopCalculate);
                    MultiAndDiv();
                } while (temp != expression && !stopCalculate);
                AddAndSub();
            } while (temp != expression && !stopCalculate);
        }



    }
    public class CalcResult
    {
        internal CalcResult()
        {

        }
        internal CalcResult(string lastExp,IEnumerable< string> steps)
        {
            Steps = steps.ToArray();
            if (double.TryParse(lastExp, out double num))
            {
                Success = true;
                Value = num;
            }
            else
            {
                Success = false;
                LastExpression = lastExp;
            }
        }
        public double Value { get; private set; } = double.NaN;
        public bool Success { get; private set; }
        public string LastExpression { get; private set; }
        public string[] Steps { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            CalcResult result = obj as CalcResult;
            if (result == null)
            {
                return false;
            }
            return Value == result.Value;

        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CalcResult left, CalcResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CalcResult left, CalcResult right)
        {
            return !(left == right);
        }
    }

}
