using Assignment_A2_01.Services;
using System;
using System.Linq;

namespace Assignment_A2_01
{
    class Program
    {
        static void Main(string[] args)
        {

            var t1 = new NewsService().GetNewsAsync();

            //Your Code

            Console.WriteLine($"Top Headlines");


            foreach (var item in t1.Result.Articles)
            {
                {
                    Console.WriteLine($"   -  {item.Title}");
                }
            }
        }
    }
}



