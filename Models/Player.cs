using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CardGame.Models
{
    public class Player
    {
        public string Name { get; }
        public List<Card> Cards { get; }

        public Player(string name)
        {
            Name = name;
            Cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public override string ToString()
        {
            return $"{Name}: {string.Join(", ", Cards)}";
        }
    }
}
