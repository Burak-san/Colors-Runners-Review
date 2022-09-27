using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.ValueObject;
using Unity.Mathematics;
using Keys;
using Enums;
using Managers;
using DG.Tweening;

namespace Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        #endregion

        #region Serialized Variables

        [SerializeField] private new Rigidbody rigidbody;

        [SerializeField] private PlayerManager manager;
        #endregion

        #region Private Variables

        [Header("Data")][ShowInInspector] private PlayerMovementData _movementData;

        [ShowInInspector] private bool _isReadyToMove, _isReadyToPlay;

        [ShowInInspector] private float _inputValueX,_inputValueZ;

        [ShowInInspector] private Vector2 _clampValues;

        #endregion

        #endregion

        public void SetMovementData(PlayerMovementData movementData)
        {
            _movementData = movementData;
        }

        public void EnableMovement()
        {
            _isReadyToMove = true;
        }

        public void DisableMovement()
        {
            _isReadyToMove = false;
        }

        public void SetRunnerMovementValues(float X, float Z)
        {
            _inputValueX = X;
            _inputValueZ = Z;
        }

        public void UpdateRunnerInputValue(RunnerGameInputParams runnerGameInput)
        {
            _inputValueX = runnerGameInput.XValue;
            _clampValues = runnerGameInput.ClampValues;
        }

        public void UpdateIdleInputValue(IdleGameInputParams ıdleGameInputParams)
        {
            _inputValueX = ıdleGameInputParams.XValue;
            _inputValueZ = ıdleGameInputParams.ZValue;
        }

        public void IsReadyToPlay(bool state)
        {
            _isReadyToPlay = state;
            Stop();
        }

        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                if (_isReadyToMove)
                {
                    if(manager.CurrentGameState == GameStates.Runner)
                    {
                        RunnerMove();
                    }
                    if(manager.CurrentGameState == GameStates.Idle)
                    {
                        IdleMove();
                    }
                }
                else
                {
                    if (manager.CurrentGameState == GameStates.Runner)
                    {
                        StopSideways();
                    }
                    if (manager.CurrentGameState == GameStates.Idle)
                    {
                        Stop();
                    }
                }
            }
        }

        private void RunnerMove()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValueX * _movementData.SidewaysSpeed, velocity.y, _movementData.ForwardSpeed);
            rigidbody.velocity = velocity;

            Vector3 position = transform.position;
            position = new Vector3(Mathf.Clamp(rigidbody.position.x, _clampValues.x, _clampValues.y),position.y,position.z);
            rigidbody.position = position;
        }

        private void IdleMove()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValueX * _movementData.ForwardSpeed, velocity.y,_inputValueZ * _movementData.ForwardSpeed);
            rigidbody.velocity = velocity;
        }

        private void StopSideways()
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _movementData.ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }

        private void Stop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        public void StopVerticalMovement()
        {
            rigidbody.angularVelocity = Vector3.zero;
            _movementData.ForwardSpeed = 0;
        }

        public void StartVerticalMovement(GameObject other)
        {
            manager.transform.DOMove(new Vector3(0, manager.transform.position.y,
                other.transform.position.z + other.gameObject.transform.localScale.z), 2f).OnComplete(
                () =>
                {
                    _movementData.ForwardSpeed = 10;
                });
        }

        public void Reset()
        {
            Stop();
            _isReadyToMove = false;
            _isReadyToPlay = false;
            manager.transform.position = Vector3.zero;
            gameObject.transform.rotation = quaternion.identity;
        }


    }
}

