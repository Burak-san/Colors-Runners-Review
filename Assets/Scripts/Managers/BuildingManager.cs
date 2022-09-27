using System;
using Controllers.IdleBuilding;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Interface;
using Signals;
using UnityEngine;

namespace Managers
{
    public class BuildingManager : MonoBehaviour,ISaveable
    {
        #region Self Variables

        #region Public Variables

        public BuildingsData BuildingsData;
        public int BuildingAdressID;

        #endregion
        
        #region  Serialized Variables

        [SerializeField] private BuildingMarketStatusController buildingMarketStatusController;
        [SerializeField] private BuildingMeshController buildingMeshController;

        [SerializeField] private SideBuildingStatusController sideBuildingStatusController;
        [SerializeField] private SideBuildingMeshController sideBuildingMeshController;

        [SerializeField] private BuildingScorePhysicsController buildingScorePhysicsController;
        [SerializeField] private GameObject sideObject;
        

        #endregion

        #region Private Variables

        private int _idleLevelId;
        private Material _material;
        private string _stringUniqueID;
        private int _uniqueID;
        private int _one = 1;

        #endregion
        #endregion

        private BuildingsData GetBuildingsData()
        {
            return Resources.Load<CD_IdleLevel>("Data/CD_IdleLevel").IdleLevelList[_idleLevelId]
                .BuildingsDatas[BuildingAdressID];
        }

        private void GetIdleLevelData()
        {
            _idleLevelId = LevelSignals.Instance.onGetIdleLevelID.Invoke();
        }
        
        private void Start()
        {
            GetIdleLevelData();
            GetReferences();
            SetData();
            CheckBuildingScoreStatus(BuildingsData.idleLevelState);

            if (BuildingsData.IsDepended && BuildingsData.idleLevelState ==IdleLevelStateType.Completed)
            {
                CheckSideBuildingScoreStatus(BuildingsData.SideObject.IdleLevelStateType);
            }
            SendDataToControllers();
        }

        private void GetReferences()
        {
            BuildingsData = GetBuildingsData();
            _stringUniqueID = _one.ToString() + _idleLevelId.ToString() + BuildingAdressID.ToString();
            int.TryParse(_stringUniqueID, out _uniqueID);
        }

        private void SetData()
        {
            if (!ES3.FileExists($"IdleBuildingDataKey{_uniqueID}.es3"))
            {
                if (!ES3.KeyExists("IdleBuildingDataKey"))
                {
                    BuildingsData = GetBuildingsData();
                    Save(_uniqueID);
                }
            }
            Load(_uniqueID);
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
            LevelSignals.Instance.onNextLevel += OnSave;
            LevelSignals.Instance.onLevelInitialize += OnLoad;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onApplicationQuit -= OnSave;
            CoreGameSignals.Instance.onGamePause -= OnSave;
            LevelSignals.Instance.onNextLevel -= OnSave;
            LevelSignals.Instance.onLevelInitialize -= OnLoad;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        private void OnSave()
        {
            Save(_uniqueID);
            SendDataToControllers();
        }

        private void OnLoad()
        {
            Load(_uniqueID);
            SendDataToControllers();
        }
        private void SendDataToControllers()
        {
            buildingMarketStatusController.UpdatePayedAmountText(BuildingsData.PayedAmount);
            buildingMeshController.Saturation = BuildingsData.Saturation;
            UpdateSaturation();

            if (BuildingsData.IsDepended)
            {
                sideBuildingStatusController.UpdatePayedAmountText(BuildingsData.SideObject.PayedAmount);
                sideBuildingMeshController.Saturation = BuildingsData.SideObject.Saturation;
                UpdateSideBuildingSaturation();
            }
        }

        private void UpdateSaturation()
        {
            buildingMeshController.CalculateSaturation();
        }

        private void UpdateSideBuildingSaturation()
        {
            sideBuildingMeshController.CalculateSturation();
        }
        
        public void UpdatePayedAmount()
        {
            var payedAmount = BuildingsData.PayedAmount++;
            buildingMarketStatusController.UpdatePayedAmountText(BuildingsData.PayedAmount);
            UpdateSaturation();
        }

        public void UpdateSidePayedAmount()
        {
            var sidePayedAmount = BuildingsData.SideObject.PayedAmount++;
            sideBuildingStatusController.UpdatePayedAmountText(BuildingsData.SideObject.PayedAmount);
            UpdateSideBuildingSaturation();
        }

        public void UpdateBuildingStatus(IdleLevelStateType idleLevelState)
        {
            BuildingsData.idleLevelState = idleLevelState;
            if (!BuildingsData.IsDepended)
            {
                BuildingSignals.Instance.onBuildingsCompleted.Invoke(BuildingsData.BuildingAdressId);
            }
        }

        public void UpdateSideBuildingStatus(IdleLevelStateType idleLevelState)
        {
            BuildingsData.SideObject.IdleLevelStateType = idleLevelState;
            BuildingSignals.Instance.onSideBuildingsCompleted.Invoke(BuildingsData.SideObject.BuildingAddressId);
        }

        public void CheckBuildingScoreStatus(IdleLevelStateType idleLevelStateType)
        {
            if (idleLevelStateType == IdleLevelStateType.Completed)
            {
                buildingMarketStatusController.gameObject.SetActive(false);
            }
            else
            {
                buildingMarketStatusController.gameObject.SetActive(true);
            }
        }

        public void CheckSideBuildingScoreStatus(IdleLevelStateType idleLevelStateType)
        {
            if (idleLevelStateType == IdleLevelStateType.Completed)
            {
                sideBuildingStatusController.gameObject.SetActive(false);
            }
            else
            {
                sideBuildingStatusController.gameObject.SetActive(true);
            }
        }

        public void OpenSideObject()
        {
            sideObject.SetActive(true);
        }

        public void Save(int uniqueId)
        {
            BuildingsData = new BuildingsData(BuildingsData.IsDepended,
                BuildingsData.SideObject,
                    BuildingAdressID,
                        BuildingsData.BuildingMarketPrice,
                            BuildingsData.PayedAmount,
                                BuildingsData.Saturation,
                                    BuildingsData.idleLevelState);
            SaveSignals.Instance.onSaveBuildingsData.Invoke(BuildingsData,uniqueId);
        }

        public void Load(int uniqueId)
        {
            BuildingsData _buildingsData =
                SaveSignals.Instance.onLoadBuildingsData.Invoke(BuildingsData.Key, uniqueId);
            BuildingsData.SideObject = _buildingsData.SideObject;
            BuildingsData.Saturation = _buildingsData.Saturation;
            BuildingsData.PayedAmount = _buildingsData.PayedAmount;
            BuildingsData.idleLevelState = _buildingsData.idleLevelState;
            BuildingsData.BuildingMarketPrice = _buildingsData.BuildingMarketPrice;
            BuildingsData.IsDepended = _buildingsData.IsDepended;
        }
    }
}