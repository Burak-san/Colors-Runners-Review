using System;
using Data.ValueObject;
using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class SaveSignals : MonoSingleton<SaveSignals>
    {
        public UnityAction<LevelIdData, int> onSaveGameData = delegate { };
        public UnityAction<IdleLevelData, int> onSaveIdleLevelData = delegate { };
        public UnityAction<BuildingsData, int> onSaveBuildingsData = delegate { };

        public Func<string, int, LevelIdData> onLoadGameData;
        public Func<string, int, IdleLevelData> onLoadIdleLevelData;
        public Func<string, int, BuildingsData> onLoadBuildingsData;

    }
}