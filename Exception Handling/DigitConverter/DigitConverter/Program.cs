﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitConverterClass;

namespace DigitConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(DigitConverterClass.DigitConverterClass.ConvertToInt(Console.ReadLine()));
            }
        }
    }
}
