using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace number_factory // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        class num {
            public int number;
            public int stepc;
            public string makeorder;


            public num(int n) {
                number = n;
                stepc = 1;
                makeorder = $"{n}";
            }
            public num(num n1, num n2,char opcode) {
                //a  add 
                //m  mult
                //s  subt
                //e  exp
                if (opcode == 'a') { number = n1.number + n2.number;makeorder = $"({n1.makeorder}+{n2.makeorder})";}
                else if (opcode == 'm') { number = n1.number * n2.number;makeorder = $"({n1.makeorder}*{n2.makeorder})"; }
                else if (opcode == 's') { number = n1.number - n2.number;makeorder = $"({n1.makeorder}-{n2.makeorder})"; }
                else if (opcode == 'e') { number = (int)Math.Pow(n1.number,n2.number);makeorder = $"({n1.makeorder}^{n2.makeorder})"; }
                stepc = n1.stepc+n2.stepc;
            }
        }
        static void Main(string[] args)
        {
            //um imp
            //d  div
            //r  div remainder

            List<num> q = new List<num>();
            List<num> qt = new List<num>();
            List<num> sol = new List<num>();
            Dictionary<int,int> made = new Dictionary<int,int>();

            int get_to = 9111;

            List<int> numss = new List<int>();
            numss.Add(1);
            numss.Add(2);
            numss.Add(3);
            numss.Add(4);
            numss.Add(5);
            numss.Add(6);
            numss.Add(7);
            numss.Add(8);
            numss.Add(9);
            //numss.Add(11);
            //numss.Add(12);
            

            foreach (int i in numss){
                q.Add(new num(i));
                made.Add(i,1);
            }


            while (true) {
                foreach (num n1 in q) {
                    foreach (num n2 in q)
                    {
                        //a  add
                        adder(n1, n2, 'a');
                        //m  mult
                        adder(n1, n2, 'm');
                        //s  subt
                        adder(n1, n2, 's');
                        //e  exp
                        adder(n1, n2, 'e');
                    }
                }
                //Dict show
                //foreach (var i in made) {Console.WriteLine($"{i.Key}|{i.Value}");}
                //compile to q
                foreach(num n in qt)
                {
                    //if (n.number == get_to) { Console.WriteLine($"n:{n.number}|step:{n.stepc-1}|{n.makeorder}"); }
                    q.Add(n);
                }
                //print the thing
                prnt(sol, get_to);

                q = dedup(q);

                //print all the numbers
                //foreach (num n in q) {Console.WriteLine($"n:{n.number}|s:{n.stepc}|o:{n.makeorder}");}
                Console.WriteLine("ran:");
                Console.ReadLine();
            }

            void adder(num n1,num n2,char opc) {
                int a = 0;
                if (opc == 'a') {a = n1.number + n2.number; }
                if (opc == 'm') {a = n1.number * n2.number; }
                if (opc == 's') {a = n1.number - n2.number; }
                if (opc == 'e') {a = (int)Math.Pow(n1.number, n2.number); }

                num n = new num(n1, n2, opc);
                int st = n1.stepc + n2.stepc;
                if (a == get_to) { sol.Add(n); prnt(sol, get_to); Console.WriteLine(); }
                if (!made.ContainsKey(a)) { qt.Add(n); made.Add(a,st); }
                else if (made[a] > st) { qt.Add(n); made[a] = st; }
            }

            List<num> dedup(List<num> l) {
                Console.WriteLine("dd start");
                List<num> list = new List<num>();
                bool b = true;
                int i = 0;
                int c = 0;
                Console.WriteLine(l.Count());
                int d = l.Count()/20;
                foreach (num n in l)
                {
                    b = true;
                    i = 0;
                    c++;
                    if (c % d == 0) {
                        Console.WriteLine((double)l.Count()/ (double)c);
                    }
                    if (n.number > 0 && n.number < 2147483648 && n.number < get_to*10) {
                    foreach (num n2 in list)
                    {
                        //Console.WriteLine($"n{n.number}|{n.stepc}|n2:{n2.number}|{n2.stepc}");
                        if (n2.number == n.number)
                        {
                            if (n2.stepc > n.stepc)
                            {
                                //Console.WriteLine(i);
                                list[i] = n;
                            }
                            b = false; break;
                        }
                        i++;
                    }
                    if (b) { list.Add(n); }
                }
                }
                Console.WriteLine(list.Count());
                Console.WriteLine("dd end");
                return list;
            }

            void prnt(List<num> l,int tn) {
                Dictionary<int, List<num>> s = new Dictionary<int, List<num>>();
                List<int> keys = new List<int>();
                foreach (num n in l) {
                    if (n.number == tn) {
                        if (!s.ContainsKey(n.stepc)) { s.Add(n.stepc, new List<num> { n });keys.Add(n.stepc); }
                        else { s[n.stepc].Add(n);}
                    }
                }
                keys.Sort();
                foreach (int i in keys) {
                    foreach (num n in s[i]) {
                        Console.WriteLine($"n:{n.number}|step:{n.stepc - 1}|{n.makeorder}");
                    }
                    break;
                }

            }
        }
    }
}