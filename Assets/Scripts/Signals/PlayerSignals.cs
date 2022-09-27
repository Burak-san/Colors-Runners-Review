using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
        public UnityAction onIncreaseScale = delegate { };
        public UnityAction onDecreaseScale = delegate { };

    }
}