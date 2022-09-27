using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Signals;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public GameStates CurrentState;

        #endregion

        #endregion


        private void Awake()
        {
            Application.targetFrameRate = 60;
            

        }

        private void Start()
        {
            GameOpen();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
            GameClose();
        }
        #endregion

        private void GameOpen()
        {
            CurrentState = GameStates.Runner;
            CoreGameSignals.Instance.onGameOpen?.Invoke();
        }

        private void GameClose()
        {
            CoreGameSignals.Instance.onGameClose?.Invoke();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                CoreGameSignals.Instance.onGamePause?.Invoke();
            }
        }

        private void OnChangeGameState(GameStates NewCurrentState)
        {
            CurrentState = NewCurrentState;
        }
    }
}


