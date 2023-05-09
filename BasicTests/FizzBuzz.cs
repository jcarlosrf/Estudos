using System;
using System.Collections.Generic;
using System.Text;

namespace BasicTests
{
    public static class FizzBuzz
    {
        public static void fizzBuzz(int n)
        {
            for (int i = 1; i <= n; i++)
            {
                Math.DivRem(i, 3, out int rem3);
                Math.DivRem(i, 5, out int rem5);

                if (rem3 + rem5 == 0)
                    Console.WriteLine("FizzBuzz");
                else if (rem3 == 0)
                    Console.WriteLine("Fizz");
                else if (rem5 == 0)
                    Console.WriteLine("Buzz");
                else
                    Console.WriteLine(i);

            }
        }
    }
}
