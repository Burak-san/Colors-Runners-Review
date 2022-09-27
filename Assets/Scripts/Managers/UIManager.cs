using Commands.UI;
using Controllers.UI;
using DG.Tweening;
using Enums;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables



        #region Serializefield Variables
        [SerializeField] private UIPanelController uIPanelController;
        [SerializeField] private GameObject joystickInner;
        [SerializeField] private GameObject joystickOuter;
        [SerializeField] private TextMeshProUGUI leveltext;
        [SerializeField] private TextMeshProUGUI leveltextIdle;
        [SerializeField] private RectTransform roulletteArrowRectTransform;

        #endregion Serializefield Variables

        #region Private Variables

        private JoyStickStateCommand _joyStickStateCommand;
        private int _multiply;

        #endregion Private Variables

        #endregion Self Variables

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            CursorMovement();
            _joyStickStateCommand = new JoyStickStateCommand();
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onOpenPanel += OnOpenPanel;
            UISignals.Instance.onClosePanel += OnClosePanel;
            UISignals.Instance.onSetLevelText += OnSetLevelText;

            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;

            LevelSignals.Instance.onLevelFailed += OnLevelFailed;
        }

        private void UnSubscribeEvents()
        {
            UISignals.Instance.onOpenPanel -= OnOpenPanel;
            UISignals.Instance.onClosePanel -= OnClosePanel;
            UISignals.Instance.onSetLevelText -= OnSetLevelText;

            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;

            LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion Event Subscriptions

        private void OnOpenPanel(UIPanels panel)
        {
            uIPanelController.OpenPanel(panel);
        }

        private void OnClosePanel(UIPanels panel)
        {
            uIPanelController.ClosePanel(panel);
        }

        private void OnPlay()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.StartPanel);
        }

        public void PlayButton()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();

        }

        public void ClaimButton()
        {
            CursorSelect();
            NoThanksButton();
        }

        public void CursorSelect()
        {
            float cursorPos = roulletteArrowRectTransform.localPosition.x;

            if (250 <= cursorPos && cursorPos < 500)
            {
                _multiply = 2;
            }
            else if (120 <= cursorPos && cursorPos < 250)
            {
                _multiply = 3;
            }
            else if (-120 <= cursorPos && cursorPos < 120)
            {
                _multiply = 5;
            }
            else if (-250 <= cursorPos && cursorPos < -120)
            {
                _multiply = 3;
            }
            else if (-500 < cursorPos && cursorPos <= -250)
            {
                _multiply = 2;
            }
            ScoreSignals.Instance.onMultiplyScore?.Invoke(_multiply);
        }

        public void NoThanksButton()
        {
            OnClosePanel(UIPanels.RoullettePanel);
            OnClosePanel(UIPanels.LevelPanel);
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Idle);
            OnOpenPanel(UIPanels.IdlePanel);
        }

        public void RestartButton()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.IdlePanel);
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.RoullettePanel);
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.FailPanel);
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Runner);
        }

        public void NextLevelButton()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.IdlePanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.StartPanel);
            LevelSignals.Instance.onNextLevel?.Invoke();
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Runner);
        }

        private void OnChangeGameState(GameStates CurrentStates)
        {
            ChangeUIState(CurrentStates);
        }

        private void ChangeUIState(GameStates CurrentState)
        {
            switch (CurrentState)
            {
                case GameStates.Roullette:
                    OnOpenPanel(UIPanels.RoullettePanel);
                    //CursorMovement();
                    break;

                case GameStates.Idle:
                    OnOpenPanel(UIPanels.IdlePanel);
                    _joyStickStateCommand.JoyStickUIStateChanger(CurrentState, joystickOuter, joystickInner);
                    break;

                case GameStates.Runner:
                    _joyStickStateCommand.JoyStickUIStateChanger(CurrentState, joystickOuter, joystickInner);
                    break;
            }
        }
        
        private void CursorMovement()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(roulletteArrowRectTransform.DORotate(new Vector3(0, 0, 30), 1f)
                .SetEase(Ease.Linear)).SetLoops(-1, LoopType.Yoyo);
            sequence.Join(roulletteArrowRectTransform.DOLocalMoveX(-500f, 1f))
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnSetLevelText(int levelID)
        {
            if (levelID == 0)
            {
                levelID = 1;
                leveltext.text = "level " + levelID.ToString();
                leveltextIdle.text = "level " + levelID.ToString();
            }
            else
            {
                leveltext.text = "level " + levelID.ToString();
                leveltextIdle.text = "level " + levelID.ToString();
            }
            OnOpenPanel(UIPanels.LevelPanel);
        }

        private void OnLevelFailed()
        {
            OnOpenPanel(UIPanels.FailPanel);
        }

        private void OnReset()
        {
            
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.StartPanel);
        }
    }
}