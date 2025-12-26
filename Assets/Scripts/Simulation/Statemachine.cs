using UnityEngine;
using System;

namespace UnstableUnicorn
{
    public enum GameState : byte
    {
        BeginingPhase,
        DrawPhase,
        ActionPhase,
        EndPhase,
    }
    public delegate void OnStateChange(int player, GameState state);
    public class StateMachine
    {
        public event OnStateChange stateChange;
        private GameState _state;
        public int Player { get; private set; }
        public int[] playerChoices;
        public GameState State
        {
            get => _state;
            set
            {
                _state = value;
                stateChange?.Invoke(this.Player, _state);
            }
        }

        public StateMachine(int[] playerChoices)
        {
            _state = GameState.BeginingPhase;
            this.playerChoices = playerChoices;
        }
        public void Next()
        {
            switch (_state)
            {
                case GameState.BeginingPhase:
                    State = GameState.DrawPhase;
                    break;
                case GameState.DrawPhase:
                    State = GameState.ActionPhase;
                    break;
                case GameState.ActionPhase:
                    State = GameState.EndPhase;
                    break;
                case GameState.EndPhase:
                    int index = (Player + 1) % playerChoices.Length;
                    Player = playerChoices[index];
                    State = GameState.BeginingPhase;
                    break;
                default:
                    throw new Exception("Game State not defined in StateMachine");
            }
        }
    }
}
