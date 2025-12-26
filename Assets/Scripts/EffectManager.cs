using UnityEngine;
using System.Collections.Generic;

namespace UnstableUnicorn
{
    public enum CardEffect
    {
    }
    public struct Effect
    {
        int cardOwner;
    }
    public class EffectManager
    {
        private Stack<Effect> effectStack;
        public EffectManager()
        {
            effectStack = new Stack<Effect>();
        }
        public void Next()
        {

        }
    }
}
