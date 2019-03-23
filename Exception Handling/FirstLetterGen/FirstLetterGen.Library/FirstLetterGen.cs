using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLetterGen.Library
{
    public static class FirstLetterGen
    {
        public static string ReturnFirstLetter(string input)
        {
            try
            {
                if(input.First() == ' ')
                {
                    return "<SPACE>";
                }
                return input.First().ToString();
            }
            catch (InvalidOperationException)
            {
                return "<EMPTY STRING>";
            }
        }
    }
}
