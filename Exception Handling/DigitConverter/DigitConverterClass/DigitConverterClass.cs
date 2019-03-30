using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitConverterClass.Exceptions;

namespace DigitConverterClass
{
    public static class DigitConverterClass
    {
        public static int ConvertToInt(string inputNumber)
        {
            if (string.IsNullOrEmpty(inputNumber))
            {
                ArgumentNullException ex = new ArgumentNullException($"{nameof(inputNumber)} is null or empty");
                throw ex;
            }
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
                if (IsDigit(inputNumber[i]))
                {
                    resultNumber = checked(resultNumber * 10 + ConvertCharToInt(inputNumber[i]));
                }
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

        private static bool IsDigit(char digit)
        {
            if(digit < '0' || digit > '9')
            {
                throw new NotADigitException("Provided char is not a digit");
            }
            return true;
        }
    }
}
