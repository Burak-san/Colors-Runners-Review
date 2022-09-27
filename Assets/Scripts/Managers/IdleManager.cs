using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Interface;
using Signals;
using UnityEngine;

namespace Managers
{
    public class IdleManager : MonoBehaviour,ISaveable
    {
        #region Self Variables

        #region Public Variables

        [Header("BuildingsData")] public IdleLevelData IdleLevelData;
        public List<BuildingManager> BuildingManagers = new List<BuildingManager>();

        public IdleLevelStateType IdleLevelStateType;

        #endregion

        #region Private Variables

        private int _idleLevelID;

        #endregion

        #endregion

        private IdleLevelData GetCityData() =>
            Resources.Load<CD_IdleLevel>("Data/CD_IdleLevel").IdleLevelList[_idleLevelID];

        private void GetIdleLevelData()
        {
            _idleLevelID = LevelSignals.Instance.onGetIdleLevelID.Invoke();
        }

        private void Awake()
        {
            GetIdleLevelData();
            SetData();
        }

        private void SetData()
        {
            if (!ES3.FileExists($"IdleLevelDataKey{_idleLevelID}.es3"))
            {
                if (!ES3.KeyExists("IdleLevelDataKey"))
                {
                    IdleLevelData = GetCityData();
                    GetIdleLevelData();
                    Save(_idleLevelID);
                }
            }
            Load(_idleLevelID);
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onApplicationQuit += OnSave;
            CoreGameSignals.Instance.onGamePause += OnSave;
            LevelSignals.Instance.onLevelInitialize += OnLoad;
            BuildingSignals.Instance.onBuildingsCompleted += OnSetBuildingStatus;
            BuildingSignals.Instance.onSideBuildingsCompleted += OnSetBuildingStatus;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onApplicationQuit -= OnSave;
            CoreGameSignals.Instance.onGamePause -= OnSave;
            LevelSignals.Instance.onLevelInitialize -= OnLoad;
            BuildingSignals.Instance.onBuildingsCompleted -= OnSetBuildingStatus;
            BuildingSignals.Instance.onSideBuildingsCompleted -= OnSetBuildingStatus;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        private void OnSave()
        {
            Save(_idleLevelID);
        }

        private void OnLoad()
        {
            Load(_idleLevelID);
        }

        private void OnSetBuildingStatus(int adressid)
        {
            IdleLevelData.CompletedCount++;
            CheckLevelStatus();
        }

        private void CheckLevelStatus()
        {
            Save(_idleLevelID);
            if (IdleLevelData.CompletedCount == BuildingManagers.Count)
            {
                IdleLevelData.IdleLevelStateType = IdleLevelStateType.Completed;
                Save(_idleLevelID);
                LevelSignals.Instance.onIdleLevelChange.Invoke();
            }
        }

        public void Save(int uniqueId)
        {
            IdleLevelData = new IdleLevelData(IdleLevelData.IdleLevelStateType, IdleLevelData.CompletedCount);
            SaveSignals.Instance.onSaveIdleLevelData.Invoke(IdleLevelData,uniqueId);
        }

        public void Load(int uniqueId)
        {
            IdleLevelData idleLevelData =
                SaveSignals.Instance.onLoadIdleLevelData.Invoke(IdleLevelData.key, uniqueId);

            IdleLevelData.IdleLevelStateType = idleLevelData.IdleLevelStateType;
            IdleLevelData.CompletedCount = idleLevelData.CompletedCount;
        }
    }
}