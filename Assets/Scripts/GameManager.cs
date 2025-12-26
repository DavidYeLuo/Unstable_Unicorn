using UnityEngine;
using System;
using System.Collections.Generic;
using Utility; // Card Lexer
using Core;

namespace UnstableUnicorn
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextAsset _cardsJSON;
        [SerializeField] private int _numPlayers;
        [SerializeField] private int _startingHandSize = 5;
        [SerializeField] private int _maxHandCards = 32;
        [SerializeField] private int _maxStableSize = 32;
        [SerializeField] private int _maxSpellFieldSize = 32;
        [SerializeField] private float secondsBeforeStateChange = 5.0f;
        [Space]
        [Header("View")]
        [SerializeField] private PlayerFieldView mainPlayerHand;
        [SerializeField] private PlayerFieldView[] otherPlayers;
        private Deck _deck;

        private Game _game;

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
            Texture2D[] _images = Resources.LoadAll<Texture2D>("Textures");
            Dictionary<string, Texture2D> _stringImageMap = new Dictionary<string, Texture2D>(); // Helps with mapping string type to the right image
            foreach (Texture2D texture in _images)
            {
                _stringImageMap.Add(texture.name, texture);
            }
            CardLexer cardLexer = new CardLexer(_cardTypeMap, _stringImageMap);
            LexResult lexResult = cardLexer.LexCards(deckInfo);
            cardContext = lexResult.cardContext;
            _deck = Deck.CreateDeckFrom(cardContext, lexResult.uniqueCards, lexResult.totalCards);

            GameSettings gameSettings = new GameSettings(_maxHandCards, _maxStableSize, _maxSpellFieldSize);
            _game = new Game(cardContext, gameSettings, _deck, _numPlayers);
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
            _game.ShuffleDeck();
            for (int playerId = 0; playerId < _numPlayers; playerId++)
            {
                _game.PlayerDrawFromDeck(playerId, _startingHandSize);
            }

            // View
            mainPlayerHand.SetCardContext(cardContext); // Init
            mainPlayerHand.UpdateHandView(_game.playersFields[0].hand.cards, _game.playersFields[0].hand.size);
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
