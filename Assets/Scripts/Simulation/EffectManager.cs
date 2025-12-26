using UnityEngine;
using System.Collections.Generic;
using Core;

namespace UnstableUnicorn
{
    public enum InfoType : byte
    {
        Card,
        Effect,
        Player,
        Null,
    }
    /// This is like bytecode
    public struct Info
    {
        public InfoType type;
        public Card card;
        public int playerId; // Owner
        public Pile location;
    }
    /// This is basically an interpreter
    public class EffectParser
    {
        private Stack<Info> infoStack;
        private CardContext context;
        public EffectParser(CardContext context)
        {
            infoStack = new Stack<Info>();
            this.context = context;
        }
        public void Push(Info info)
        {
            infoStack.Push(info);
        }
        public void Process()
        {
            Process(infoStack.Pop());
        }
        public void Process(Info info)
        {

        }
    }
    public class EffectLexer
    {
        private CardContext context;
        public EffectLexer(CardContext context)
        {
            this.context = context;
        }

    }
}
