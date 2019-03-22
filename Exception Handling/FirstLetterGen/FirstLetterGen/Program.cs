using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstLetterGen.Library;

namespace FirstLetterGen
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var input = Console.ReadLine();
                Console.WriteLine(FirstLetterGen.Library.FirstLetterGen.ReturnFirstLetter(input));
            }            
        }
    }
}
