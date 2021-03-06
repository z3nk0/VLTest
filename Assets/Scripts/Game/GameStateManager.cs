﻿
namespace VLTest.Game
{
    public static class GameStateManager
    {
        public enum GameState
        {
            START = 0,
            MENU = 1,
            GAME = 2,
            LOSE = 3,
            WIN = 4,
            PAUSE = 5
        }

        #region EVENTS
        public delegate void GameStateEvent(GameState last, GameState current);
        public static event GameStateEvent OnGameStateChanges;
        #endregion

        #region MEMBERS
        private static GameState _currentState = GameState.START;

        public static GameState currentState
        {
            get { return _currentState; }
            set
            {
                if (value != currentState)
                {
                    GameState last = _currentState;
                    _currentState = value;
                    if (OnGameStateChanges != null)
                    {
                        OnGameStateChanges(last, currentState);
                    }
                }
            }
        }
        #endregion
    }
}