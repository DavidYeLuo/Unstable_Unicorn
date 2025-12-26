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

    public class Deck
    {
        private Card[] deck;
        int size;
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

    public struct CardContext
    {
        public readonly CardType[] idCardTypeMap; // Map<id,CardType>
        public readonly string[] idStringCardTypeMap; // Map<id,string>
        public readonly string[] titleMap; // Map<id,string>
        public readonly string[] stringImageMap; // Map<id,CardImage>
        public readonly string[] idStringDescriptionMap; // Map<id,string>
        public readonly Texture2D[] idImageMap; // Map<id,Texture2D>
        public CardContext(CardType[] idCardTypeMap,
                string[] idStringCardTypeMap,
                Texture2D[] idImageMap,
                string[] titleMap,
                string[] idStringDescription,
                string[] cardImageMap)
        {
            this.idCardTypeMap = idCardTypeMap;
            this.idStringCardTypeMap = idStringCardTypeMap;
            this.titleMap = titleMap;
            this.idStringDescriptionMap = idStringDescription;
            this.stringImageMap = cardImageMap;
            this.idImageMap = idImageMap;
        }
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextAsset _cardsJSON;
        [SerializeField] private int _numPlayers;
        [SerializeField] private int _startingHandSize = 5;
        [SerializeField] private int _maxHandCards = 32;
        [SerializeField] private int _maxStableSize = 32;
        [SerializeField] private float secondsBeforeStateChange = 5.0f;
        [Space]
        [Header("View")]
        [SerializeField] private PlayerFieldView mainPlayerHand;
        [SerializeField] private PlayerFieldView[] otherPlayers;
        private Deck _deck;

        private PlayerField[] _playersFields; // Server side (Knows everyone's cards)
        private BoardGame _boardGame;

        // Contains all the card mappings
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

        private StateMachine _stateMachine;
        private float timeSinceStateChange = 0.0f;

        /// Loads JSON file and images containing card info thanks to the author from 
        /// https://github.com/geniegeist/unstable-unicorns
        /// They're loaded into CardContext
        private void Init()
        {
            // Deserialize JSON file
            string jsonString = _cardsJSON.text;
            DeckInfo deckInfo = JsonUtility.FromJson<DeckInfo>(jsonString);
            int deckSize = 0;
            int uniqueCardSize = deckInfo.cards.Length;
            foreach (var cardInfo in deckInfo.cards)
            {
                deckSize += cardInfo.count;
            }
            Card[] deckAsArray = new Card[deckSize];
            string[] _titleMap = new string[uniqueCardSize];
            string[] _cardImageMap = new string[uniqueCardSize];
            string[] _idStringDescription = new string[uniqueCardSize];
            Texture2D[] _idImageMap = new Texture2D[uniqueCardSize];
            Texture2D[] _images = Resources.LoadAll<Texture2D>("Textures");
            CardType[] _idCardTypeMap = new CardType[uniqueCardSize];
            string[] _idStringCardTypeMap = new string[uniqueCardSize];
            Dictionary<string, Texture2D> _stringImageMap = new Dictionary<string, Texture2D>(); // Helps with mapping string type to the right image
            foreach (Texture2D texture in _images)
            {
                _stringImageMap.Add(texture.name, texture);
            }
            for (int i = 0, deckIndex = 0; i < deckInfo.cards.Length; i++)
            {
                CardInfo cardInfo = deckInfo.cards[i];
                Card card = new Card(i);
                _titleMap[i] = cardInfo.title;
                _idStringDescription[i] = cardInfo.description.en; // Sorry only english atm
                _cardImageMap[i] = cardInfo.image;
                _idImageMap[i] = _stringImageMap[cardInfo.image]; // NOTE: image must exist
                _idStringCardTypeMap[i] = cardInfo.type;
                _idCardTypeMap[i] = _cardTypeMap[cardInfo.type];
                for (int count = 0; count < deckInfo.cards[i].count; count++, deckIndex++)
                {
                    deckAsArray[deckIndex] = card;
                }
            }
            cardContext = new CardContext(_idCardTypeMap, _idStringCardTypeMap, _idImageMap, _titleMap, _idStringDescription, _cardImageMap);
            _deck = new Deck(deckAsArray);

            _playersFields = new PlayerField[_numPlayers];
            for (int playerIndex = 0; playerIndex < _numPlayers; playerIndex++)
            {
                _playersFields[playerIndex].hand.cards = new Card[_maxHandCards];
                _playersFields[playerIndex].stable.cards = new Card[_maxStableSize];
                _playersFields[playerIndex].spellField.cards = new Card[_maxStableSize];
            }
            _boardGame = new BoardGame()
            {
                playerFields = _playersFields,
                deck = _deck,
                discardPile = new Pile() { cards = new Card[_deck.capacity] },
                nursery = new Pile() { cards = new Card[_deck.capacity] }
            };

        }
        private void Awake()
        {
            int[] playerChoices = new int[_numPlayers];
            for (int i = 0; i < _numPlayers; i++)
            {
                playerChoices[i] = i;
            }
            _stateMachine = new StateMachine(playerChoices);
            Init();
        }
        private void OnEnable()
        {
            _stateMachine.stateChange += OnStateChange;
        }
        private void OnDisable()
        {
            _stateMachine.stateChange -= OnStateChange;
        }
        private void Start()
        {
            _deck.Shuffle();
            for (int playerIndex = 0; playerIndex < _numPlayers; playerIndex++)
            {
                for (int i = 0; i < _startingHandSize; i++)
                {
                    _playersFields[playerIndex].hand.Add(_deck.Draw());
                }
            }

            // View
            mainPlayerHand.SetCardContext(cardContext); // Init
            mainPlayerHand.UpdateHandView(_playersFields[0].hand.cards, _playersFields[0].hand.size);
            // I don't think the game can display more than 4 players at once
            // maybe we can have a way to switch views
            for (int i = 0; i < 3; i++)
            {
                if (i >= _numPlayers) break;
                otherPlayers[i].SetCardContext(cardContext); // Init
                otherPlayers[i].UpdateWithHiddenCards(_startingHandSize);
            }
        }
        public void Update()
        {
            // Currently, it just cycles through states and then players
            if (timeSinceStateChange > secondsBeforeStateChange)
            {
                timeSinceStateChange = 0.0f;
                _stateMachine.Next();
            }
            timeSinceStateChange += Time.deltaTime;
        }
        private void OnStateChange(int player, GameState state)
        {
            switch (state)
            {
                case GameState.BeginingPhase:
                    // Prompt player for optional effects
                    break;
                case GameState.DrawPhase:
                    // player receives card
                    break;
                case GameState.ActionPhase:
                    // Prompt player choices
                    break;
                case GameState.EndPhase:
                    // Check if player has more than 7 cards
                    // (if card > 7) Prompt cards to discard
                    break;
            }
        }
    }
}
