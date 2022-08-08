using System;
using System.Threading.Tasks;

namespace RetornosAssincronos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Main ");

            var one = ThreadOne();
            Task two = ThreadTwo();
            Task three  = ThreadThree();

            Console.WriteLine("Main Esperando");
                       
            Console.WriteLine($"Main 01 - Result: " + $"{await one}");
            await two;
            Console.WriteLine("Main 02 ");
            await three;
            Console.WriteLine("Main 03 ");

            Console.WriteLine("Main Final");
        }

        static async Task<int> ThreadOne()
        {
            Console.WriteLine("Thread 01 " + DateTime.Now.ToLongTimeString());
            await Task.Delay(3000);
            Console.WriteLine("Thread 01 " + DateTime.Now.ToLongTimeString());

            return 1;
        }

        static async Task ThreadTwo()
        {
            Console.WriteLine("Thread 02 " + DateTime.Now.ToLongTimeString());
            await Task.Delay(5000);
            Console.WriteLine("Thread 02 " + DateTime.Now.ToLongTimeString());
        }

        static async Task ThreadThree()
        {
            Console.WriteLine("Thread 03 " + DateTime.Now.ToLongTimeString());
            await Task.Delay(1000);
            Console.WriteLine("Thread 03 " + DateTime.Now.ToLongTimeString());
        }
    }
}
