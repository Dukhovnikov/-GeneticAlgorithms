using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MathParser
{
    class Parser
    {
        public static Delegate variable;
        /// <summary>
        /// Компиляция кода с переданной функцией
        /// </summary>
        public void createDelegat(string function)
        {
            string begin = @"using System;
            namespace MyNamespace
            {
                public delegate double Del(double[] x);
                public static class LambdaCreator 
                {
                    public static Del Create()
                    {
                        return (x)=>";
            string end = @";
                    }
                }
            }";
            function = initialFunction(function);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add("System.dll");
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, begin + function + end);
            try
            {
                var cls = results.CompiledAssembly.GetType("MyNamespace.LambdaCreator");
                var method = cls.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);
                variable = (method.Invoke(null, null) as Delegate);
            }
            catch (Exception) { throw new Exception(function); }
        }
        /// <summary>
        /// Приведение функции к виду C#
        /// </summary>
        public static string initialFunction(string f)
        {
            Regex regec = new Regex(@"(\d+\)?)(\(?x\d)");
            string func = f;
            if (func.Contains("sin")) func = func.Replace("sin", "Math.Sin");
            if (func.Contains("cos")) func = func.Replace("cos", "Math.Cos");
            if (func.Contains("abs")) func = func.Replace("abs", "Math.Abs");
            if (func.Contains("sqrt")) func = func.Replace("sqrt", "Math.Sqrt");
            if (func.Contains("exp")) func = func.Replace("exp", "Math.Exp");
            if (func.Contains("ln")) func = func.Replace("ln", "Math.Log");
            if (func.Contains("e")) func = func.Replace("e", "Math.E");
            func = func.Replace(",", ".");
            while (regec.IsMatch(func)) func = Regex.Replace(func, @"(\d+\)?)(\(?x\d)", @"$1*$2");
            func = Regex.Replace(func, @"(\([^)(]+\))\^(x?\d+)", "Math.Pow($1,$2)");
            func = Regex.Replace(func, @"(x?\d+)\^(x?\d+)", "Math.Pow($1,$2)");
            for (int i = 1; func.Contains("x" + i); i++) func = func.Replace("x" + i, "x[" + (i - 1) + "]");
            return func;
        }
    }
}

#region Синтаксис
//Parser P = new Parser();          //ИНИЦИАЛИЗИРОВАННИЕ ДЕЛЕГАТА ПАРСЕРА
//P.createDelegat(Text);            //СОЗДАНИЕ КОДА
//var a = Parser.a;                 //СОЗДАНИЕ В ДИНАМИЧЕСКОЙ ПАМЯТИ ДЕЛИГАТА С ЛЯМБДА ВЫРАЖЕНИЕМ
//var c = a.DynamicInvoke(x);       //ИСПОЛЬЗОВАНИЕ 
#endregion
#region Анализирование возведения числа в степень
//int b = func.IndexOf("^");
//while (b > 0)
//{
//    int t = b;
//    string s1, s2;//s1^s2;
//    byte openBraket = 0;
//    while ((t >= 0) && (("+*–-/".IndexOf(func[t]) < 0) || (openBraket > 0)) && openBraket < 255)
//    {
//        if (func[t] == ')') openBraket++;
//        else if (func[t] == '(') openBraket--;
//        t--;
//    }
//    if (("(".IndexOf(func[t + 1]) >= 0) && (openBraket == 255)) t++;
//    t++;
//    //if (t != 0) t++;
//    openBraket = 0;
//    while ((b < func.Length) && (("+*-/)".IndexOf(func[b]) < 0) || (openBraket > 0)))
//    {
//        if (func[b] == '(') openBraket++;
//        else if (func[b] == ')') openBraket--;
//        b++;
//    }
//    s1 = func.Substring(t, b - t).Split('^')[0];
//    s2 = func.Substring(t, b - t).Split('^')[1];
//    func = func.Remove(t, b - t);
//    func = func.Insert(t, string.Format("Math.Pow({0},{1})", s1, s2));
//    b = func.IndexOf("^");
//}
#endregion
#region Пытался через регулярные
//func = Regex.Replace(func, @"(\d+)(x\d+)", @"$1*$2");
//func = Regex.Replace(func, @"(\(?\d*x\d+\)?)\^(\d+)", "Math.Pow($1,$2)");
//func = Regex.Replace(func, @"(\(?\d+?\*?x?\d?\)?)\^(\d+|x\d)", "Math.Pow($1,$2)"); //xMath.Pow(1,x2)
//func = Regex.Replace(func, @"(\b\(?[0-9]?[*+-/]?x?\d?\)?\b)\^(\(?[0-9][+-*/]?!x\d\)?)", "Math.Pow($1,$2)");
//func = Regex.Replace(func, @"(\(?\d+|[-+*/]?x\d\)?)\^(\(?\d+|[-+*/]?x\d\)?)", "Math.Pow($1,$2)");
//func = Regex.Replace(func, @"(\(?\d+[-+*/]?|x\d\)?)\^(\(?\d+[-+*/]?|x\d\)?)", "Math.Pow($1,$2)");

//func = Regex.Replace(func, @"(\(x?\d+\^?x?\d+?[+-]x?\d+\^?x?\d+?\))\^(x?\d+)", "Math.Pow($1,$2)");
//func = Regex.Replace(func, @"(\(.+\))\^(x?\d+)", "Math.Pow($1,$2)");
#endregion
