using System.ComponentModel.DataAnnotations;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AdventEngine
{
    /// <summary>
    /// A class to hold the solutions for Day 1 of Advent of Code 2023.
    /// https://adventofcode.com/2023/day/1 
    /// </summary>
    public static class DayOne
    {
        public static int ReadFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            TrebuchetCoordinates tc = new TrebuchetCoordinates();

            foreach (string line in lines)
            {
                tc.Add(new Coordinate(line));
            }

            return tc.Sum;
        }

        public static int GetCoordinates(string line)
        {
            var coordinate = new Coordinate(line);
            return coordinate.ConcatValue;
        }

        public class TrebuchetCoordinates
        {
            private List<ICoordinate> _coordinates = new List<ICoordinate>();
            public int Sum {get {
                int sum = 0;
                foreach (var coordinate in _coordinates)
                {
                    sum += coordinate.ConcatValue;
                }
                return sum;
            }} 

            public void Add(ICoordinate coordinate)
            {
                _coordinates.Add(coordinate);
            }
        }

        public class Coordinate : ICoordinate
        {
            public NumberCoordinate NumberCoordinate {get; set;}
            public StringCoordinate StringCoordinate {get; set;}

            private int _concatValue = 0;

            public int ConcatValue => _concatValue;

            public Coordinate(NumberCoordinate numberCoordinate, StringCoordinate stringCoordinate)
            {
                NumberCoordinate = numberCoordinate;
                StringCoordinate = stringCoordinate;
                Resolve();
            }

            public Coordinate(string line) : 
            this(new NumberCoordinate(line), new StringCoordinate(line))
            {
            }

            /// <summary>
            /// Set the Concat value based on the index of the first coordinate and the index of the second coordinate index.
            /// </summary>
            private void Resolve()
            {
                // handle the case where NumberCoordinate is invalid
                if (NumberCoordinate.Status == CoordinateStatus.Invalid && StringCoordinate.Status == CoordinateStatus.Invalid)
                {
                    _concatValue = 0;
                    return;
                }

                if (NumberCoordinate.Status == CoordinateStatus.Valid && StringCoordinate.Status == CoordinateStatus.Invalid)
                {
                    _concatValue = NumberCoordinate.ConcatValue;
                    return;
                }

                if (NumberCoordinate.Status == CoordinateStatus.Invalid && StringCoordinate.Status == CoordinateStatus.Valid)
                {
                    _concatValue = StringCoordinate.ConcatValue;
                    return;
                }

                // both coordinates are valid
                if (NumberCoordinate.FirstIndex < StringCoordinate.FirstIndex)
                {
                    var firstValue = NumberCoordinate.FirstValue;
                    if (NumberCoordinate.SecondIndex > StringCoordinate.SecondIndex)
                    {
                        var secondValue = NumberCoordinate.SecondValue;
                        _concatValue = Int32.Parse(firstValue.ToString() + secondValue.ToString());
                    }
                    else
                    {
                        var secondValue = StringCoordinate.SecondValue;
                        _concatValue = Int32.Parse(firstValue.ToString() + CoordinateTranslator.Numbers[secondValue!.ToString()].ToString());
                    }
                    return;
                }
                else
                {
                    var firstValue = StringCoordinate.FirstValue;
                    if (NumberCoordinate.SecondIndex > StringCoordinate.SecondIndex)
                    {
                        var secondValue = NumberCoordinate.SecondValue;
                        _concatValue = Int32.Parse(CoordinateTranslator.Numbers[firstValue!.ToString()].ToString() + secondValue.ToString());
                    }
                    else
                    {
                        var secondValue = StringCoordinate.SecondValue;
                        _concatValue = Int32.Parse(CoordinateTranslator.Numbers[firstValue!.ToString()].ToString() + CoordinateTranslator.Numbers[secondValue!.ToString()].ToString());
                    }
                    return;
                }

            }
        }

        public class NumberCoordinate : ICoordinate
        {
            public CoordinateStatus Status {get; set;} = CoordinateStatus.Invalid;
            public int FirstValue {get; set;} = -1;
            public int SecondValue {get; set;} = -1;

            public int FirstIndex {get; set;} = -1;
            public int SecondIndex {get; set;} = -1;

            public int ConcatValue {
            get
            {
                if (FirstValue == -1 || SecondValue == -1) // both values are -1
                    return 0;
                return Int32.Parse(FirstValue.ToString() + SecondValue.ToString());
            }}

            public NumberCoordinate(string line)
            {
                GetCoordinate(line ?? string.Empty);
            }

            public NumberCoordinate GetCoordinate(string line)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    // check if char is a number
                    if (Char.IsDigit(line[i]))
                    {
                        FirstValue = Int32.Parse(line[i].ToString());
                        FirstIndex = i;
                        break;
                    }
                }

                for (int i = line.Length - 1; i >= 0; i--)
                {
                    // check if char is a number
                    if (Char.IsDigit(line[i]))
                    {
                        SecondValue = Int32.Parse(line[i].ToString());
                        SecondIndex = i;
                        break;
                    }
                }

                if (FirstIndex != -1 && SecondIndex != -1)
                    Status = CoordinateStatus.Valid;

                return this;
            }
        }

        public class StringCoordinate : ICoordinate
        {
            public CoordinateStatus Status {get; set;} = CoordinateStatus.Invalid;
            public string? FirstValue {get; set;}
            public string? SecondValue {get; set;}
            public int FirstIndex {get; set;} = -1;
            public int SecondIndex {get; set;} = -1;

            public int ConcatValue { 
            get
            {
                if (FirstValue is null || SecondValue is null) // both values are null
                    return 0;

                CoordinateTranslator.Numbers.TryGetValue(FirstValue!, out int first);
                CoordinateTranslator.Numbers.TryGetValue(SecondValue!, out int second);
                
                return Int32.Parse(first.ToString() + second.ToString());
            }}

            public StringCoordinate(string line)
            {
                GetCoordinate(line ?? string.Empty);
            }

            public StringCoordinate GetCoordinate(string line)
            {
                var regexPattern = @"(?=(zero|one|two|three|four|five|six|seven|eight|nine|ten))";

                var matches = Regex.Matches(line.ToLower(), regexPattern, RegexOptions.IgnoreCase);

                // if no matches are found, return the default values and set the status to invalid
                if (matches.Count == 0)
                {
                    Status = CoordinateStatus.Invalid;
                    return this;
                }

                foreach (Match match in matches)
                {
                    if (FirstIndex == -1)
                    {
                        // update all the values. If only one match is found, then the second value == first value
                        FirstIndex = match.Groups[1].Index;
                        FirstValue = match.Groups[1].Value.ToLower();
                        SecondIndex = match.Groups[1].Index;
                        SecondValue = match.Groups[1].Value.ToLower();
                        Status = CoordinateStatus.Valid;
                    }
                    else
                    {
                        SecondIndex = match.Groups[1].Index;
                        SecondValue = match.Groups[1].Value.ToLower();
                    }
                }

                return this;
            }
        }

        public static class CoordinateTranslator
        {
            public static Dictionary<string, int> Numbers = new Dictionary<string, int>()
            {
                {"zero", 0},
                {"one", 1},
                {"two", 2},
                {"three", 3},
                {"four", 4},
                {"five", 5},
                {"six", 6},
                {"seven", 7},
                {"eight", 8},
                {"nine", 9},
            };
        }

        public enum CoordinateStatus
        {
            Invalid,
            Valid
        }

        public interface ICoordinate
        {
            int ConcatValue {get;}
        }
    }
}