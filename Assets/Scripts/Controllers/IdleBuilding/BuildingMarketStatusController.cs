using Managers;
using TMPro;
using UnityEngine;

namespace Controllers.IdleBuilding
{
    public class BuildingMarketStatusController : MonoBehaviour
    {
        #region SelfVariables
    
        #region Public Variables
          
        public int PayedAmount;
        public int MarketPrice;
        
        #endregion

        #region Serialized Variables

        [SerializeField] private TextMeshPro marketPriceText;
        [SerializeField] private BuildingManager manager;
        
        #endregion
        #endregion

        private void SetRequiredAmountToText()
        {
            marketPriceText.text = $"{manager.BuildingsData.PayedAmount}/{manager.BuildingsData.BuildingMarketPrice}";
        }

        public void UpdatePayedAmountText(int payedAmount)
        {
            PayedAmount = payedAmount;
            SetRequiredAmountToText();
        }
    }
}