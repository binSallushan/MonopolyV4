using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.ActionCards
{
    public class CardDeck
    {
        public CardType DeckType { get; }
        public IReadOnlyCollection<ICard> Cards { get; }
        private readonly Queue<ICard> cardsQueue;
        private readonly List<ICard> unresolvedCards;
        public CardDeck(ICard[] cards)
        {
            ArgumentNullException.ThrowIfNull(cards, nameof(cards));
            if (cards.Length == 0) throw new ArgumentException("Empty Array.");
            if (cards.DistinctBy(card => card.CardType).Count() != 1) throw new ArgumentException("Cards must have the same type.");

            DeckType = cards.First().CardType;
            Cards = cards;
            cardsQueue = [];
            unresolvedCards = [];
            ShuffleCards();            
        }

        public void ShuffleCards()
        {
            cardsQueue.Clear();
            var shuffledList = new List<ICard>();
            
            var rnd = new Random();
            var cardList = Cards.ToList();
            var count = cardList.Count;

            for (int i = 0; i < count; i++)
            {
                var index = rnd.Next(0, cardList.Count);
                shuffledList.Add(cardList[index]);
            }

            foreach(var card in shuffledList)            
                cardsQueue.Enqueue(card);            
        }

        public ICard TakeCard()
        {
            var resolvedCards = unresolvedCards.Where(x => x.IsResolved);
            foreach (var card in resolvedCards)
                InsertCard(card);

            return cardsQueue.Dequeue();
        }

        public void InsertCard(ICard card)
        {
            ArgumentNullException.ThrowIfNull(card, nameof(card));
            if (!Cards.Contains(card)) throw new ArgumentException("Card is not part of the deck.");

            if (!card.IsResolved)
                unresolvedCards.Add(card);
            else
                cardsQueue.Enqueue(card);
        }
    }
}
