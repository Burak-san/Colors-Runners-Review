using Managers;
using TMPro;
using UnityEngine;

namespace Controllers.IdleBuilding
{
    public class SideBuildingStatusController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public int PayedAmount;
        public int MarketPrice;

        #endregion

        #region Serialized Variables

        [SerializeField] private TextMeshPro marketPriceText;
        [SerializeField] private BuildingManager buildingManager;

        #endregion

        #endregion

        private void SetRequiredAmountToText()
        {
            marketPriceText.text =
                $"{buildingManager.BuildingsData.SideObject.PayedAmount}/{buildingManager.BuildingsData.SideObject.MarketPrice}";
        }

        public void UpdatePayedAmountText(int payedAmount)
        {
            PayedAmount = payedAmount;
            SetRequiredAmountToText();
        }
    }
}