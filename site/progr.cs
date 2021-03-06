﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ConsoleApplication1
{
 

    public class Parser : Component
    {


        private static bool operatorCheek(String s)
        {
            return s == "+" || s == "-" || s == "*" || s == "/" || s == "^";
        }

        private static bool functionCheek(String s)
        {
            return s == "l" || s == "c" || s == "s";
        }

        private static bool numberCheek(String s)
        {
            return (s== "," || s == "0" || s == "1" || s == "2" || s == "3" || s == "4" || s == "5" || s == "6" || s == "7" || s == "8" || s == "9" || s == "." || s == "x" || s == "y");
        }

        private static int priority(String c)
        {
            switch (c)
            {
                case ("+"): return 1;
                case ("-"): return 1;
                case ("*"): return 2;
                case ("/"): return 2;
                case ("^"): return 3;
            }
            return 0;
        }

        private String[] Parse(String s)
        {
            String[] s1 = new String[s.Length];
            Stack<String> st = new Stack<String>();
            int i = 0;

            s = s.Replace("ln", "l");
            s = s.Replace("cos", "c");
            s = s.Replace("sin", "s");
            int t = 0;
            for (int j = 0; j < s.Length; j++)
            {

                String c = s[j] + "";
                if (functionCheek(c))
                    t = 1;
                if (numberCheek(c))
                {
                    s1[i] = c;
                    while (s.Length > j + 1 && numberCheek(s[j + 1] + ""))
                    {
                        j++;
                        s1[i] += s[j];
                    }
                    i++;
                }
                else
                    if (operatorCheek(c))
                    {
                        while (!(st.Count() == 0))
                        {
                            if (priority(st.Peek()) >= priority(c))
                            {
                                s1[i] = st.Peek();
                                st.Pop();
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        st.Push(c);
                    }
                    else
                        if ((c == "(" || functionCheek(c)) && t==1)
                        {
                            st.Push(c);
                        }
                        else 
                            if (c == ")" && t==1)
                            {
                                while (st.Peek() != "(")
                                {
                                    s1[i] = st.Peek();
                                    st.Pop();
                                    i++;
                                }
                                st.Pop();

                                if (functionCheek(st.Peek()))
                                {
                                    s1[i] = st.Peek();
                                    st.Pop();
                                    i++;
                                }
                                t = 0;
                            }
                            else
                                if (c == "(" )
                                {
                                    st.Push(c);
                                }
                                else
                                    if (c == ")")
                                    {
                                        while (st.Peek() != "(")
                                        {
                                            s1[i] = st.Peek();
                                            st.Pop();
                                            i++;
                                        }
                                        st.Pop();
                                    } 
            }
            while (!(st.Count() == 0))
            {
                s1[i] = st.Peek();
                st.Pop();
                i++;
            }

            return s1;
        }

        private String[] S;

        public Parser(String s)
        {
            S = Parse(s);
        }
        public double calc(double x, double y)
        {
            String[] s = S;
            Stack<double> st = new Stack<double>();

            for (int j = 0; j < s.Length && s[j] != null; j++)
            {
                if (s[j] == "+" || s[j] == "-" || s[j] == "*" || s[j] == "/" || s[j] == "^")
                {
                    double result = 0, op2 = st.Pop(), op1 = st.Pop();

                    switch (s[j])
                    {
                        case ("+"): { result = op1 + op2; break; }
                        case ("-"): { result = op1 - op2; break; }
                        case ("*"): { result = op1 * op2; break; }
                        case ("/"): { result = op1 / op2; break; }
                        case ("^"): { result = Math.Pow(op1, op2); break; }
                    }

                    st.Push(result);
                }
                else
                    if (s[j] == "l" || s[j] == "c" || s[j] == "s")
                    {
                        double result = 0, op = st.Pop();

                        switch (s[j])
                        {
                            case ("l"): { result = Math.Log(op); break; }
                            case ("c"): { result = Math.Cos(op); break; }
                            case ("s"): { result = Math.Sin(op); break; }
                        }
                        //if (result == 0) result = 0.001;
                        st.Push(result);
                    }
                    else
                         if (s[j] == "y")
                            st.Push(y);
                    
                    else
                    {
                        if (s[j] == "x")
                            st.Push(x);
                        else
                            st.Push(Double.Parse(s[j]));
                    }
            }

            return st.Pop();
        }
    }


    class Program
    {
        static double[] ravn_rasp(double[] p)
        {
 

            double M = Math.Max(Math.Abs(p[0]), Math.Abs(p[1]));
            double Sn = Math.Sqrt(p[0] * p[0] + p[1] * p[1]);
            p[0] = p[0] * M / Sn;
            p[1] = p[1] * M / Sn;

            return p;
        }

        static void Main(string[] args)
        {
           Parser p = new Parser("((x-0,5)^2 - 1)^4-(y-0,1)*((y-0,1)^3)");
            double answer = p.calc(-0.3361, -0.2010);

            double rad = 2;
            double[] Y = new double[2];
            double[] X = new double[2];
            double[] Xok = new double[2];
            int m = 1000;
            int j = 1;
            X[0] = 2;  X[1] = 2;
            double cal, calx;

            Random rx = new Random();
            double[] ar = new double[2];
            double a, b;
            while (j <= m)
            {   rad=Math.Sqrt(X[0] * X[0] + X[1] * X[1]);
                a = -rad; b = rad;
                ar[0] = rx.NextDouble() * (b - a) + a;
                ar[1] = rx.NextDouble() * (b - a) + a;
                Y = ravn_rasp(ar);
                cal = p.calc(Y[0], Y[1]);
                if (cal < 0)
                {
                    X[0] = Y[0];
                    X[1] = Y[1];
                }
                else if (j < m)
                    j = j + 1;
                else if (j == m)
                {
                    Xok[0] = X[0];
                    Xok[1] = X[1];
                    j = j + 1;
                }
            }
           //Console.WriteLine("x={0}, y={1}, f={2}", Xok[0], Xok[1], cal);

            calx = p.calc(Xok[0], Xok[1]);
            Console.WriteLine("x(1)={0}, x(2)={1}, f(x)={2}", Xok[0], Xok[1], calx);      

            Console.ReadKey();

        }
    }
}
