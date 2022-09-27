using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class TurretAreaSignals : MonoSingleton<TurretAreaSignals>
    {
        public UnityAction<GameObject> onTurretDetect = delegate {  };
        public UnityAction onExitTurretArea = delegate {  };
    }
}