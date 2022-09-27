using Extentions;
using Keys;
using UnityEngine.Events;

namespace Signals
{
    public class InputSignals : MonoSingleton<InputSignals>
    {
        public UnityAction onInputTaken = delegate { };
        public UnityAction<RunnerGameInputParams> onRunnerInputDragged = delegate { };
        public UnityAction<IdleGameInputParams> onIdleInputDragged = delegate { };
        public UnityAction onInputReleased = delegate { };
    }
}