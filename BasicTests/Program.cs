using System;
using System.Collections.Generic;
using System.IO;

namespace BasicTests
{
    class Program
    {
        static void Main(string[] args)
        {

            // fizzbuzz

            //Console.WriteLine("Hello World!");

            //FizzBuzz.fizzBuzz(230);



            // card


            //int numsCount = Convert.ToInt32(Console.ReadLine().Trim());

            //List<int> nums = new List<int>();

            //for (int i = 0; i < numsCount; i++)
            //{
            //    int numsItem = Convert.ToInt32(Console.ReadLine().Trim());
            //    nums.Add(numsItem);
            //}

            //List<int> result = CardinalityResult.cardinalitySort(nums);

            //Console.WriteLine(String.Join("\n", result));



            // aladin

            //int magicCount = Convert.ToInt32(Console.ReadLine().Trim());

            //List<int> magic = new List<int>();

            //for (int i = 0; i < magicCount; i++)
            //{
            //    int magicItem = Convert.ToInt32(Console.ReadLine().Trim());
            //    magic.Add(magicItem);
            //}

            //int distCount = Convert.ToInt32(Console.ReadLine().Trim());

            //List<int> dist = new List<int>();

            //for (int i = 0; i < distCount; i++)
            //{
            //    int distItem = Convert.ToInt32(Console.ReadLine().Trim());
            //    dist.Add(distItem);
            //}

            //int result = AlladinResult.optimalPoint(magic, dist);

            //Console.WriteLine(result);


            // triangulo

            int x1 = Convert.ToInt32(Console.ReadLine().Trim());

            int y1 = Convert.ToInt32(Console.ReadLine().Trim());

            int x2 = Convert.ToInt32(Console.ReadLine().Trim());

            int y2 = Convert.ToInt32(Console.ReadLine().Trim());

            int x3 = Convert.ToInt32(Console.ReadLine().Trim());

            int y3 = Convert.ToInt32(Console.ReadLine().Trim());

            int xp = Convert.ToInt32(Console.ReadLine().Trim());

            int yp = Convert.ToInt32(Console.ReadLine().Trim());

            int xq = Convert.ToInt32(Console.ReadLine().Trim());

            int yq = Convert.ToInt32(Console.ReadLine().Trim());

            int result = TriResult.pointsBelong(x1, y1, x2, y2, x3, y3, xp, yp, xq, yq);

            Console.WriteLine(result);


        }
    }
}
