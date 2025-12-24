using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace UnstableUnicorn
{
    public class CardDisplay : MonoBehaviour
    {
        [SerializeField] private CardTemplate cardTemplate;
        [SerializeField] private int capacity = 32;
        private CardTemplate[] userHand;
        private CardContext context;
        private void Start()
        {
            userHand = new CardTemplate[capacity];
            for (int i = 0; i < capacity; i++)
            {
                CardTemplate current = Instantiate(cardTemplate);
                userHand[i] = current;
                current.gameObject.transform.SetParent(transform);
                current.gameObject.SetActive(false);
            }
        }
        // Probably not a good design...
        public void SetCardContext(CardContext context)
        {
            this.context = context;
        }
        public void UpdateHand(PlayerHand mainPlayerHand)
        {
            for (int i = 0; i < mainPlayerHand.size; i++)
            {
                userHand[i].text.text = context.titleMap[mainPlayerHand.cards[i].id];
                userHand[i].gameObject.SetActive(true);
            }
            for (int i = mainPlayerHand.size; i < capacity; i++)
            {
                userHand[i].gameObject.SetActive(false);
            }
        }
    }
}
