using Cinemachine;
using Enums;
using Extentions;
using Signals;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public CinemachineStateDrivenCamera StateCam;

        #region Serialized Variables

        [SerializeField] private LookCinemachineAxis lookCinemachineAxis;

        #endregion Serialized Variables

        #region Private Variables

        [ShowInInspector] private Vector3 _initialPosition;
        private Animator _animator;
        private CameraStatesType _cameraStatesType;
        private PlayerManager _playerManager;

        #endregion Private Variables

        #endregion Public Variables

        #endregion Self Variables

        private void Awake()
        {
            GetReferences();
            GetInitialPosition();
        }

        private void GetReferences()
        {
            _animator = GetComponent<Animator>();
            _cameraStatesType = CameraStatesType.Runner;
        }

        private void GetInitialPosition()
        {
            _initialPosition = transform.localPosition;
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            CoreGameSignals.Instance.onReset += OnReset;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            CoreGameSignals.Instance.onReset -= OnReset;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion Event Subscriptions

        private void OnPlay()
        {
            SetCameraTarget(CameraStatesType.Runner);
        }

        private void SetCameraTarget(CameraStatesType cameraCurrentState)
        {
            if (!_playerManager)
            {
                _playerManager = FindObjectOfType<PlayerManager>();
            }
            if (cameraCurrentState == CameraStatesType.Runner)
            {
                StateCam.Follow = _playerManager.transform;
                StateCam.LookAt = null;
                lookCinemachineAxis.enabled = true;
            }
            else
            {
                StateCam.LookAt = _playerManager.transform;
                lookCinemachineAxis.enabled = false;
            }
            _animator.Play(cameraCurrentState.ToString());
        }

        private void OnChangeGameState(GameStates currentGameState)
        {
            switch (currentGameState)
            {
                case GameStates.Roullette:
                    _cameraStatesType = CameraStatesType.Idle;
                    SetCameraTarget(_cameraStatesType);
                    break;

                case GameStates.Runner:
                    _cameraStatesType = CameraStatesType.Runner;
                    SetCameraTarget(_cameraStatesType);
                    break;
            }
        }

        private void OnNextLevel()
        {
            CameraTargetSetting();
        }

        private async void CameraTargetSetting()
        {
            await Task.Delay(50);
            SetCameraTarget(CameraStatesType.Runner);
        }

        private void OnReset()
        {
            CameraTargetSetting();
        }
    }
}