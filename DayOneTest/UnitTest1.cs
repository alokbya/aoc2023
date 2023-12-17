using NUnit.Framework;
using System.IO;
using AdventEngine;

[TestFixture]
public class DayOneTests
{
    [Test]
    public void ReadFile_ShouldCalculateSumOfCoordinates()
    {
        // Arrange
        string filePath = "test_input.txt";
        
        string[] lines = { "two1nine", "eightwothree", "abcone2threexyz", "xtwone3four", "4nineeightseven2", "zoneight234", "7pqrstsixteen" };

        File.WriteAllLines(filePath, lines);

        // Act
        var result = DayOne.ReadFile(filePath);

        // Assert
        // Add your assertions here

        Assert.AreEqual(281, result);
    }

    [TestCase("abc123def", ExpectedResult = 13)]
    [TestCase("xyz456uvw", ExpectedResult = 46)]
    [TestCase("123abc789", ExpectedResult = 19)]
    [TestCase("7abc", ExpectedResult = 77)]
    [TestCase("", ExpectedResult = 0)]
    [TestCase("abcdef", ExpectedResult = 0)]
    [TestCase("123456", ExpectedResult = 16)]
    [TestCase(null, ExpectedResult = 0)]
    [TestCase("12345678901234567890", ExpectedResult = 10)]
    [TestCase("Ihavefourapplesandsevenoranges", ExpectedResult = 47)]
    [TestCase("5Shebakedonecakeandsixcookiesfortheevent", ExpectedResult = 56)]
    [TestCase("5Shebakedonecakeandsixcookiesfortheevent99999", ExpectedResult = 59)]
    [TestCase("canlkeingone5weroijlkvnine", ExpectedResult = 19)]
    [TestCase("oneteoifjlk", ExpectedResult = 11)]
    [TestCase("onqdlkjgnineweni", ExpectedResult = 99)]
    [TestCase("two1nine", ExpectedResult = 29)]
    [TestCase("eightwothree", ExpectedResult = 83)]
    [TestCase("abcone2threexyz", ExpectedResult = 13)]
    [TestCase("xtwone3four", ExpectedResult = 24)]
    [TestCase("4nineeightseven2", ExpectedResult = 42)]
    [TestCase("zoneight234", ExpectedResult = 14)]
    [TestCase("7pqrstsixteen", ExpectedResult = 76)]
    [TestCase("9ninefour8fourpd2threetwo", ExpectedResult = 92)]
    [TestCase("oneight", ExpectedResult = 18)]
    public int GetCoordinates_ShouldReturnCorrectCoordinates(string line)
    {
        // Arrange & Act
        int result = DayOne.GetCoordinates(line);

        // Assert
        return result;
    }

    [Test]
    public void ReadFile_ShouldCalculateSumOfCoordinates_EmptyString()
    {
        // Arrange
        string filePath = "test_input.txt";
        string[] lines = { "" };
        File.WriteAllLines(filePath, lines);

        // Act
        var result = DayOne.ReadFile(filePath);

        // Assert
        Assert.AreEqual(0, result);
    }
}