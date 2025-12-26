using UnityEngine;
using System;

namespace Core
{
    public enum CardType : byte
    {
        Baby,
        Basic,
        Unicorn,
        Narwhal,

        Neigh,
        Super_Neigh,

        Upgrade,
        Downgrade,

        Magic,
    }

    // JSON Format
    [Serializable]
    public struct CardInfo
    {
        public int count;
        public string title;
        public string type;
        public string image;
        public Description description;
    }

    // JSON Format
    [Serializable]
    public struct Description
    {
        public string en;
        public string de;
    }

    // JSON Format
    [Serializable]
    public struct DeckInfo
    {
        public CardInfo[] cards;
    }

    public struct Card
    {
        public int id;
        public Card(int id)
        {
            this.id = id;
        }
    }

    public struct GameSettings
    {
        public readonly int maxHandCards;
        public readonly int maxStableSize;
        public readonly int maxSpellFieldSize;
        public GameSettings(int maxHandCards, int maxStableSize, int maxSpellFieldSize)
        {
            this.maxHandCards = maxHandCards;
            this.maxStableSize = maxStableSize;
            this.maxSpellFieldSize = maxSpellFieldSize;
        }
    }

    public struct CardContext
    {
        public readonly CardType[] idCardTypeMap; // Map<id,CardType>
        public readonly int[] cardFreq;
        public readonly string[] idStringCardTypeMap; // Map<id,string>
        public readonly string[] titleMap; // Map<id,string>
        public readonly string[] stringImageMap; // Map<id,CardImage>
        public readonly string[] idStringDescriptionMap; // Map<id,string>
        public readonly Texture2D[] idImageMap; // Map<id,Texture2D>
        public CardContext(CardType[] idCardTypeMap,
                int[] cardFreq,
                string[] idStringCardTypeMap,
                Texture2D[] idImageMap,
                string[] titleMap,
                string[] idStringDescription,
                string[] cardImageMap)
        {
            this.idCardTypeMap = idCardTypeMap;
            this.cardFreq = cardFreq;
            this.idStringCardTypeMap = idStringCardTypeMap;
            this.titleMap = titleMap;
            this.idStringDescriptionMap = idStringDescription;
            this.stringImageMap = cardImageMap;
            this.idImageMap = idImageMap;
        }
    }
    public class Deck
    {
        private Card[] deck;
        public int size { get; private set; }
        public readonly int capacity;
        public Deck(Card[] deck)
        {
            this.deck = deck;
            size = deck.Length;
            capacity = deck.Length;
        }
        public void Shuffle()
        {
            for (int i = 0; i < size - 1; i++)
            {
                int rng = UnityEngine.Random.Range(i + 1, size);
                Card temp = deck[i];
                deck[i] = deck[rng];
                deck[rng] = temp;
            }
        }
        public void Add(Card card)
        {
            deck[size] = card;
            size++;
        }
        public Card Draw()
        {
            Card ret = deck[size - 1];
            size--;
            return ret;
        }
        public static Deck CreateDeckFrom(CardContext cardContext, int uniqueCards, int totalCards)
        {
            Card[] deckAsArray = new Card[totalCards];
            for (int i = 0, index = 0; i < uniqueCards; i++)
            {
                int freq = cardContext.cardFreq[i];
                for (int j = 0; j < freq; j++, index++)
                {
                    deckAsArray[index].id = i;
                }
            }
            return new Deck(deckAsArray);
        }
    }

    public struct Pile
    {
        public Card[] cards;
        public int size;
        public int Find(Card card)
        {
            for (int i = 0; i < size; i++)
            {
                if (cards[i].id == card.id) return i;
            }
            return -1;
        }
        public Card Take(int index)
        {
            Card ret = cards[index];
            // shift cards since the index no longer valid
            for (int i = index; i < size - 1; i++)
            {
                cards[i] = cards[i + 1];
            }
            size--;
            return ret;
        }
        public void Add(Card card)
        {
            cards[size] = card;
            size++;
        }
    }

    public struct PlayerField
    {
        public Pile hand;
        public Pile stable;
        public Pile spellField;
    }

    public struct BoardGame
    {
        public PlayerField[] playerFields;
        public Deck deck;
        public Pile discardPile;
        public Pile nursery;
    }

}
