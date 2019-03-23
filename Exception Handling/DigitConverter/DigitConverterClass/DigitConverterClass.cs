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
            bool isNegative = false;
            int startPoint = 0;

            if(inputNumber[0] == '-')
            {
                isNegative = true;
                startPoint = 1;
            }

            if (inputNumber[0] == '+')
            {
                startPoint = 1;
            }

            for (int i = startPoint; i < inputNumber.Length; i++)
            {
                resultNumber = resultNumber * 10 + ConvertCharToInt(inputNumber[i]);                
            }
            if (isNegative)
            {
                resultNumber = -resultNumber;
            }

            return resultNumber;
        }

        private static int ConvertCharToInt(char digit)
        {
            return digit - '0';
        }
    }
}
