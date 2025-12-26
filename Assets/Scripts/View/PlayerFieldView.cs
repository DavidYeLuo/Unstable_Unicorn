using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Core;
namespace UnstableUnicorn
{
    public class PlayerFieldView : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private CardView cardPrefab;
        [Header("GameObjects Location")]
        [SerializeField] private GameObject handGameOject;
        [SerializeField] private GameObject spellGameObject;
        [SerializeField] private GameObject unicornGameObject;
        [SerializeField] private int capacity = 32; // Capacity
        private CardView[] handView;
        private CardView[] spellView;
        private CardView[] unicornView;

        private CardContext context; // Maps id to useful information
        /// 
        private void Awake()
        {
            handView = new CardView[capacity];
            for (int i = 0; i < capacity; i++)
            {
                CardView current = Instantiate(cardPrefab);
                handView[i] = current;
                current.gameObject.transform.SetParent(handGameOject.gameObject.transform);
                current.gameObject.SetActive(false);
            }
            spellView = new CardView[capacity];
            for (int i = 0; i < capacity; i++)
            {
                CardView current = Instantiate(cardPrefab);
                spellView[i] = current;
                current.gameObject.transform.SetParent(spellGameObject.gameObject.transform);
                current.gameObject.SetActive(false);
            }
            unicornView = new CardView[capacity];
            for (int i = 0; i < capacity; i++)
            {
                CardView current = Instantiate(cardPrefab);
                unicornView[i] = current;
                current.gameObject.transform.SetParent(unicornGameObject.gameObject.transform);
                current.gameObject.SetActive(false);
            }
        }
        // Probably not a good design...
        public void SetCardContext(CardContext context)
        {
            this.context = context;
        }
        public void UpdateHandView(Card[] cards, int handSize)
        {
            for (int i = 0; i < handSize; i++)
            {
                handView[i].title.text = context.titleMap[cards[i].id];
                handView[i].description.text = context.idStringDescriptionMap[cards[i].id];
                handView[i].type.text = context.idStringCardTypeMap[cards[i].id];
                handView[i].image.texture = context.idImageMap[cards[i].id];
                handView[i].backSide.SetActive(false);
                handView[i].gameObject.SetActive(true);
            }
            for (int i = handSize; i < capacity; i++)
            {
                handView[i].gameObject.SetActive(false);
            }
        }
        // Instead of showing cards, the cards will appear in back position
        public void UpdateWithHiddenCards(int handSize)
        {
            for (int i = 0; i < handSize; i++)
            {
                handView[i].backSide.SetActive(true);
                handView[i].gameObject.SetActive(true);
            }
            for (int i = handSize; i < capacity; i++)
            {
                handView[i].gameObject.SetActive(false);
            }

        }
        public void UpdateUnicornStableView(Card[] cards, int handSize)
        {
            for (int i = 0; i < handSize; i++)
            {
                unicornView[i].title.text = context.titleMap[cards[i].id];
                unicornView[i].description.text = context.idStringDescriptionMap[cards[i].id];
                unicornView[i].type.text = context.idStringCardTypeMap[cards[i].id];
                unicornView[i].image.texture = context.idImageMap[cards[i].id];
                unicornView[i].gameObject.SetActive(true);
            }
            for (int i = handSize; i < capacity; i++)
            {
                unicornView[i].gameObject.SetActive(false);
            }
        }
        public void UpdateSpellStableView(Card[] cards, int handSize)
        {
            for (int i = 0; i < handSize; i++)
            {
                spellView[i].title.text = context.titleMap[cards[i].id];
                spellView[i].description.text = context.idStringDescriptionMap[cards[i].id];
                spellView[i].type.text = context.idStringCardTypeMap[cards[i].id];
                spellView[i].image.texture = context.idImageMap[cards[i].id];
                spellView[i].gameObject.SetActive(true);
            }
            for (int i = handSize; i < capacity; i++)
            {
                spellView[i].gameObject.SetActive(false);
            }
        }
    }
}
