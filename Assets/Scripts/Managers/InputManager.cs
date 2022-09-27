using Commands.Input;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
using UnityEngine;
using Enums;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public InputData InputData;

        #endregion Public Variables

        #region Serialized Variables

        [SerializeField] private FloatingJoystick joystickInput;

        #endregion Serialized Variables

        #region Private Variables

        private GameStates _currentState;

        private RunnerGameInputUpdateCommand _runnerInputCommand;

        private IdleGameInputUpdateCommand _ıdleInputCommand;

        #endregion Private Variables

        #endregion Self Variables

        private void Awake()
        {
            _currentState = GameStates.Runner;
            InputData = GetInputData();
            Init();
        }


        private void Init()
        {
            _runnerInputCommand = new RunnerGameInputUpdateCommand();
            _ıdleInputCommand = new IdleGameInputUpdateCommand();
        }

        private InputData GetInputData() => Resources.Load<CD_Input>("Data/CD_Input").InputData;

        #region Event Subscriptions

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
        }

        #endregion Event Subscriptions

        private void Update()
        {
            if (_currentState == GameStates.Runner)
            {
                CurrentStateRunner();
            }

            if (_currentState == GameStates.Idle)
            {
                CurrentStateIdle();
            }
            
        }

        #region Subcribed Methods

        private void OnChangeGameState(GameStates currentState)
        {
            _currentState = currentState;
        }

        #endregion Subcribed Methods

        private void CurrentStateRunner()
        {
            if (_currentState == GameStates.Runner)
            {
                _runnerInputCommand.RunnerInputUpdate(joystickInput, InputData);
            }
        }

        private void CurrentStateIdle()
        {
            if (_currentState == GameStates.Idle)
            {
                _ıdleInputCommand.IdleInputUpdate(joystickInput);
            }
        }
    }
}