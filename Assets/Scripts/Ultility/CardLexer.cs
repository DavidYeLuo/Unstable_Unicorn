using UnityEngine;
using System.Collections.Generic;
using Core;

namespace Utility
{
    public struct LexResult
    {
        public readonly CardContext cardContext;
        public readonly int uniqueCards;
        public readonly int totalCards;
        public LexResult(CardContext cardContext,
                int uniqueCards,
                int totalCards)
        {
            this.cardContext = cardContext;
            this.uniqueCards = uniqueCards;
            this.totalCards = totalCards;
        }
    }
    /// Transforms String Data into another
    public class CardLexer
    {
        private Dictionary<string, CardType> _cardTypeMap;
        private Dictionary<string, Texture2D> _stringTextureMap;
        public CardLexer(Dictionary<string, CardType> cardTypeMap, Dictionary<string, Texture2D> stringTextureMap)
        {
            _cardTypeMap = cardTypeMap;
            _stringTextureMap = stringTextureMap;
        }
        public LexResult LexCards(DeckInfo deckInfo)
        {
            int uniqueCardSize = deckInfo.cards.Length;
            int totalCards = 0;
            int[] _cardFreq = new int[uniqueCardSize];
            string[] _titleMap = new string[uniqueCardSize];
            string[] _cardImageMap = new string[uniqueCardSize];
            string[] _idStringDescription = new string[uniqueCardSize];
            Texture2D[] _idImageMap = new Texture2D[uniqueCardSize];
            CardType[] _idCardTypeMap = new CardType[uniqueCardSize];
            string[] _idStringCardTypeMap = new string[uniqueCardSize];
            for (int i = 0; i < deckInfo.cards.Length; i++)
            {
                CardInfo cardInfo = deckInfo.cards[i];
                _titleMap[i] = cardInfo.title;
                _cardFreq[i] = cardInfo.count;
                totalCards += cardInfo.count;
                _idStringDescription[i] = cardInfo.description.en; // Sorry only english atm
                _cardImageMap[i] = cardInfo.image;
                _idImageMap[i] = _stringTextureMap[cardInfo.image]; // NOTE: image must exist
                _idStringCardTypeMap[i] = cardInfo.type;
                _idCardTypeMap[i] = _cardTypeMap[cardInfo.type];
            }
            CardContext context = new CardContext(_idCardTypeMap,
                    _cardFreq,
                    _idStringCardTypeMap,
                    _idImageMap,
                    _titleMap,
                    _idStringDescription,
                    _cardImageMap);

            return new LexResult(context, uniqueCardSize, totalCards);
        }
    }
}
