using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace UnstableUnicorn
{
    [Serializable]
    public class CardView : MonoBehaviour
    {
        public TMP_Text text;
        public RawImage image;
        public GameObject backSide;
    }
}
