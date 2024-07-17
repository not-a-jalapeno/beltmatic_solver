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
            public List<int> ints = new List<int>();

            public num(int n) {
                number = n;
                stepc = 1;
                makeorder = $"{n}";
                ints.Add(n);
            }
            public num(num n1, num n2,char opcode) {
                //a  add 
                //m  mult
                //s  subt
                //e  exp
                //d  div
                //r  div remainder
                //c  conjgate
                if (opcode == 'a') { number = n1.number + n2.number; makeorder = $"({n1.makeorder}+{n2.makeorder})"; }
                else if (opcode == 'm') { number = n1.number * n2.number; makeorder = $"({n1.makeorder}*{n2.makeorder})"; }
                else if (opcode == 's') { number = n1.number - n2.number; makeorder = $"({n1.makeorder}-{n2.makeorder})"; }
                else if (opcode == 'e') { number = (int)Math.Pow(n1.number, n2.number); makeorder = $"({n1.makeorder}^{n2.makeorder})"; }
                else if (opcode == 'd') { number = n1.number / n2.number;makeorder = $"({n1.makeorder}/{n2.makeorder})"; }
                else if (opcode == 'r') { number = n1.number / n2.number;makeorder = $"({n1.makeorder}%{n2.makeorder})"; }
                stepc = n1.stepc+n2.stepc;
                foreach (int i in n1.ints) { ints.Add(i); }
                foreach (int i in n2.ints) { ints.Add(i); }
            }
            public string intz() {
                string so = "";
                List<int> insz = new List<int>();
                foreach (int i in ints) { insz.Add(i); }
                insz.Sort();
                foreach (int i in insz) { so = $"{so} {i}"; }
                return so;
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
            Dictionary<int,int> made = new Dictionary<int,int>();//number | step count

            int get_to = 10;

            int serchmax = (int)(get_to * 1.1);

            serchmax = Math.Max(serchmax,10000);

            List<int> extractable = new List<int>();

            extractable.Add(1);
            extractable.Add(2);
            extractable.Add(3);
            extractable.Add(4);
            extractable.Add(5);
            extractable.Add(6);
            extractable.Add(7);
            extractable.Add(8);
            extractable.Add(9);
            /*
            extractable.Add(11);
            extractable.Add(12);
            extractable.Add(13);
            extractable.Add(14);
            extractable.Add(15);
            extractable.Add(16);
            extractable.Add(17);*/

            foreach (int i in extractable) { q.Add(new num(i));made.Add(i, 1); }

            //q.Add(new num(1)); made.Add(q[0].number,1);
            //q[0].stepc = made[q[0].number];


            int count = 0;
            int c = 0;
            int l = 0;
            string strin = "";
            Console.WriteLine("enter to run");
            while (true)
            {
                strin = Console.ReadLine();
                if (strin == "") { calc_loop(); }
                else{ Console.WriteLine($"change target number max:{serchmax}"); get_to = int.Parse(strin); sol = new List<num>();
                    foreach (num n in q) { if (n.number == get_to) { Console.WriteLine($"n:{n.number}|step:{n.stepc - 1}|{n.intz()}|{n.makeorder}|"); } }
                }
            }


            void calc_loop() {
                count = 0;
                l = Math.Max(q.Count() / 20, 1);
                c = 0;
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.Write("number maker:");
                foreach (num n1 in q)
                {

                    foreach (num n2 in q)
                    {
                        //a  add
                        adder(n1, n2, 'a');
                        //m  mult
                        adder(n1, n2, 'm');
                        //s  subt
                        //adder(n1, n2, 's');
                        //e  exp
                        //adder(n1, n2, 'e');
                        //d  div
                        //adder(n1, n2, 'd');
                        //r  div remainder
                        //adder(n1, n2, 'r');

                    }
                    count++;
                    if (count % l == 0)
                    {
                        c++;
                        Console.Write($"[{c}]");
                    }
                }
                Console.WriteLine();
                //showes the dict
                //foreach (var i in made) {Console.WriteLine($"{i.Key}|{i.Value}");}

                //compile to q
                foreach (num n in qt) { q.Add(n); }
                //removes duplicates
                sol = dedupsol(sol);
                //print the solutions
                prnt(sol, get_to);
                q = dedup(q);
                //tell when finished scycle and wait for me to tell to run agian
                Console.WriteLine("ran:");
            }

            //the function that addes new numbers
            void adder(num n1,num n2,char opc) {
                int a = 0;
                if (opc == 'a') {a = n1.number + n2.number; }
                else if (opc == 'm') {a = n1.number * n2.number; }
                else if (opc == 's') {a = n1.number - n2.number; }
                else if (opc == 'e') {a = (int)Math.Pow(n1.number, n2.number); }
                else if (opc == 'd') { a = n1.number / n2.number; }
                else if (opc == 'r') { a = n1.number % n2.number; }                
                if (a > 0 && a < serchmax)
                {
                    if (made.ContainsKey(get_to)) { if (a == get_to && (made[get_to] >= n1.stepc + n2.stepc)) { sol.Add(new num(n1, n2, opc)); } }
                    if (!made.ContainsKey(a)) {
                        qt.Add(new num(n1, n2, opc)); made.Add(a, n1.stepc + n2.stepc);
                    }
                    else if (made[a] > n1.stepc + n2.stepc) {
                        qt.Add(new num(n1, n2, opc)); made[a] = n1.stepc + n2.stepc;
                    }
                }
            }
            //

            //removes duplicate numbers from the list of elelments replace longer step counts 
            List<num> dedup(List<num> l) {
                Console.Write("\ndeduplication:");
                List<num> list = new List<num>();
                bool b = true;
                int i = 0;
                int c = 0;
                Console.WriteLine(l.Count());
                int d = Math.Max(l.Count()/20,1);
                int co = 0;
                foreach (num n in l)
                {
                    b = true;
                    i = 0;
                    c++;
                    if (c % d == 0) {
                        co++;
                        Console.Write($"[{co}]");
                    }
                    foreach (num n2 in list){
                        if (n2.number == n.number){
                            if (n2.stepc > n.stepc){
                                list[i] = n;
                            }
                            b = false; break;
                        }
                        i++;
                    }
                    if (b) { list.Add(n); }
                }
                Console.WriteLine();
                if (list.Count() == serchmax-1) { Console.WriteLine("### | all of serch space | ###"); }
                Console.WriteLine($"after reduction:{list.Count()+1} ot off:{serchmax}");
                //Console.WriteLine($"dict size:{made.Count()}");
                return list;
            }
            //


            //a smallwer version of the dedup for the sol list 
            List<num> dedupsol(List<num> l)
            {
                List<num> list = new List<num>();
                bool b = true;
                foreach (num n in l)
                {
                    b = true;
                    foreach (num n2 in list)
                    {
                        //Console.WriteLine($"n{n.number}|{n.stepc}|n2:{n2.number}|{n2.stepc}");
                        if (n2.makeorder == n.makeorder)
                        {
                            b = false; break;
                        }
                    }
                    if (b) { list.Add(n); }
                }
                return list;
            }
            //
            //sorts by the sum of the extracted numbers
            List<num> sortsum(List<num> l)
            {
                Dictionary<int, List<num>> s = new Dictionary<int, List<num>>();
                List<int> keys = new List<int>();
                foreach (num n in l)
                {
                    if (!s.ContainsKey(n.ints.Sum())) { s.Add(n.ints.Sum(), new List<num> { n }); keys.Add(n.ints.Sum()); }
                    else { s[n.ints.Sum()].Add(n); }
                }
                keys.Sort();
                List<num> listout = new List<num>();
                foreach (int i in keys)
                {
                    foreach (num n in s[i])
                    {
                        listout.Add(n);
                    }
                }
                return listout;
            }

            //prints all elements that are of the lowest step count
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
                    foreach (num n in sortsum(s[i])) {
                        Console.WriteLine($"n:{n.number}|step:{n.stepc - 1}|{n.intz()}|{n.makeorder}|");
                    }
                    break;
                }

            }
            //

        }
    }
}