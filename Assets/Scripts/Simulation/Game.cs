using UnityEngine;
using Core;

namespace UnstableUnicorn
{
    public class Game
    {
        public readonly CardContext cardContext;
        public readonly GameSettings gameSettings;

        public PlayerField[] playersFields { get; private set; }
        public Pile discardPile { get; private set; }
        public Pile nursery { get; private set; }
        public Deck deck { get; private set; }

        public Game(CardContext cardContext, GameSettings gameSettings, Deck deck, int numPlayers)
        {
            this.gameSettings = gameSettings;
            this.deck = deck;
            playersFields = new PlayerField[numPlayers];
            for (int playerId = 0; playerId < numPlayers; playerId++)
            {
                playersFields[playerId].hand.cards = new Card[gameSettings.maxHandCards];
                playersFields[playerId].stable.cards = new Card[gameSettings.maxStableSize];
                playersFields[playerId].spellField.cards = new Card[gameSettings.maxSpellFieldSize];
            }
        }
        public void PlayerDrawFromDeck(int playerId, int numTimes)
        {
            for (int i = 0; i < numTimes; i++)
            {
                playersFields[playerId].hand.Add(deck.Draw());
            }
        }
        public void ShuffleDeck()
        {
            deck.Shuffle();
        }
    }
}
