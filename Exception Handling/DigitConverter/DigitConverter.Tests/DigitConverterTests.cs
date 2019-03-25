using System;
using NUnit.Framework;
using DigitConverterClass;

namespace DigitConverter.Tests
{
    public class Tests
    {
        [TestCase("123")]
        [TestCase("-123")]
        [TestCase("+123")]
        public void DigitConverter_CorrectNumberPassed_ExpectedCorrectBehaviour(string inputNumber)
        {
            // Arrange.
            var expectedNumber = int.Parse(inputNumber);
            int actualNumber;
            // Act.
            actualNumber = DigitConverterClass.DigitConverterClass.ConvertToInt(inputNumber);

            // Assert.
            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestCase("2147483648")]
        [TestCase("-2147483648")]
        [TestCase("214748364843523452")]
        public void DigitConverter_OverflowIntNumberPassed_ThrowsOverflowException(string inputNumber)
        {
            // Arrange.
            int actualNumber;

            // Act.
            void ConvertNumber() => actualNumber = DigitConverterClass.DigitConverterClass.ConvertToInt(inputNumber);

            // Assert.
            Assert.Throws<OverflowException>(ConvertNumber);
        }

        [TestCase("sdgsdfg3424gsdf")]
        [TestCase(" ")]
        [TestCase("    332322323efes ")]
        public void DigitConverter_IncorrectStringPassed_ThrowsArithmeticException(string inputNumber)
        {
            // Arrange.
            int actualNumber;

            // Act.
            void ConvertNumber() => actualNumber = DigitConverterClass.DigitConverterClass.ConvertToInt(inputNumber);

            // Assert.
            Assert.Throws<ArithmeticException>(ConvertNumber);
        }

        [TestCase("")]
        [TestCase(null)]
        public void DigitConverter_NullOrEmptyStringPassed_ThrowsArgumentNullException(string inputNumber)
        {
            // Arrange.
            int actualNumber;

            // Act.
            void ConvertNumber() => actualNumber = DigitConverterClass.DigitConverterClass.ConvertToInt(inputNumber);

            // Assert.
            Assert.Throws<ArgumentNullException>(ConvertNumber);
        }
    }
}