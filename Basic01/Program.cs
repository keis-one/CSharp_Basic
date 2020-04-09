using System;
using Basic01.Front;

namespace Basic01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Basic01!");

            var repl = new Repl();
            repl.Start();
        }
    }
}
