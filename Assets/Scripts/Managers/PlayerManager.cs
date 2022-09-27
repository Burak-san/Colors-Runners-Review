using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Keys;
using Data.ValueObject;
using Data.UnityObject;
using Signals;
using Controllers.Player;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public PlayerData PlayerData;
        public GameStates CurrentGameState;

        public ColorType PlayerColorType;

        #endregion

        #region Serialized Variables

        [Space][SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private PlayerMeshController playerMeshController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private GameObject playerMesh;
        #endregion

        #region Private Variables
        #endregion

        #endregion

        private void Awake()
        {
            PlayerData = GetPlayerData();
            SendPlayerDataToControllers();
        }

        

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>("Data/CD_Player").PlayerData;

        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(PlayerData.PlayerMovementData);
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;

            InputSignals.Instance.onInputTaken += OnActivateMovement;
            InputSignals.Instance.onInputReleased += OnDeactivateMovement;
            InputSignals.Instance.onRunnerInputDragged += OnGetRunnerInputValues;
            InputSignals.Instance.onIdleInputDragged += OnGetIdleInputValues;

            PlayerSignals.Instance.onIncreaseScale += OnIncreaseScale;
            PlayerSignals.Instance.onDecreaseScale += OnDecreaseScale;

            LevelSignals.Instance.onLevelFailed += OnLevelFailed;

        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;

            InputSignals.Instance.onInputTaken -= OnActivateMovement;
            InputSignals.Instance.onInputReleased -= OnDeactivateMovement;
            InputSignals.Instance.onRunnerInputDragged -= OnGetRunnerInputValues;
            InputSignals.Instance.onIdleInputDragged -= OnGetIdleInputValues;

            PlayerSignals.Instance.onIncreaseScale -= OnIncreaseScale;
            PlayerSignals.Instance.onDecreaseScale -= OnDecreaseScale;

            LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        #endregion

        #region Subscribed Methods

        private void OnPlay()
        {
            movementController.IsReadyToPlay(true);
        }

        private void OnChangeGameState(GameStates gameStates)
        {
            CurrentGameState = gameStates;
            if(gameStates == GameStates.Idle)
            {
                ActivateAllMovement(true);
            }
        }

        private void OnActivateMovement()
        {
            movementController.EnableMovement();
        }

        private void OnDeactivateMovement()
        {
            movementController.DisableMovement();
            movementController.SetRunnerMovementValues(0, 0);
            animationController.ChangeCollectableAnimation(PlayerAnimationStates.Idle);
        }

        private void OnGetRunnerInputValues(RunnerGameInputParams runnerGameInputParams)
        {
            movementController.UpdateRunnerInputValue(runnerGameInputParams);
        }

        private void OnGetIdleInputValues(IdleGameInputParams idleGameInputParams)
        {
            movementController.UpdateIdleInputValue(idleGameInputParams);
            animationController.ChangeCollectableAnimation(PlayerAnimationStates.Running);
            Vector3 movementDirection = new Vector3(idleGameInputParams.XValue, 0, idleGameInputParams.ZValue);
            if(movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                animationController.transform.rotation = Quaternion.RotateTowards(animationController.transform.rotation, toRotation, 30);
                ParticleSignals.Instance.onParticleLookRotation.Invoke(toRotation);
            }
        }

        public void SendGateColorData(ColorType colorType)
        {
            StackSignals.Instance.onColorChange?.Invoke(colorType);
            PlayerColorType = colorType;
            playerMeshController.SetColor(colorType);
        }

        public void ChangeState(GameStates CurrentState)
        {
            CoreGameSignals.Instance.onChangeGameState?.Invoke(CurrentState);
            if (CurrentState == GameStates.Runner)
            {
                playerMesh.SetActive(false);
            }
            else
            {
                playerMesh.SetActive(true);
            }
        }

        public void EnableFinalCollider(GameObject other)
        {
            movementController.StartVerticalMovement(other.gameObject);
        }

        public void StopVerticalMovement()
        {
            movementController.StopVerticalMovement();
        }
        public void ActivateAllMovement(bool Activate)
        {
            movementController.IsReadyToPlay(Activate);
        }

        public void OnIncreaseScale()
        {
            if(CurrentGameState == GameStates.Roullette)
            {
                playerMeshController.IncreaseSize();
            }
        }

        public void OnDecreaseScale()
        {
            if(CurrentGameState == GameStates.Idle)
            {
                playerMeshController.DecreaseSize();
            }
        }

        private void OnLevelFailed()
        {
            ActivateAllMovement(false);
        }
        private void OnReset()
        {
            movementController.Reset();
        }


        #endregion
    }
}

