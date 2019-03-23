using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitConverterClass
{
    public static class DigitConverterClass
    {
        public static int ConvertToInt(string inputNumber)
        {
            int resultNumber = 0;
            for (int i = 0; i < inputNumber.Length; i++)
            {
                resultNumber = resultNumber * 10 + ConvertCharToInt(inputNumber[i]);
            }

            return resultNumber;
        }

        private static int ConvertCharToInt(char digit)
        {
            return digit - '0';
        }
    }
}
