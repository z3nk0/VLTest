﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLTest.Game;

namespace VLTest.UI
{
    public class PauseManager : MonoBehaviour
    {
        #region MEMBERS
        public string pauseInput = "Pause";

        private bool pauseDown;
        #endregion

        #region PRIVATE METHODS
        private void Update()
        {
            bool pausePressed = Input.GetAxis(pauseInput) != 0;

            if (!pauseDown && pausePressed)
            {
                Pause();
                pauseDown = true;
            }
            if (pauseDown && !pausePressed)
            {
                pauseDown = false;
            }
        }

        private void Pause()
        {
            GameStateManager.currentState = GameStateManager.GameState.PAUSE;
        }
        #endregion
    }
}