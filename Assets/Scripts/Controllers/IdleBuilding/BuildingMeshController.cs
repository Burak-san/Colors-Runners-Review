using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Managers;

namespace Controllers.IdleBuilding
{
    public class BuildingMeshController : MonoBehaviour
    {
        #region Self Variables
    
        #region Public Variables
          
        public float Saturation;
        
        #endregion

        #region Serialized Variables

        [SerializeField] private BuildingManager manager;
        [SerializeField] private List<MeshRenderer> renderers;

        #endregion
        #endregion

        public void CalculateSaturation()
        {
            Saturation = (float)manager.BuildingsData.PayedAmount / manager.BuildingsData.BuildingMarketPrice * 2f;
            SaturationChange(Saturation);
        }

        public void SaturationChange(float saturation)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                var matSaturation = renderers[i].material;
                matSaturation.DOFloat(saturation, "_Saturation", 1f);
            }
        }
    }
}