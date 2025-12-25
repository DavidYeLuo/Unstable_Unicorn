using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace UnstableUnicorn
{
    [Serializable]
    public class CardView : MonoBehaviour
    {
        public TMP_Text title;
        public TMP_Text description;
        public TMP_Text type;
        public RawImage image;
        public GameObject backSide;
    }
}
