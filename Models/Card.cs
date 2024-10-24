using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Models
{
    public class Card : IComparable<Card>
    {
        public string Value { get; }
        public char Symbol { get; }

        private readonly int numericValue;

        private static readonly Dictionary<string, int> ValueOrder = new()
        {
            {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6}, {"7", 7},
            {"8", 8}, {"9", 9}, {"10", 10}, {"J", 11}, {"Q", 12}, {"K", 13}, {"A", 14}
        };

        private static readonly Dictionary<char, int> SymbolOrder = new()
        {
            {'@', 1}, {'#', 2}, {'^', 3}, {'*', 4}
        };

        public Card(string value, char symbol)
        {
            Value = value;
            Symbol = symbol;
            numericValue = ValueOrder[value];
        }

        public override string ToString() => $"{Value}{Symbol}";

        public int CompareTo(Card other)
        {
            if (numericValue != other.numericValue)
                return numericValue.CompareTo(other.numericValue);
            return SymbolOrder[Symbol].CompareTo(SymbolOrder[other.Symbol]);
        }
    }
}
