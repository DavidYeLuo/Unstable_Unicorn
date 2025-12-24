using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnstableUnicorn
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
        public int count; // Not sure what this is for
        public string title;
        public string type;
        public string image;
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
        public CardType cardType;
        public Card(int id, CardType cardType)
        {
            this.id = id;
            this.cardType = cardType;
        }
    }

    public class Deck
    {
        private Card[] deck;
        int size;
        public Deck(Card[] deck)
        {
            this.deck = deck;
            size = deck.Length;
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
        public Card Draw()
        {
            Card ret = deck[size - 1];
            size--;
            return ret;
        }
    }

    public struct CardImage
    {
        public string image;
    }

    public struct PlayerHand
    {
        public Card[] cards;
        public int size;
    }

    public struct CardContext
    {
        public readonly Dictionary<string, CardType> cardTypeMap;
        public readonly string[] titleMap;
        public readonly CardImage[] cardImageMap;
        public CardContext(Dictionary<string, CardType> cardTypeMap, string[] titleMap, CardImage[] cardImageMap)
        {
            this.cardTypeMap = cardTypeMap;
            this.titleMap = titleMap;
            this.cardImageMap = cardImageMap;
        }
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextAsset _cardsJSON;
        [SerializeField] private int _numPlayers;
        [SerializeField] private int _startingHandSize = 5;
        [SerializeField] private int _maxHandCards = 32;
        [Space]
        [SerializeField] private CardDisplay mainPlayerHand;
        private Deck _deck;
        private CardImage[] _cardImageMap;
        private string[] _titleMap;

        private PlayerHand[] playersHands;

        public CardContext cardContext { get; private set; }

        private Dictionary<string, CardType> _cardTypeMap = new Dictionary<string, CardType>()
        {
            {"baby",CardType.Baby},
            {"basic",CardType.Basic},
            {"unicorn",CardType.Unicorn},
            {"narwhal",CardType.Narwhal},

            {"neigh",CardType.Neigh},
            {"super_neigh",CardType.Super_Neigh},

            {"upgrade",CardType.Upgrade},
            {"downgrade",CardType.Downgrade},

            {"magic",CardType.Magic},
        };

        private void Start()
        {
            string jsonString = _cardsJSON.text;
            DeckInfo deckInfo = JsonUtility.FromJson<DeckInfo>(jsonString);
            int deckSize = 0;
            foreach (var cardInfo in deckInfo.cards)
            {
                deckSize += cardInfo.count;
            }
            Card[] arrayCards = new Card[deckSize];
            _cardImageMap = new CardImage[deckInfo.cards.Length];
            _titleMap = new string[deckInfo.cards.Length];
            for (int i = 0, deckIndex = 0; i < deckInfo.cards.Length; i++)
            {
                CardInfo cardInfo = deckInfo.cards[i];
                Card card = new Card(i, _cardTypeMap[cardInfo.type]);
                _cardImageMap[i].image = cardInfo.image;
                _titleMap[i] = cardInfo.title;
                for (int count = 0; count < deckInfo.cards[i].count; count++, deckIndex++)
                {
                    arrayCards[deckIndex] = card;
                }
            }
            cardContext = new CardContext(_cardTypeMap, _titleMap, _cardImageMap);
            _deck = new Deck(arrayCards);
            _deck.Shuffle();
            foreach (Card card in arrayCards)
            {
                Debug.Log($"title: {_titleMap[card.id]}");
            }

            playersHands = new PlayerHand[_numPlayers];
            for (int playerIndex = 0; playerIndex < _numPlayers; playerIndex++)
            {
                playersHands[playerIndex].cards = new Card[_maxHandCards];
                for (int i = 0; i < _startingHandSize; i++)
                {
                    playersHands[playerIndex].cards[i] = _deck.Draw();
                }
                playersHands[playerIndex].size += _startingHandSize;
            }

            // View
            mainPlayerHand.SetCardContext(cardContext); // Init
            mainPlayerHand.UpdateHand(playersHands[0]);
        }
    }
}
