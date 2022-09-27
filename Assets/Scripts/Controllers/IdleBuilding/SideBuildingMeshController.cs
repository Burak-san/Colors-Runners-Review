using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Controllers.IdleBuilding
{
    public class SideBuildingMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public float Saturation;

        #endregion

        #region Serialized Variables

        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private List<MeshRenderer> meshRenderers;

        #endregion

        #endregion

        public void CalculateSturation()
        {
            Saturation = (float)buildingManager.BuildingsData.SideObject.PayedAmount /
                buildingManager.BuildingsData.SideObject.MarketPrice * 2f;
            SaturationChange(Saturation);
        }

        public void SaturationChange(float saturation)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                var matSaturation = meshRenderers[i].material;
                matSaturation.DOFloat(saturation, "_Saturation", 1f);
            }
        }
    }
}